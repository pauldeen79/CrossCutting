namespace CrossCutting.Utilities.ExpressionEvaluator.BinaryExpressionComponents;

public class DivideBinaryExpressionComponent : IBinaryExpressionComponent
{
    public bool HasBooleanResult => false;

    public Result<object?> Process(ExpressionTokenType type, ExpressionEvaluatorContext context, IReadOnlyDictionary<string, Result> results)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        results = ArgumentGuard.IsNotNull(results, nameof(results));

        return Supports(type)
            ? Divide.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider)
            : Result.Continue<object?>();
    }

    public bool Supports(ExpressionTokenType type)
        => type == ExpressionTokenType.Divide;
}
