namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMemberDescriptorMapper
{
    Result<IReadOnlyCollection<MemberDescriptor>> Map(object source, Type? customImplementationType);
}
