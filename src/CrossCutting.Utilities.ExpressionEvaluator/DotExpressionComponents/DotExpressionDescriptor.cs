namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class DotExpressionDescriptor<T>
{
    public DotExpressionDescriptor(Dictionary<string, DotExpressionDelegates<T>> delegates)
    {
        ArgumentGuard.IsNotNull(delegates, nameof(delegates));

        Delegates = delegates;
    }

    public Type GetUnderlyingType => typeof(T);

    public Dictionary<string, DotExpressionDelegates<T>> Delegates { get; }
}
