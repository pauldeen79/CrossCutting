namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class AsyncResultDictionaryBuilderExtensions
{
    public static AsyncResultDictionaryBuilder Add(this AsyncResultDictionaryBuilder instance, FunctionCallContext context, int index, string name)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(name, nameof(name));

        return instance.Add(name, context.GetArgumentValueResultAsync(index, name));
    }

    public static AsyncResultDictionaryBuilder Add<T>(this AsyncResultDictionaryBuilder instance, FunctionCallContext context, int index, string name)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(name, nameof(name));

        return instance.Add(name, context.GetArgumentValueResultAsync<T>(index, name));
    }

    public static AsyncResultDictionaryBuilder<TResult> Add<TResult>(this AsyncResultDictionaryBuilder<TResult> instance, FunctionCallContext context, int index, string name)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(name, nameof(name));

        return instance.Add(name, context.GetArgumentValueResultAsync<TResult>(index, name));
    }
}
