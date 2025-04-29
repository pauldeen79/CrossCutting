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

        if (state.Type == DotExpressionType.Property && state.CurrentEvaluateResult.Value is T typedValue && _descriptor.Delegates.TryGetValue(state.Part, out var delegates))
        {
            return delegates.EvaluateDelegate(state, typedValue);
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Type == DotExpressionType.Property && state.ResultType == typeof(T) && _descriptor.Delegates.TryGetValue(state.Part, out var delegates))
        {
            return delegates.ParseDelegate(state);
        }

        return Result.Continue<Type>();
    }
}
