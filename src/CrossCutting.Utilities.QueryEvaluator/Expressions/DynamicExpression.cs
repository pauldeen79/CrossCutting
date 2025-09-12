namespace CrossCutting.Utilities.QueryEvaluator.Core.Expressions;

public partial record DynamicExpression
{
    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await Expression.EvaluateAsync(context, token)
            .ConfigureAwait(false))
            .EnsureNotNull("Expression evaluation resulted in null");
    }
}
