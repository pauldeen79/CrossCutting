namespace CrossCutting.Utilities.ExpressionEvaluator;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class FunctionResultTypeAttribute : Attribute
{
    public Type Type { get; }

    public FunctionResultTypeAttribute(Type type)
    {
        ArgumentGuard.IsNotNull(type, nameof(type));

        Type = type;
    }
}
