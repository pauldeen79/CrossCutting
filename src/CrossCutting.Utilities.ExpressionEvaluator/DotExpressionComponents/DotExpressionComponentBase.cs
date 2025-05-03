namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public abstract class DotExpressionComponentBase<T> : IDotExpressionComponent
{
    protected DotExpressionDescriptor<T> Descriptor { get; set; } = default!;

    public abstract int Order { get; }

    public Result<object?> Evaluate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.CurrentEvaluateResult.Value is T typedValue && Descriptor.Delegates.TryGetValue(state.Name, out var delegates) && state.Type == delegates.ExpressionType && ArgumentsValid(delegates, state.FunctionParseResult))
        {
            return delegates.EvaluateDelegate(state, typedValue);
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (typeof(T).IsAssignableFrom(state.ResultType) && Descriptor.Delegates.TryGetValue(state.Name, out var delegates) && state.Type == delegates.ExpressionType && ArgumentsValid(delegates, state.FunctionParseResult))
        {
            return delegates.ParseDelegate(state);
        }

        return Result.Continue<Type>();
    }

    private static bool ArgumentsValid(DotExpressionDelegates<T> delegates, Result<FunctionCall> functionParseResult)
        => delegates.ExpressionType != DotExpressionType.Method
        || functionParseResult.Value!.Arguments.Count >= delegates.ArgumentCount;
}
