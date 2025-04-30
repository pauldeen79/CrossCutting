namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionArgumentInfo
{
    public FunctionArgumentInfo(FunctionDescriptorArgument descriptorArgument, string callArgument)
    {
        ArgumentGuard.IsNotNull(descriptorArgument, nameof(descriptorArgument));
        ArgumentGuard.IsNotNull(callArgument, nameof(callArgument));

        DescriptorArgument = descriptorArgument;
        CallArgument = callArgument;
    }

    public FunctionDescriptorArgument DescriptorArgument { get; }
    public string CallArgument { get; }
}
