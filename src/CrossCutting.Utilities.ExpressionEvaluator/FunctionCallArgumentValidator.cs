namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionCallArgumentValidator : IFunctionCallArgumentValidator
{
    public ExpressionParseResult Validate(FunctionDescriptorArgument descriptorArgument, string callArgument, FunctionCallContext context)
    {
        descriptorArgument = descriptorArgument.IsNotNull(nameof(descriptorArgument));
        callArgument = callArgument.IsNotNull(nameof(callArgument));
        context = context.IsNotNull(nameof(context));

        var callArgumentResult = context.Context.Parse(callArgument);
        if (!callArgumentResult.Status.IsSuccessful())
        {
            return callArgumentResult;
        }
        else if (context.Context.Settings.ValidateArgumentTypes && !IsTypeValid(descriptorArgument, callArgumentResult))
        {
            return new ExpressionParseResultBuilder()
                .WithSourceExpression(context.Context.Expression)
                .WithStatus(ResultStatus.Invalid)
                .WithErrorMessage($"Argument {descriptorArgument.Name} is not of type {descriptorArgument.Type.FullName}")
                .WithExpressionComponentType(callArgumentResult.ExpressionComponentType);
        }

        return callArgumentResult;
    }

    private static bool IsTypeValid(FunctionDescriptorArgument descriptorArgument, ExpressionParseResult callArgumentResult)
    {
        if (descriptorArgument.Type == typeof(object))
        {
            // Function accepts object type, so will take care of conversion itself
            return true;
        }

        if (callArgumentResult.ResultType is null)
        {
            // Result type is unknown, so we cannot determine whether the type will be compatible
            return true;
        }

        // For now, we assume that the type is assignable.
        // Note that this doesn't take automatic type conversion into account, using implicit operators
        return descriptorArgument.Type.IsAssignableFrom(callArgumentResult.ResultType);
    }
}
