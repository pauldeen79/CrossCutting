namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMemberCallArgumentValidator
{
    ExpressionParseResult Validate(MemberDescriptorArgument descriptorArgument, string callArgument, FunctionCallContext context);
    Result<MemberAndTypeDescriptor> Validate(MemberDescriptor functionDescriptor, IMember member, FunctionCallContext context);
}
