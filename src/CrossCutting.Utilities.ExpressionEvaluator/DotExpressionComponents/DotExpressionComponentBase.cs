namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public abstract class DotExpressionComponentBase<T> : IDotExpressionComponent
{
    private readonly DotExpressionDescriptor<T> _descriptor;

    protected DotExpressionComponentBase(DotExpressionDescriptor<T> descriptor)
    {
        ArgumentGuard.IsNotNull(descriptor, nameof(descriptor));

        _descriptor = descriptor;
    }

    public abstract int Order { get; }

    public Result<object?> Evaluate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.CurrentEvaluateResult.Value is T typedValue && _descriptor.Delegates.TryGetValue(state.Name, out var delegates) && state.Type == delegates.ExpressionType && ArgumentsValid(delegates, state.FunctionParseResult))
        {
            return delegates.EvaluateDelegate(state, typedValue);
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (typeof(T).IsAssignableFrom(state.ResultType) && _descriptor.Delegates.TryGetValue(state.Name, out var delegates) && state.Type == delegates.ExpressionType && ArgumentsValid(delegates, state.FunctionParseResult))
        {
            return delegates.ParseDelegate(state);
        }

        return Result.Continue<Type>();
    }

    private static bool ArgumentsValid(DotExpressionDelegates<T> delegates, Result<FunctionCall> functionParseResult)
        => delegates.ExpressionType != DotExpressionType.Method
        || functionParseResult.Value!.Arguments.Count >= delegates.ArgumentCount;
}
