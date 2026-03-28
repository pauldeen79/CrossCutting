namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.EvaluatableSqlExpressionProviderHandlers;

public static class BinaryOperatorEvaluatableHandlerUtil
{
    public static Result<string> GetSqlExpression(IReadOnlyDictionary<string, Result<string>> results, string combinator)
        => results.OnSuccess(_ =>
        {
            var leftOperand = results.GetValue(nameof(BinaryAndOperatorEvaluatable.LeftOperand));
            var rightOperand = results.GetValue(nameof(BinaryAndOperatorEvaluatable.RightOperand));

            if (string.IsNullOrEmpty(leftOperand) && string.IsNullOrEmpty(rightOperand))
            {
                return Result.Success(string.Empty);
            }

            if (string.IsNullOrEmpty(leftOperand))
            {
                return Result.Success(rightOperand);
            }

            if (string.IsNullOrEmpty(rightOperand))
            {
                return Result.Success(leftOperand);
            }

            return Result.Success($"({leftOperand} {combinator} {rightOperand})");
        });
}