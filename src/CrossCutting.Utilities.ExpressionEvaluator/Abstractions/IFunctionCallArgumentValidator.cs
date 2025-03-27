namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IFunctionCallArgumentValidator
{
    Result<Type> Validate(FunctionDescriptorArgument descriptorArgument, string callArgument, FunctionCallContext functionCallContext);
}
