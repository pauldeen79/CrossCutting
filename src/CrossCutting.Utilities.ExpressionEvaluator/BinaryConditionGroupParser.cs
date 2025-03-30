namespace CrossCutting.Utilities.ExpressionEvaluator;

public class BinaryConditionGroupParser : IBinaryConditionGroupParser
{
    public Result<BinaryConditionGroup> Parse(string expression)
    {
        var conditions = new List<BinaryCondition>();
        var combination = string.Empty;

        // First get array of parts
        var parts = expression.SplitDelimited(BinaryOperatorExpression.OperatorExpressions, '"', true, true, true, StringComparison.OrdinalIgnoreCase);
        if (parts.Length <= 1)
        {
            // Messed up expression! Should always be <left expression> <operator>
            return Result.Continue<BinaryConditionGroup>();
        }

        // Now, we need to convert this array of parts to conditions
        var itemCountIsCorrect = (parts.Length - 1) % 2 == 0;
        if (!itemCountIsCorrect)
        {
            return Result.Invalid<BinaryConditionGroup>("Comparison expression has invalid number of parts");
        }

        for (var i = 0; i < parts.Length; i += 2)
        {
            //-parts[i + 0] is the left expression
            //-parts[i + 1] is the combination for the next condition
            var currentExpression = parts[i];
            var startGroup = false;
            var endGroup = false;

            //remove brackets and set bracket property values for this query item.
            if (currentExpression.StartsWith("("))
            {
                currentExpression = currentExpression.Substring(1);
                startGroup = true;
            }

            if (currentExpression.EndsWith(")"))
            {
                currentExpression = currentExpression.Substring(0, currentExpression.Length - 1);
                endGroup = true;
            }

            var condition = new BinaryConditionBuilder()
                .WithCombination(combination.ToUpperInvariant() switch
                {
                    "&&" => Combination.And,
                    "||" => Combination.Or,
                    _ => default(Combination?)
                })
                .WithExpression(currentExpression)
                .WithStartGroup(startGroup)
                .WithEndGroup(endGroup)
                .Build();

            combination = parts.Length > i + 1
                ? parts[i + 1]
                : string.Empty;

            conditions.Add(condition);
        }

        return Result.Success(new BinaryConditionGroup(conditions));
    }
}
