namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class BinaryOperatorExpression : IExpression<bool>
{
    private static readonly string[] _operatorExpressions = ["&&", "||"];

    public int Order => 20; // important: after ComparisonExpression. if the expression is recognized as a ComparisonExpression, it may contain binary operators as combinations

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
        => EvaluateTyped(context).Transform<object?>(x => x);

    public static Result<bool> Evaluate(ExpressionEvaluatorContext context, Result<BinaryConditionGroup> comparisonResult)
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

        return Evaluate(context, conditionsResult.Transform(x => new BinaryConditionGroup(x)));
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var conditionsResult = ParseConditions(context);
        if (!conditionsResult.IsSuccessful() || conditionsResult.Status == ResultStatus.Continue)
        {
            return Result.FromExistingResult<Type>(conditionsResult);
        }

        return ValidateConditions(context, conditionsResult.Value!);
    }

    private static Result<List<BinaryCondition>> ParseConditions(ExpressionEvaluatorContext context)
    {
        var conditions = new List<BinaryCondition>();
        var combination = string.Empty;

        var foundAnyComparisonCharacter = context.FindAllOccurencedNotWithinQuotes(_operatorExpressions, StringComparison.Ordinal);
        if (!foundAnyComparisonCharacter)
        {
            return Result.Continue<List<BinaryCondition>>();
        }

        // First get array of parts
        var parts = context.Expression.SplitDelimited(_operatorExpressions, '"', true, true, true, StringComparison.OrdinalIgnoreCase);
        if (parts.Length <= 1)
        {
            // Messed up expression! Should always be <left expression> <operator>
            return Result.Continue<List<BinaryCondition>>();
        }

        // Now, we need to convert this array of parts to conditions
        var itemCountIsCorrect = (parts.Length - 3) % 4 == 0;
        if (!itemCountIsCorrect)
        {
            return Result.Invalid<List<BinaryCondition>>("Comparison expression has invalid number of parts");
        }

        for (var i = 0; i < parts.Length; i += 2)
        {
            //-parts[i + 0] is the left expression
            //-parts[i + 1] is the combination for the next condition
            var expression = parts[i];
            var startGroup = false;
            var endGroup = false;

            //remove brackets and set bracket property values for this query item.
            if (expression.StartsWith("("))
            {
                expression = expression.Substring(1);
                startGroup = true;
            }

            if (expression.EndsWith(")"))
            {
                expression = expression.Substring(0, expression.Length - 1);
                endGroup = true;
            }

            var condition = new BinaryConditionBuilder()
                .WithCombination(combination.ToUpperInvariant() switch
                {
                    "&&" => Combination.And,
                    "||" => Combination.Or,
                    _ => default(Combination?)
                })
                .WithExpression(expression)
                .WithStartGroup(startGroup)
                .WithEndGroup(endGroup)
                .Build();

            combination = parts.Length > i + 1
                ? parts[i + 1]
                : string.Empty;

            conditions.Add(condition);
        }

        return Result.Success(conditions);
    }

    private static bool ConditionsAreSimple(IEnumerable<BinaryCondition> conditions)
        => !conditions.Any(x =>
            (x.Combination ?? Combination.And) == Combination.Or
            || x.StartGroup
            || x.EndGroup);

    private static Result<bool> EvaluateSimpleConditions(ExpressionEvaluatorContext context, IEnumerable<BinaryCondition> conditions)
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

    private static Result<bool> EvaluateComplexConditions(ExpressionEvaluatorContext context, IEnumerable<BinaryCondition> conditions)
    {
        var builder = new StringBuilder();
        foreach (var evaluatable in conditions)
        {
            if (builder.Length > 0)
            {
                builder.Append(evaluatable.Combination == Combination.Or ? "|" : "&");
            }

            var prefix = evaluatable.StartGroup ? "(" : string.Empty;
            var suffix = evaluatable.EndGroup ? ")" : string.Empty;
            var itemResult = EvaluateCondition(evaluatable, context);
            if (!itemResult.IsSuccessful())
            {
                return itemResult;
            }
            builder.Append(prefix)
                   .Append(itemResult.Value ? "T" : "F")
                   .Append(suffix);
        }

        return Result.Success(OperatorExpression.EvaluateBooleanExpression(builder.ToString()));
    }

    private static Result<bool> EvaluateCondition(BinaryCondition condition, ExpressionEvaluatorContext context)
    {
        var expressionResult = context.Evaluate(condition.Expression);
        if (!expressionResult.IsSuccessful())
        {
            return Result.FromExistingResult<bool>(expressionResult);
        }

        if (expressionResult.Value is bool b)
        {
            return Result.Success(b);
        }
        else if (expressionResult.Value is string s)
        {
            // desin decision: if it's a string, then do a null or empty check
            return Result.Success(!string.IsNullOrEmpty(s));
        }
        else
        {
            // design decision: if it's not a boolean, then do a null check
            return Result.Success(expressionResult.Value is not null);
        }
    }

    private static Result<Type> ValidateConditions(ExpressionEvaluatorContext context, IEnumerable<BinaryCondition> conditions)
    {
        foreach (var evaluatable in conditions)
        {
            var itemResult = ValidateCondition(evaluatable, context);
            if (!itemResult.IsSuccessful())
            {
                return itemResult;
            }
        }

        return Result.Success(typeof(bool));
    }

    private static Result<Type> ValidateCondition(BinaryCondition condition, ExpressionEvaluatorContext context)
    {
        var expressionResult = context.Validate(condition.Expression);
        if (!expressionResult.IsSuccessful())
        {
            return expressionResult;
        }

        return Result.Success(typeof(bool));
    }
}
