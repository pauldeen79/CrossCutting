namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMemberDescriptorProvider
{
    Result<IReadOnlyCollection<MemberDescriptor>> GetAll();
}
