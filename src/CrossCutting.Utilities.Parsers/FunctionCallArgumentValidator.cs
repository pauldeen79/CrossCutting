namespace CrossCutting.Utilities.Parsers;

public class FunctionCallArgumentValidator : IFunctionCallArgumentValidator
{
    public IEnumerable<Result> Validate(FunctionDescriptorArgument descriptorArgument, FunctionCallArgument callArgument, FunctionCallContext functionCallContext)
    {
        descriptorArgument = descriptorArgument.IsNotNull(nameof(descriptorArgument));
        callArgument = callArgument.IsNotNull(nameof(callArgument));
        functionCallContext = functionCallContext.IsNotNull(nameof(functionCallContext));

        var callArgumentResult = callArgument.GetValueResult(functionCallContext);
        if (!callArgumentResult.IsSuccessful())
        {
            yield return callArgumentResult;
        }
        else if (callArgumentResult.Value is not null && !descriptorArgument.Type.IsInstanceOfType(callArgumentResult.Value))
        {
            yield return Result.Invalid($"Argument {descriptorArgument.Name} is not of type {descriptorArgument.Type.FullName}");
        }
        else if (callArgumentResult.Value is null && descriptorArgument.IsRequired)
        {
            yield return Result.Invalid($"Argument {descriptorArgument.Name} is required");
        }
    }
}
