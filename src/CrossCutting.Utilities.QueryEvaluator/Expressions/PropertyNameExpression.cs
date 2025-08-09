namespace CrossCutting.Utilities.QueryEvaluator.Core.Expressions;

public partial record PropertyNameExpression
{
    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var contextValueResult = (await context.State
            .GetValueAsync(Constants.Context)
            .ConfigureAwait(false)).EnsureValue();

        if (!contextValueResult.IsSuccessful())
        {
            return contextValueResult;
        }

        var property = contextValueResult.Value!.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.Public);
        if (property is null)
        {
            return Result.Invalid<object?>($"Type {contextValueResult.Value.GetType().FullName} does not contain property {PropertyName}");
        }

        return Result.WrapException(() => Result.Success<object?>(property.GetValue(contextValueResult.Value)));
    }
}
