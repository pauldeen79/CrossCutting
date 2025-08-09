namespace CrossCutting.Utilities.ExpressionEvaluator.BinaryExpressionComponents;

public class LessOrEqualBinaryExpressionComponent : IBinaryExpressionComponent
{
    public bool HasBooleanResult => true;

    public Result<object?> Process(ExpressionTokenType type, ExpressionEvaluatorContext context, IReadOnlyDictionary<string, Result> results)
    {
        results = ArgumentGuard.IsNotNull(results, nameof(results));

        return Supports(type)
            ? SmallerOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression))
            : Result.Continue<object?>();
    }

    public bool Supports(ExpressionTokenType type)
        => type == ExpressionTokenType.LessOrEqual;
}
