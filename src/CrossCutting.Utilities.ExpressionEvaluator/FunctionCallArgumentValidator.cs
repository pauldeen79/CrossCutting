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
        else if (!IsTypeValid(descriptorArgument, callArgumentResult))
        {
            return new ExpressionParseResultBuilder()
                .WithSourceExpression(context.Context.Expression)
                .WithStatus(ResultStatus.Invalid)
                .WithErrorMessage($"Argument {descriptorArgument.Name} is not of type {descriptorArgument.Type.FullName}")
                .WithExpressionType(typeof(FunctionExpression));
        }

        return callArgumentResult;
    }

    private static bool IsTypeValid(FunctionDescriptorArgument descriptorArgument, ExpressionParseResult callArgumentResult)
    {
        if (descriptorArgument.Type == typeof(object))
        {
            return true;
        }

        if (callArgumentResult.ResultType is null)
        {
            return true;
        }

        return descriptorArgument.Type.IsAssignableFrom(callArgumentResult.ResultType);
    }
}
