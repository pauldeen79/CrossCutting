namespace CrossCutting.Utilities.ExpressionEvaluator;

public sealed class MemberAndTypeDescriptor
{
    public MemberAndTypeDescriptor(IMember member, Type? returnValueType)
    {
        ArgumentGuard.IsNotNull(member, nameof(member));

        ReturnValueType = returnValueType;
        Member = member;
    }

    public Type? ReturnValueType { get; }
    public IMember Member { get; }
}
