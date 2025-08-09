namespace CrossCutting.Utilities.ExpressionEvaluator.BinaryExpressionComponents;

public class AndBinaryExpressionComponent : IBinaryExpressionComponent
{
    public bool HasBooleanResult => true;

    public Result<object?> Process(ExpressionTokenType type, ExpressionEvaluatorContext context, IReadOnlyDictionary<string, Result> results)
    {
        results = ArgumentGuard.IsNotNull(results, nameof(results));

        return Supports(type)
            ? EvaluateAnd(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression))
            : Result.Continue<object?>();
    }

    public bool Supports(ExpressionTokenType type)
        => type == ExpressionTokenType.And;

    private static Result<object?> EvaluateAnd(object? left, object? right)
        => Result.Success<object?>(left.IsTruthy() && right.IsTruthy());
}
