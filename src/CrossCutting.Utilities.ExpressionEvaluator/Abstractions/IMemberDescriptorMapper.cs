namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMemberDescriptorMapper
{
    Result<IReadOnlyCollection<MemberDescriptor>> Map(IMember source, Type? customImplementationType);
}
