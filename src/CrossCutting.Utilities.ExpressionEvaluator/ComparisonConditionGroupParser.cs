namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ComparisonConditionGroupParser : IComparisonConditionGroupParser
{
    private readonly string[] _delimiters;
    private readonly IOperator[] _operators;

    public ComparisonConditionGroupParser(IEnumerable<IOperator> operators)
    {
        operators = ArgumentGuard.IsNotNull(operators, nameof(operators));

        _operators = operators.OrderBy(x => x.Order).ToArray();
        _delimiters = _operators.Select(x => x.OperatorExpression).Concat(["&&", "||"]).ToArray();
    }

    public Result<ComparisonConditionGroup> Parse(string expression)
    {
        var conditions = new List<ComparisonCondition>();
        var combination = string.Empty;

        // First get array of parts
        var parts = expression.SplitDelimited(_delimiters, '"', true, true, true, StringComparison.OrdinalIgnoreCase);
        if (parts.Length <= 2)
        {
            // Messed up expression! Should always be <left expression> <operator> <right expression>
            return Result.Continue<ComparisonConditionGroup>();
        }

        // Now, we need to convert this array of parts to conditions
        var itemCountIsCorrect = (parts.Length - 3) % 4 == 0;
        if (!itemCountIsCorrect)
        {
            return Result.Invalid<ComparisonConditionGroup>("Comparison expression has invalid number of parts");
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
                return Result.Invalid<ComparisonConditionGroup>("Comparison expression is malformed");
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

        return Result.Success(new ComparisonConditionGroup(conditions));
    }

    private IOperator GetOperator(string @operator)
        => _operators.FirstOrDefault(x => x.OperatorExpression.Equals(@operator.Trim(), StringComparison.OrdinalIgnoreCase));
}
