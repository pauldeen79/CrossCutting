namespace CrossCutting.Utilities.QueryEvaluator.Core.Expressions;

public partial record LiteralExpression
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.Success(Value));
}
