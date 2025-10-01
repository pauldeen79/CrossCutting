namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

internal abstract class ExpressionBase : IExpression
{
    public abstract Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);
    public abstract Task<ExpressionParseResult> ParseAsync(CancellationToken token);
    
    public IEvaluatableBuilder ToBuilder() => throw new NotSupportedException();
}
