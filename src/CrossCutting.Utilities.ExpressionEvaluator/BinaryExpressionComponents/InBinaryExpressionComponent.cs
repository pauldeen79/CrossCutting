namespace CrossCutting.Utilities.ExpressionEvaluator.BinaryExpressionComponents;

public class InBinaryExpressionComponent : IBinaryExpressionComponent
{
    public bool HasBooleanResult => true;

    public Result<object?> Process(ExpressionTokenType type, ExpressionEvaluatorContext context, IReadOnlyDictionary<string, Result> results)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        results = ArgumentGuard.IsNotNull(results, nameof(results));

        return Supports(type)
            ? In.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.StringComparison)
            : Result.Continue<object?>();
    }

    public bool Supports(ExpressionTokenType type)
        => type == ExpressionTokenType.In;
}
