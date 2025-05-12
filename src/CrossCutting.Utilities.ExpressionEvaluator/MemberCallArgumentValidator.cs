namespace CrossCutting.Utilities.ExpressionEvaluator;

public class MemberCallArgumentValidator : IMemberCallArgumentValidator
{
    public async Task<ExpressionParseResult> ValidateAsync(MemberDescriptorArgument descriptorArgument, string callArgument, FunctionCallContext context)
    {
        descriptorArgument = ArgumentGuard.IsNotNull(descriptorArgument, nameof(descriptorArgument));
        callArgument = ArgumentGuard.IsNotNull(callArgument, nameof(callArgument));
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var callArgumentResult = await context.Context.ParseAsync(callArgument).ConfigureAwait(false);
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

    public async Task<Result<MemberAndTypeDescriptor>> ValidateAsync(MemberDescriptor functionDescriptor, IMember member, FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        functionDescriptor = ArgumentGuard.IsNotNull(functionDescriptor, nameof(functionDescriptor));

        var arguments = functionDescriptor.Arguments.Zip(context.FunctionCall.Arguments, (descriptor, call) => new MemberArgumentInfo(descriptor, call));

        var errors = new List<ValidationError>();
        foreach (var argument in arguments)
        {
            var validationResult = await ValidateAsync(argument.DescriptorArgument, argument.CallArgument, context).ConfigureAwait(false);
            if (!validationResult.IsSuccessful() && validationResult.ErrorMessage is not null)
            {
                errors.Add(new ValidationError(validationResult.ErrorMessage, [argument.DescriptorArgument.Name]));
            }
        }

        if (errors.Count > 0)
        {
            return Result.Invalid<MemberAndTypeDescriptor>($"Validation of member {context.FunctionCall.Name} failed, see validation errors for more details", errors);
        }

        return Result.Success(new MemberAndTypeDescriptor(member, functionDescriptor.ReturnValueType));
    }

    private static bool IsTypeValid(ExpressionEvaluatorSettings settings, MemberDescriptorArgument descriptorArgument, ExpressionParseResult callArgumentResult)
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
