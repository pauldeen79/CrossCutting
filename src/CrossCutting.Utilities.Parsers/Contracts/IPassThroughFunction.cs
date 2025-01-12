namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IPassThroughFunction : IFunction
{
    IEnumerable<FunctionDescriptor> GetDescriptors();
}
