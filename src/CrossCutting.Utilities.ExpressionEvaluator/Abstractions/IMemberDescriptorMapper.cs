namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMemberDescriptorMapper
{
    IEnumerable<MemberDescriptor> Map(object source, Type? customImplementationType);
    Result<MemberDescriptor> Map(Delegate @delegate);
}
