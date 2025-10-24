namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class AsyncResultDictionaryBuilderExtensions
{
    public static IAsyncResultDictionaryBuilder Add(this IAsyncResultDictionaryBuilder instance, FunctionCallContext context, int index, string name, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(name, nameof(name));

        return instance.Add(name, context.GetArgumentValueResultAsync(index, name, token));
    }

    public static IAsyncResultDictionaryBuilder Add<T>(this IAsyncResultDictionaryBuilder instance, FunctionCallContext context, int index, string name, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(name, nameof(name));

        return instance.Add(name, context.GetArgumentValueResultAsync<T>(index, name, token));
    }

    public static IAsyncResultDictionaryBuilder<TResult> Add<TResult>(this IAsyncResultDictionaryBuilder<TResult> instance, FunctionCallContext context, int index, string name, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(name, nameof(name));

        return instance.Add(name, context.GetArgumentValueResultAsync<TResult>(index, name, token));
    }
}
