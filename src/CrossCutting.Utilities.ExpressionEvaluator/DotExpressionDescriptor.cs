namespace CrossCutting.Utilities.ExpressionEvaluator;

public sealed class DotExpressionDescriptor<T>
{
    public DotExpressionDescriptor(Dictionary<string, DotExpressionDelegates<T>> delegates)
    {
        ArgumentGuard.IsNotNull(delegates, nameof(delegates));

        Delegates = delegates;
    }

    public Dictionary<string, DotExpressionDelegates<T>> Delegates { get; }
}
