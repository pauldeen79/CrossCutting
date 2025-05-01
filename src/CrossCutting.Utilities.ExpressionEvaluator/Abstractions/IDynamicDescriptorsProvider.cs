namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IDynamicDescriptorsProvider
{
    IEnumerable<MemberDescriptor> GetDescriptors(IMemberDescriptorCallback callback);
}
