namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public sealed class EvaluatableExpression : IExpression
{
    public string SourceExpression { get; }

    public EvaluatableExpression(string expression)
    {
        ArgumentGuard.IsNotNull(expression, nameof(expression));

        SourceExpression = expression;
    }

    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.EvaluateAsync(SourceExpression, token);
    }

    public Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.ParseAsync(SourceExpression, token);
    }

    public IEvaluatableBuilder ToBuilder() => new EvaluatableExpressionBuilder(SourceExpression);
}
