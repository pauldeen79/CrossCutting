namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public partial record LiteralExpression
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.Success(Value));

    public override Task<ExpressionParseResult> ParseAsync(CancellationToken token)
        => Task.FromResult(new ExpressionParseResultBuilder()
            .WithStatus(ResultStatus.NotSupported)
            .WithExpressionComponentType(GetType())
            .Build());
}
