namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMemberDescriptorProvider
{
    IReadOnlyCollection<MemberDescriptor> GetAll();
}
