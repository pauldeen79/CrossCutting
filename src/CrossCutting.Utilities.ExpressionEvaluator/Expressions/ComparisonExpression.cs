namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class ComparisonExpression : IExpression<bool>
{
    private const string LeftExpression = nameof(LeftExpression);
    private const string RightExpression = nameof(RightExpression);
    private static readonly Dictionary<string, Func<OperatorContext, Result<bool>>> Operators = new Dictionary<string, Func<OperatorContext, Result<bool>>>
    {
        { "<=", @operator => SmallerOrEqualThan.Evaluate(@operator.Results.GetValue(LeftExpression), @operator.Results.GetValue(RightExpression)) },
        { ">=", @operator => GreaterOrEqualThan.Evaluate(@operator.Results.GetValue(LeftExpression), @operator.Results.GetValue(RightExpression)) },
        { "<", @operator => SmallerThan.Evaluate(@operator.Results.GetValue(LeftExpression), @operator.Results.GetValue(RightExpression)) },
        { ">", @operator => GreaterThan.Evaluate(@operator.Results.GetValue(LeftExpression), @operator.Results.GetValue(RightExpression)) },
        { "==", @operator => Equal.Evaluate(@operator.Results.GetValue(LeftExpression), @operator.Results.GetValue(RightExpression), @operator.StringComparison) },
        { "!=", @operator => NotEqual.Evaluate(@operator.Results.GetValue(LeftExpression), @operator.Results.GetValue(RightExpression), @operator.StringComparison) },
    };

    private static readonly string[] Delimiters = Operators.Keys.Concat([" AND ", " OR "]).ToArray();

    public int Order => 10;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
        => EvaluateTyped(context).Transform<object?>(x => x);

    public Result<bool> EvaluateTyped(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var conditionsResult = ParseConditions(context);
        if (!conditionsResult.IsSuccessful() || conditionsResult.Status == ResultStatus.Continue)
        {
            return Result.FromExistingResult<bool>(conditionsResult);
        }

        return Evaluate(context, conditionsResult.Transform(x => new Comparison(x)));
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

    public static Result<bool> Evaluate(ExpressionEvaluatorContext context, Result<Comparison> comparisonResult)
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

    private static Result<List<Condition>> ParseConditions(ExpressionEvaluatorContext context)
    {
        var conditions = new List<Condition>();
        var combination = string.Empty;

        var foundAnyComparisonCharacter = context.FindAllOccurencedNotWithinQuotes(Operators.Select(x => x.Key), StringComparison.Ordinal);
        if (!foundAnyComparisonCharacter)
        {
            return Result.Continue<List<Condition>>();
        }

        // First get array of parts
        var parts = context.Expression.SplitDelimited(Delimiters, '"', true, true, true, StringComparison.OrdinalIgnoreCase);
        if (parts.Length <= 2)
        {
            // Messed up expression! Should always be <left expression> <operator> <right expression>
            return Result.Continue<List<Condition>>();
        }

        // Now, we need to convert this array of parts to conditions
        var itemCountIsCorrect = (parts.Length - 3) % 4 == 0;
        if (!itemCountIsCorrect)
        {
            return Result.Invalid<List<Condition>>("Comparison expression has invalid number of parts");
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

            var queryOperatorSuccess = Operators.ContainsKey(@operator);
            if (!queryOperatorSuccess)
            {
                // Messed up expression! Should always be <left expression> <operator> <right expression>
                return Result.Invalid<List<Condition>>("Comparison expression is malformed");
            }

            var condition = new ConditionBuilder()
                .WithCombination(combination.ToUpperInvariant() switch
                {
                    "AND" => Combination.And,
                    "OR" => Combination.Or,
                    _ => default(Combination?)
                })
                .WithLeftExpression(leftExpression)
                .WithOperator(@operator)
                .WithRightExpression(rightExpression)
                .WithStartGroup(startGroup)
                .WithEndGroup(endGroup)
                .Build();

            combination = parts.Length > i + 3
                ? parts[i + 3].Trim()
                : string.Empty;

            conditions.Add(condition);
        }

        return Result.Success(conditions);
    }

    private static bool ConditionsAreSimple(IEnumerable<Condition> conditions)
        => !conditions.Any(x =>
            (x.Combination ?? Combination.And) == Combination.Or
            || x.StartGroup
            || x.EndGroup);

    private static Result<bool> EvaluateSimpleConditions(ExpressionEvaluatorContext context, IEnumerable<Condition> conditions)
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

    private static Result<bool> EvaluateComplexConditions(ExpressionEvaluatorContext context, IEnumerable<Condition> conditions)
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

        return Result.Success(EvaluateBooleanExpression(builder.ToString()));
    }

    private static Result<bool> EvaluateCondition(Condition condition, ExpressionEvaluatorContext context)
    {
        var results = new ResultDictionaryBuilder()
            .Add(LeftExpression, () => context.Evaluator.Evaluate(condition.LeftExpression, context.Settings, context.Context))
            .Add(RightExpression, () => context.Evaluator.Evaluate(condition.RightExpression, context.Settings, context.Context))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return Result.FromExistingResult<bool>(error);
        }

        return Operators[condition.Operator].Invoke(new OperatorContextBuilder().WithResults(results).WithStringComparison(context.Settings.StringComparison));
    }

    private static Result<Type> ValidateConditions(ExpressionEvaluatorContext context, IEnumerable<Condition> conditions)
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

    private static Result<Type> ValidateCondition(Condition condition, ExpressionEvaluatorContext context)
    {
        var results = new ResultDictionaryBuilder()
            .Add(LeftExpression, () => context.Evaluator.Validate(condition.LeftExpression, context.Settings, context.Context))
            .Add(RightExpression, () => context.Evaluator.Validate(condition.RightExpression, context.Settings, context.Context))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return Result.FromExistingResult<Type>(error);
        }

        return Result.NoContent<Type>();
    }

    private static bool EvaluateBooleanExpression(string expression)
    {
        var result = ProcessRecursive(ref expression);

        var @operator = "&";
        foreach (var character in expression)
        {
            bool currentResult;
            switch (character)
            {
                case '&':
                    @operator = "&";
                    break;
                case '|':
                    @operator = "|";
                    break;
                case 'T':
                case 'F':
                    currentResult = character == 'T';
                    result = @operator == "&"
                        ? result && currentResult
                        : result || currentResult;
                    break;
            }
        }

        return result;
    }

    private static bool ProcessRecursive(ref string expression)
    {
        var result = true;
        var openIndex = -1;
        int closeIndex;
        do
        {
            closeIndex = expression.IndexOf(")");
            if (closeIndex > -1)
            {
                openIndex = expression.LastIndexOf("(", closeIndex);
                if (openIndex > -1)
                {
                    result = EvaluateBooleanExpression(expression.Substring(openIndex + 1, closeIndex - openIndex - 1));
                    expression = string.Concat(GetPrefix(expression, openIndex),
                                               GetCurrent(result),
                                               GetSuffix(expression, closeIndex));
                }
            }
        } while (closeIndex > -1 && openIndex > -1);
        return result;
    }

    private static string GetPrefix(string expression, int openIndex)
        => openIndex == 0
            ? string.Empty
            : expression.Substring(0, openIndex - 2);

    private static string GetCurrent(bool result)
        => result
            ? "T"
            : "F";

    private static string GetSuffix(string expression, int closeIndex)
        => closeIndex == expression.Length
            ? string.Empty
            : expression.Substring(closeIndex + 1);
}
