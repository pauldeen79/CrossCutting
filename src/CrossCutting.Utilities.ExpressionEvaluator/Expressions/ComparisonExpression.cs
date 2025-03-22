namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class ComparisonExpression : IExpression
{
    private static readonly char[] ComparisonChars = ['<', '>', '=', '!'];
    private static readonly string[] Operators = ["<=", ">=", "<", ">", "==", "!="];
    private static readonly string[] Delimiters = ["<=", ">=", "<", ">", "==", "!=", "AND", "OR"];

    private static readonly IEnumerable<IOperator> _operators =
    [
        new SmallerThanOrEqualOperator(),
        new GreaterThanOrEqualOperator(),
        new SmallerThanOperator(),
        new GreaterThanOperator(),
        new EqualsOperator(),
        new NotEqualsOperator()
    ];

    public int Order => 10;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (!context.Expression.Any(ComparisonChars.Contains))
        {
            return Result.Continue<object?>();
        }

        var parts = context.Expression.SplitDelimited(Delimiters, '"', true, true, true, StringComparison.OrdinalIgnoreCase);
        if (parts.Length <= 2)
        {
            // Messed up expression! Should always be <left expression> <operator> <right expression>
            return Result.Continue<object?>();
        }

        var itemCountIsCorrect = (parts.Length - 3) % 4 == 0;
        if (!itemCountIsCorrect)
        {
            return Result.Invalid<object?>("Comparison expression has invalid number of parts");
        }

        // Now, we need to convert this array of parts to conditions
        var conditionsResult = ParseConditions(parts);
        if (!conditionsResult.IsSuccessful())
        {
            return conditionsResult.Transform<object?>(x => x);
        }

        if (CanEvaluateSimpleConditions(conditionsResult.Value!))
        {
            return EvaluateSimpleConditions(context, conditionsResult.Value!);
        }

        return EvaluateComplexConditions(context, conditionsResult.Value!);
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));

        return Result.NotImplemented<Type>();
    }

    private static Result<List<Condition>> ParseConditions(string[] parts)
    {
        var conditions = new List<Condition>();
        var combination = string.Empty;

        for (var i = 0; i < parts.Length; i += 4)
        {
            //verify that:
            //-parts[i] is the left expression
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

            var queryOperator = Operators.FirstOrDefault(x => x == @operator);
            if (queryOperator is null)
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

    private static bool CanEvaluateSimpleConditions(IEnumerable<Condition> conditions)
        => !conditions.Any(x =>
            (x.Combination ?? Combination.And) == Combination.Or
            || x.StartGroup
            || x.EndGroup
        );

    private static Result<object?> EvaluateSimpleConditions(ExpressionEvaluatorContext context, IEnumerable<Condition> conditions)
    {
        foreach (var evaluatable in conditions)
        {
            var itemResult = IsItemValid(evaluatable, context);
            if (!itemResult.IsSuccessful())
            {
                return itemResult.Transform<object?>(x => x);
            }

            if (!itemResult.Value)
            {
                return itemResult.Transform<object?>(x => x);
            }
        }

        return Result.Success<object?>(true);
    }

    private static Result<object?> EvaluateComplexConditions(ExpressionEvaluatorContext context, IEnumerable<Condition> conditions)
    {
        var builder = new StringBuilder();
        foreach (var evaluatable in conditions)
        {
            if (builder.Length > 0)
            {
                builder.Append(evaluatable.Combination == Combination.And ? "&" : "|");
            }

            var prefix = evaluatable.StartGroup ? "(" : string.Empty;
            var suffix = evaluatable.EndGroup ? ")" : string.Empty;
            var itemResult = IsItemValid(evaluatable, context);
            if (!itemResult.IsSuccessful())
            {
                return itemResult.Transform<object?>(x => x);
            }
            builder.Append(prefix)
                   .Append(itemResult.Value ? "T" : "F")
                   .Append(suffix);
        }

        return Result.Success<object?>(EvaluateBooleanExpression(builder.ToString()));
    }

    private static Result<bool> IsItemValid(Condition condition, ExpressionEvaluatorContext context)
        => _operators
            .Select(x => x.Evaluate(condition, context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotSupported<bool>($"Unsupported operator: {condition.Operator}");

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
