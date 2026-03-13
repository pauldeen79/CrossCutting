namespace CrossCutting.Utilities.ExpressionEvaluator;

public static class Evaluatable
{
    public static PropertyNameEvaluatable OfPropertyName(string propertyName)
        => new PropertyNameEvaluatable(propertyName);

    public static PropertyNameEvaluatable OfProperty(string propertyName, IEvaluatable operand)
        => new PropertyNameEvaluatable(propertyName, operand);

    public static LiteralEvaluatable<T> OfValue<T>(T value)
        => new LiteralEvaluatable<T>(value);

    public static DelegateEvaluatable<T> OfDelegate<T>(Func<T> @delegate)
        => new DelegateEvaluatable<T>(@delegate);

    public static ContextEvaluatable OfContext()
        => new ContextEvaluatable();    
}