namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionDescriptorMapper
{
    IEnumerable<FunctionDescriptor> Map(object source, Type? customFunctionType);
}
