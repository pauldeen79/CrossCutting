namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class DotExpressionDelegates<T>
{
    public DotExpressionDelegates(Func<DotExpressionComponentState, Result<Type>> parseDelegate, Func<DotExpressionComponentState, T, Result<object?>> evaluateDelegate)
        : this(DotExpressionType.Property, 0, parseDelegate, evaluateDelegate)
    {
    }

    public DotExpressionDelegates(int argumentCount, Func<DotExpressionComponentState, Result<Type>> parseDelegate, Func<DotExpressionComponentState, T, Result<object?>> evaluateDelegate)
        : this(DotExpressionType.Method, argumentCount, parseDelegate, evaluateDelegate)
    {
    }

    private DotExpressionDelegates(DotExpressionType expressionType, int argumentCount, Func<DotExpressionComponentState, Result<Type>> parseDelegate, Func<DotExpressionComponentState, T, Result<object?>> evaluateDelegate)
    {
        ArgumentGuard.IsNotNull(parseDelegate, nameof(parseDelegate));
        ArgumentGuard.IsNotNull(evaluateDelegate, nameof(evaluateDelegate));

        ExpressionType = expressionType;
        ArgumentCount = argumentCount;
        ParseDelegate = parseDelegate;
        EvaluateDelegate = evaluateDelegate;
    }

    public DotExpressionType ExpressionType { get; }
    public int ArgumentCount { get; }
    public Func<DotExpressionComponentState, Result<Type>> ParseDelegate { get; }
    public Func<DotExpressionComponentState, T, Result<object?>> EvaluateDelegate { get; }
}
