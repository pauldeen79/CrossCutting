namespace CrossCutting.Utilities.QueryEvaluator.Core.Expressions;

public partial record EmptyExpression
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.NoContent<object?>());
}
