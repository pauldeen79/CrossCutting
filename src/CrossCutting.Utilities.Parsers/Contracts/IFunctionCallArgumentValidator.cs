namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionCallArgumentValidator
{
    Result Validate(FunctionDescriptorArgument descriptorArgument, FunctionCallArgument callArgument, FunctionCallContext functionCallContext);
}
