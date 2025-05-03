namespace CrossCutting.Utilities.ExpressionEvaluator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class MemberTypeAttribute : Attribute
{
    public MemberType MemberType { get; }

    public MemberTypeAttribute(MemberType memberType)
    {
        ArgumentGuard.IsNotNull(memberType, nameof(memberType));

        MemberType = memberType;
    }
}
