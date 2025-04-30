namespace CrossCutting.Utilities.ExpressionEvaluator;

public sealed class MemberArgumentInfo
{
    public MemberArgumentInfo(MemberDescriptorArgument descriptorArgument, string callArgument)
    {
        ArgumentGuard.IsNotNull(descriptorArgument, nameof(descriptorArgument));
        ArgumentGuard.IsNotNull(callArgument, nameof(callArgument));

        DescriptorArgument = descriptorArgument;
        CallArgument = callArgument;
    }

    public MemberDescriptorArgument DescriptorArgument { get; }
    public string CallArgument { get; }
}
