namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionDescriptorProvider
{
    IEnumerable<FunctionDescriptor> CreateFunction(IFunction source, Type? customFunctionType);
    IReadOnlyCollection<FunctionDescriptor> GetAll();
}
