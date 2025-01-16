namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionCallArgumentValidator
{
    IEnumerable<Result> Validate(FunctionDescriptorArgument descriptorArgument, FunctionCallArgument callArgument, FunctionCallContext functionCallContext);
}
