namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionDescriptorProvider
{
    IReadOnlyCollection<FunctionDescriptor> GetAll();
}
