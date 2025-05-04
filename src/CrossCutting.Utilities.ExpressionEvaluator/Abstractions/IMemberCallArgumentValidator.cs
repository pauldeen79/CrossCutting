namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMemberCallArgumentValidator
{
    ExpressionParseResult Validate(MemberDescriptorArgument descriptorArgument, string callArgument, FunctionCallContext context);
    Result<MemberAndTypeDescriptor> Validate(FunctionCallContext context, MemberDescriptor functionDescriptor, IMember member);
}
