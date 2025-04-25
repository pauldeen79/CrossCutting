namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class ResultDictionaryBuilderExtensions
{
    public static ResultDictionaryBuilder Add(this ResultDictionaryBuilder instance, FunctionCallContext context, int index, string name)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(name, nameof(name));

        return instance.Add(name, _ => context.GetArgumentValueResult(index, name));
    }

    public static ResultDictionaryBuilder Add<T>(this ResultDictionaryBuilder instance, FunctionCallContext context, int index, string name)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(name, nameof(name));

        return instance.Add(name, _ => context.GetArgumentValueResult<T>(index, name));
    }

    public static ResultDictionaryBuilder<TResult> Add<TResult>(this ResultDictionaryBuilder<TResult> instance, FunctionCallContext context, int index, string name)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(name, nameof(name));

        return instance.Add(name, _ => context.GetArgumentValueResult<TResult>(index, name));
    }
}
