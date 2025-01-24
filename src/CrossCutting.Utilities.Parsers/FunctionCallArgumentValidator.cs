namespace CrossCutting.Utilities.Parsers;

public class FunctionCallArgumentValidator : IFunctionCallArgumentValidator
{
    public Result<Type> Validate(FunctionDescriptorArgument descriptorArgument, FunctionCallArgument callArgument, FunctionCallContext functionCallContext)
    {
        descriptorArgument = descriptorArgument.IsNotNull(nameof(descriptorArgument));
        callArgument = callArgument.IsNotNull(nameof(callArgument));
        functionCallContext = functionCallContext.IsNotNull(nameof(functionCallContext));

        var callArgumentResult = callArgument.Validate(functionCallContext);
        if (!callArgumentResult.IsSuccessful())
        {
            return callArgumentResult;
        }
        else if (callArgumentResult.Value is not null && !descriptorArgument.Type.IsAssignableFrom(callArgumentResult.Value))
        {
            return Result.Invalid<Type>($"Argument {descriptorArgument.Name} is not of type {descriptorArgument.Type.FullName}");
        }
        else if (callArgumentResult.Value is null && descriptorArgument.IsRequired)
        {
            return Result.Invalid<Type>($"Argument {descriptorArgument.Name} is required");
        }

        return callArgumentResult;
    }
}
