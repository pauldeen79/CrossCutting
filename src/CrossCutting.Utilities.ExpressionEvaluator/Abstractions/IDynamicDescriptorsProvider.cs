namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IDynamicDescriptorsProvider
{
    Result<IReadOnlyCollection<MemberDescriptor>> GetDescriptors(IMemberDescriptorCallback callback);
}
