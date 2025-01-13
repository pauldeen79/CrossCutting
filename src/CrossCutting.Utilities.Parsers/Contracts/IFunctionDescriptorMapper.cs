namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionDescriptorMapper
{
    IEnumerable<FunctionDescriptor> Map(IFunction source, Type? customFunctionType);
}
