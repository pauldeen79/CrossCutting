namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IDynamicDescriptorsFunction : IFunction
{
    IEnumerable<FunctionDescriptor> GetDescriptors();
}
