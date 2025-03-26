namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class ComparisonOperatorExpression : IExpression<bool>
{
    private readonly string[] _delimiters;
    private readonly string[] _operatorExpressions;
    private readonly IOperator[] _operators;

    public ComparisonOperatorExpression(IEnumerable<IOperator> operators)
    {
        operators = ArgumentGuard.IsNotNull(operators, nameof(operators));

        _operators = operators.OrderBy(x => x.Order).ToArray();
        _delimiters = _operators.Select(x => x.OperatorExpression).Concat(["&&", "||"]).ToArray();
        _operatorExpressions = _operators.Select(x => x.OperatorExpression).ToArray();
    }

    public int Order => 20;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
        => EvaluateTyped(context).Transform<object?>(x => x);

    public static Result<bool> Evaluate(ExpressionEvaluatorContext context, Result<ComparisonConditionGroup> comparisonResult)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        comparisonResult = ArgumentGuard.IsNotNull(comparisonResult, nameof(comparisonResult));

        if (!comparisonResult.IsSuccessful())
        {
            return Result.FromExistingResult<bool>(comparisonResult);
        }

        if (ConditionsAreSimple(comparisonResult.Value!.Conditions))
        {
            return EvaluateSimpleConditions(context, comparisonResult.Value!.Conditions);
        }

        return EvaluateComplexConditions(context, comparisonResult.Value!.Conditions);
    }

    public Result<bool> EvaluateTyped(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var conditionsResult = ParseConditions(context);
        if (!conditionsResult.IsSuccessful() || conditionsResult.Status == ResultStatus.Continue)
        {
            return Result.FromExistingResult<bool>(conditionsResult);
        }

        return Evaluate(context, conditionsResult.Transform(x => new ComparisonConditionGroup(x)));
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var conditionsResult = ParseConditions(context);
        if (!conditionsResult.IsSuccessful() || conditionsResult.Status == ResultStatus.Continue)
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(conditionsResult.Status)
                .WithErrorMessage(conditionsResult.ErrorMessage)
                .AddValidationErrors(conditionsResult.ValidationErrors);
        }

        return ParseConditionExpressions(context, conditionsResult.Value!);
    }

    private Result<List<ComparisonCondition>> ParseConditions(ExpressionEvaluatorContext context)
    {
        var conditions = new List<ComparisonCondition>();
        var combination = string.Empty;

        var foundAnyComparisonCharacter = context.FindAllOccurencedNotWithinQuotes(_operatorExpressions, StringComparison.Ordinal);
        if (!foundAnyComparisonCharacter)
        {
            return Result.Continue<List<ComparisonCondition>>();
        }

        // First get array of parts
        var parts = context.Expression.SplitDelimited(_delimiters, '"', true, true, true, StringComparison.OrdinalIgnoreCase);
        if (parts.Length <= 2)
        {
            // Messed up expression! Should always be <left expression> <operator> <right expression>
            return Result.Continue<List<ComparisonCondition>>();
        }

        // Now, we need to convert this array of parts to conditions
        var itemCountIsCorrect = (parts.Length - 3) % 4 == 0;
        if (!itemCountIsCorrect)
        {
            return Result.Invalid<List<ComparisonCondition>>("Comparison expression has invalid number of parts");
        }

        for (var i = 0; i < parts.Length; i += 4)
        {
            //-parts[i + 0] is the left expression
            //-parts[i + 1] is the operator
            //-parts[i + 2] is the right expression
            //-parts[i + 3] is the combination for the next condition
            var leftExpression = parts[i];
            var @operator = parts[i + 1];
            var rightExpression = parts[i + 2];
            var startGroup = false;
            var endGroup = false;

            //remove brackets and set bracket property values for this query item.
            if (leftExpression.StartsWith("("))
            {
                leftExpression = leftExpression.Substring(1);
                startGroup = true;
            }

            if (rightExpression.EndsWith(")"))
            {
                rightExpression = rightExpression.Substring(0, rightExpression.Length - 1);
                endGroup = true;
            }

            var queryOperator = GetOperator(@operator);
            if (queryOperator is null)
            {
                // Messed up expression! Should always be <left expression> <operator> <right expression>
                return Result.Invalid<List<ComparisonCondition>>("Comparison expression is malformed");
            }

            var condition = new ComparisonConditionBuilder()
                .WithCombination(combination.ToUpperInvariant() switch
                {
                    "&&" => Combination.And,
                    "||" => Combination.Or,
                    _ => default(Combination?)
                })
                .WithLeftExpression(leftExpression)
                .WithOperator(queryOperator)
                .WithRightExpression(rightExpression)
                .WithStartGroup(startGroup)
                .WithEndGroup(endGroup)
                .Build();

            combination = parts.Length > i + 3
                ? parts[i + 3]
                : string.Empty;

            conditions.Add(condition);
        }

        return Result.Success(conditions);
    }

    private static ExpressionParseResult ParseConditionExpressions(ExpressionEvaluatorContext context, IEnumerable<ComparisonCondition> conditions)
    {
        var result = new ExpressionParseResultBuilder()
            .WithExpressionType(typeof(ComparisonOperatorExpression))
            .WithResultType(typeof(bool))
            .WithSourceExpression(context.Expression);

        var counter = 0;
        foreach (var condition in conditions)
        {
            result.AddPartResult(context.Parse(condition.LeftExpression), $"Conditions[{counter}].LeftExpression");
            result.AddPartResult(context.Parse(condition.RightExpression), $"Conditions[{counter}].RightExpression");
            counter++;
        }

        return result.DetectStatusFromPartResults();
    }

    private IOperator GetOperator(string @operator)
        => _operators.FirstOrDefault(x => x.OperatorExpression.Equals(@operator.Trim(), StringComparison.OrdinalIgnoreCase));

    private static bool ConditionsAreSimple(IEnumerable<ComparisonCondition> conditions)
        => !conditions.Any(x =>
            (x.Combination ?? Combination.And) == Combination.Or
            || x.StartGroup
            || x.EndGroup);

    private static Result<bool> EvaluateSimpleConditions(ExpressionEvaluatorContext context, IEnumerable<ComparisonCondition> conditions)
    {
        foreach (var condition in conditions)
        {
            var itemResult = EvaluateCondition(condition, context);
            if (!itemResult.IsSuccessful())
            {
                return itemResult;
            }

            if (!itemResult.Value)
            {
                return itemResult;
            }
        }

        return Result.Success(true);
    }

    private static Result<bool> EvaluateComplexConditions(ExpressionEvaluatorContext context, IEnumerable<ComparisonCondition> conditions)
    {
        var builder = new StringBuilder();
        foreach (var condition in conditions)
        {
            var itemResult = EvaluateCondition(condition, context);
            if (!itemResult.IsSuccessful())
            {
                return itemResult;
            }

            OperatorExpression.AppendCondition(builder, condition.Combination, condition.StartGroup, condition.EndGroup, itemResult.Value);
        }

        return Result.Success(OperatorExpression.EvaluateBooleanExpression(builder.ToString()));
    }

    private static Result<bool> EvaluateCondition(ComparisonCondition condition, ExpressionEvaluatorContext context)
    {
        var results = new ResultDictionaryBuilder()
            .Add(Constants.LeftExpression, () => context.Evaluate(condition.LeftExpression))
            .Add(Constants.RightExpression, () => context.Evaluate(condition.RightExpression))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return Result.FromExistingResult<bool>(error);
        }

        return condition.Operator.Evaluate(new OperatorContextBuilder().WithResults(results).WithStringComparison(context.Settings.StringComparison));
    }
}
