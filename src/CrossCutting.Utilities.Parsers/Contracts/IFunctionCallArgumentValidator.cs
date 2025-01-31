namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionCallArgumentValidator
{
    Result<Type> Validate(FunctionDescriptorArgument descriptorArgument, IFunctionCallArgument callArgument, FunctionCallContext functionCallContext);
}
