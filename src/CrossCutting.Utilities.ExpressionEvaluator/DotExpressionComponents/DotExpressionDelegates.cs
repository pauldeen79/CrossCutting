namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class DotExpressionDelegates<T>
{
    public DotExpressionDelegates(DotExpressionType expressionType, Func<DotExpressionComponentState, Result<Type>> parseDelegate, Func<DotExpressionComponentState, T, Result<object?>> evaluateDelegate)
    {
        ArgumentGuard.IsNotNull(parseDelegate, nameof(parseDelegate));
        ArgumentGuard.IsNotNull(evaluateDelegate, nameof(evaluateDelegate));

        ExpressionType = expressionType;
        ParseDelegate = parseDelegate;
        EvaluateDelegate = evaluateDelegate;
    }

    public DotExpressionType ExpressionType { get; }
    public Func<DotExpressionComponentState, Result<Type>> ParseDelegate { get; }
    public Func<DotExpressionComponentState, T, Result<object?>> EvaluateDelegate { get; }
}
