namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

internal sealed class OtherExpression : IExpression
{
    public string SourceExpression { get; }

    public OtherExpression(string expression)
    {
        ArgumentGuard.IsNotNull(expression, nameof(expression));

        SourceExpression = expression;
    }

    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => context.EvaluateAsync(SourceExpression, token);

    public Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => context.ParseAsync(SourceExpression, token);

    public IEvaluatableBuilder ToBuilder() => new OtherExpressionBuilder(SourceExpression);
}

internal class OtherExpressionBuilder : IEvaluatableBuilder
{
    public OtherExpressionBuilder(string sourceExpression)
    {
        SourceExpression = sourceExpression;
    }

    public string SourceExpression { get; set; }

    public IEvaluatable Build()
        => new OtherExpression(SourceExpression);
}