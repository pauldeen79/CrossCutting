namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMemberDescriptorMapper
{
    Result<IReadOnlyCollection<MemberDescriptor>> Map(object source, Type? customImplementationType);
}

public interface IMemberDescriptorCallback
{
    Result<MemberDescriptor> Map(Delegate @delegate);
}
