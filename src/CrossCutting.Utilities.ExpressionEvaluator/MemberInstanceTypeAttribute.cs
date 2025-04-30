namespace CrossCutting.Utilities.ExpressionEvaluator;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class MemberInstanceTypeAttribute : Attribute
{
    public Type Type { get; }

    public MemberInstanceTypeAttribute(Type type)
    {
        ArgumentGuard.IsNotNull(type, nameof(type));

        Type = type;
    }
}
