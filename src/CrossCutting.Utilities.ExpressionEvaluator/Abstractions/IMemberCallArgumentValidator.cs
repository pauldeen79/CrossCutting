namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMemberCallArgumentValidator
{
    Task<ExpressionParseResult> ValidateAsync(MemberDescriptorArgument descriptorArgument, string callArgument, FunctionCallContext context, CancellationToken token);
    Task<Result<MemberAndTypeDescriptor>> ValidateAsync(MemberDescriptor functionDescriptor, IMember member, FunctionCallContext context, CancellationToken token);
}
