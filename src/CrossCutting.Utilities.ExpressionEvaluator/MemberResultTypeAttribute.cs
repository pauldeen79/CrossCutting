namespace CrossCutting.Utilities.ExpressionEvaluator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class MemberResultTypeAttribute : Attribute
{
    public Type Type { get; }

    public MemberResultTypeAttribute(Type type)
    {
        ArgumentGuard.IsNotNull(type, nameof(type));

        Type = type;
    }
}
