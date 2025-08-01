namespace CrossCutting.Utilities.ExpressionEvaluator.BinaryExpressionComponents;

public class OrBinaryExpressionComponent : IBinaryExpressionComponent
{
    public bool HasBooleanResult => true;

    public Result<object?> Process(ExpressionTokenType type, ExpressionEvaluatorContext context, IReadOnlyDictionary<string, Result> results)
    {
        results = ArgumentGuard.IsNotNull(results, nameof(results));

        return Supports(type)
            ? EvaluateOr(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression))
            : Result.Continue<object?>();
    }

    public bool Supports(ExpressionTokenType type)
        => type == ExpressionTokenType.Or;

    private static Result<object?> EvaluateOr(object? left, object? right)
        => Result.Success<object?>(left.IsTruthy() || right.IsTruthy());
}
