namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionParser;

internal sealed class OtherExpr : IExpr
{
    private string Value { get; }

    public OtherExpr(string value)
    {
        Value = value;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context, Func<string, Result<object?>> @delegate) => @delegate(Value);
}
