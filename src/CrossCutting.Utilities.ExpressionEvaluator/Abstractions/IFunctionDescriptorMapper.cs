namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IFunctionDescriptorMapper
{
    IEnumerable<FunctionDescriptor> Map(object source, Type? customFunctionType);
}
