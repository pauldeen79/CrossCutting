namespace CrossCutting.Utilities.QueryEvaluator.Core.Expressions;

public partial record DelegateExpression
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.Success(Value()));
}
