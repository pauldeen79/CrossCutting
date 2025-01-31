namespace CrossCutting.Utilities.Parsers;

public class FunctionCallArgumentValidator : IFunctionCallArgumentValidator
{
    public Result<Type> Validate(FunctionDescriptorArgument descriptorArgument, IFunctionCallArgument callArgument, FunctionCallContext functionCallContext)
    {
        descriptorArgument = descriptorArgument.IsNotNull(nameof(descriptorArgument));
        callArgument = callArgument.IsNotNull(nameof(callArgument));
        functionCallContext = functionCallContext.IsNotNull(nameof(functionCallContext));

        var callArgumentResult = callArgument.Validate(functionCallContext);
        if (!callArgumentResult.IsSuccessful())
        {
            return callArgumentResult;
        }
        else if (functionCallContext.Settings.ValidateArgumentTypes && !IsTypeValid(descriptorArgument, callArgumentResult))
        {
            return Result.Invalid<Type>($"Argument {descriptorArgument.Name} is not of type {descriptorArgument.Type.FullName}");
        }
        else if (callArgumentResult.Value is null && descriptorArgument.IsRequired)
        {
            return Result.Invalid<Type>($"Argument {descriptorArgument.Name} is required");
        }

        return callArgumentResult;
    }

    private static bool IsTypeValid(FunctionDescriptorArgument descriptorArgument, Result<Type> callArgumentResult)
    {
        if (descriptorArgument.Type == typeof(object))
        {
            return true;
        }

        if (callArgumentResult.Value is null)
        {
            return true;
        }

        return descriptorArgument.Type.IsAssignableFrom(callArgumentResult.Value);
    }
}
