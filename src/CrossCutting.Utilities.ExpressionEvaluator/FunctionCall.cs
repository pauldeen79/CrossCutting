namespace CrossCutting.Utilities.ExpressionEvaluator;

public partial record FunctionCall
{
    public async Task<Result<object?>> GetArgumentValueResultAsync(int index, string argumentName, FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return index + 1 > Arguments.Count
            ? Result.Invalid<object?>($"Missing argument: {argumentName}")
            : await context.Context.EvaluateAsync(Arguments.ElementAt(index), token).ConfigureAwait(false);
    }

    public async Task<Result<object?>> GetArgumentValueResultAsync(int index, string argumentName, FunctionCallContext context, object? defaultValue, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return index + 1 > Arguments.Count
            ? Result.Success(defaultValue)
            : await context.Context.EvaluateAsync(Arguments.ElementAt(index), token).ConfigureAwait(false);
    }

    public async Task<Result<T>> GetArgumentValueResultAsync<T>(int index, string argumentName, FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return index + 1 > Arguments.Count
            ? Result.Invalid<T>($"Missing argument: {argumentName}")
            : (await context.Context.EvaluateAsync(Arguments.ElementAt(index), token).ConfigureAwait(false)).TryCast<T>();
    }

    public async Task<Result<T?>> GetArgumentValueResultAsync<T>(int index, string argumentName, FunctionCallContext context, T? defaultValue, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return index + 1 > Arguments.Count
            ? Result.Success(defaultValue)
            : (await context.Context.EvaluateAsync(Arguments.ElementAt(index), token).ConfigureAwait(false))
                .TryCastAllowNull<T>()
                .Transform(value => value is null ? defaultValue : value);
    }
}
