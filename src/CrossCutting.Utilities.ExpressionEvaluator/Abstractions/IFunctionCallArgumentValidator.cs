namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IFunctionCallArgumentValidator
{
    ExpressionParseResult Validate(FunctionDescriptorArgument descriptorArgument, string callArgument, FunctionCallContext context);
}
