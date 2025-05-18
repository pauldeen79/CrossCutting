namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class AsyncResultDictionaryBuilderExtensions
{
    public static AsyncResultDictionaryBuilder Add(this AsyncResultDictionaryBuilder instance, FunctionCallContext context, int index, string name, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(name, nameof(name));

        return instance.Add(name, context.GetArgumentValueResultAsync(index, name, token));
    }

    public static AsyncResultDictionaryBuilder Add<T>(this AsyncResultDictionaryBuilder instance, FunctionCallContext context, int index, string name, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(name, nameof(name));

        return instance.Add(name, context.GetArgumentValueResultAsync<T>(index, name, token));
    }

    public static AsyncResultDictionaryBuilder<TResult> Add<TResult>(this AsyncResultDictionaryBuilder<TResult> instance, FunctionCallContext context, int index, string name, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(name, nameof(name));

        return instance.Add(name, context.GetArgumentValueResultAsync<TResult>(index, name, token));
    }
}
