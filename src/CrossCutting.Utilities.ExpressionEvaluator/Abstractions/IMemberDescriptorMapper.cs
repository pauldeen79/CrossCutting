namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMemberDescriptorMapper
{
    IEnumerable<FunctionDescriptor> Map(object source, Type? customFunctionType);
}
