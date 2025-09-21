namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public partial record EmptyExpression
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.NoContent<object?>());

    public override Task<ExpressionParseResult> ParseAsync(CancellationToken token)
        => Task.FromResult(new ExpressionParseResultBuilder()
            .WithStatus(ResultStatus.NotSupported)
            .WithExpressionComponentType(GetType())
            .Build());
}
