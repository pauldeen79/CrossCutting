namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionCallArgumentValidator : IFunctionCallArgumentValidator
{
    public ExpressionParseResult Validate(FunctionDescriptorArgument descriptorArgument, string callArgument, FunctionCallContext context)
    {
        descriptorArgument = ArgumentGuard.IsNotNull(descriptorArgument, nameof(descriptorArgument));
        callArgument = ArgumentGuard.IsNotNull(callArgument, nameof(callArgument));
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var callArgumentResult = context.Context.Parse(callArgument);
        if (!callArgumentResult.IsSuccessful())
        {
            return callArgumentResult;
        }
        else if (!IsTypeValid(context.Context.Settings, descriptorArgument, callArgumentResult))
        {
            return new ExpressionParseResultBuilder()
                .WithSourceExpression(context.Context.Expression)
                .WithStatus(ResultStatus.Invalid)
                .WithErrorMessage($"Argument {descriptorArgument.Name} is not of type {descriptorArgument.Type.FullName}")
                .WithExpressionComponentType(callArgumentResult.ExpressionComponentType);
        }

        return callArgumentResult;
    }

    private static bool IsTypeValid(ExpressionEvaluatorSettings settings, FunctionDescriptorArgument descriptorArgument, ExpressionParseResult callArgumentResult)
    {
        if (!settings.ValidateArgumentTypes)
        {
            // Type checking is disabled
            return true;
        }

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

        if (callArgumentResult.ResultType == typeof(object) && !settings.StrictTypeChecking)
        {
            // Result type is object, and strict type checking is disabled.
            // So we don't know if the type is assignable, and will defer this to run-time checking.
            return true;
        }

        // For now, we assume that the type is assignable.
        // Note that this doesn't take automatic type conversion into account (implicit operators), and we don't support type conversion
        // Note that if we want either of those, this should not be built into function calls, but at a lower level, in the ExpressionEvaluator itself. (accessible through the context)
        return descriptorArgument.Type.IsAssignableFrom(callArgumentResult.ResultType);
    }
}
