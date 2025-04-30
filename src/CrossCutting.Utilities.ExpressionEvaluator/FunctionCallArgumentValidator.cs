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

    public static Result<Type> Validate(DotExpressionComponentState state, IFunctionCallArgumentValidator validator, FunctionDescriptor functionDescriptor)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));
        validator = ArgumentGuard.IsNotNull(validator, nameof(validator));
        functionDescriptor = ArgumentGuard.IsNotNull(functionDescriptor, nameof(functionDescriptor));

        // Little hacking here... We need to add an 'instance' argument (sort of an extension method), to construct a FunctionCall from this DotExpression...
        var functionCall = state.FunctionParseResult.Value!.ToBuilder().Chain(x => x.Arguments.Insert(0, Constants.DummyArgument)).Build();

        var result = Validate(validator, new FunctionCallContext(functionCall, state.Context), functionDescriptor, null, null);
        if (!result.IsSuccessful())
        {
            return Result.FromExistingResult<Type>(result);
        }

        return Result.Success(result.Value?.ReturnValueType!);
    }

    public static Result<FunctionAndTypeDescriptor> Validate(IFunctionCallArgumentValidator functionCallArgumentValidator, FunctionCallContext functionCallContext, FunctionDescriptor functionDescriptor, IGenericFunction? genericFunction, IFunction? function)
    {
        functionCallArgumentValidator = ArgumentGuard.IsNotNull(functionCallArgumentValidator, nameof(functionCallArgumentValidator));
        functionCallContext = ArgumentGuard.IsNotNull(functionCallContext, nameof(functionCallContext));
        functionDescriptor = ArgumentGuard.IsNotNull(functionDescriptor, nameof(functionDescriptor));

        var arguments = functionDescriptor.Arguments.Zip(functionCallContext.FunctionCall.Arguments, (descriptor, call) => new FunctionArgumentInfo(descriptor, call));

        var errors = new List<ValidationError>();
        foreach (var argument in arguments.Where(x => x.CallArgument != Constants.DummyArgument))
        {
            var validationResult = functionCallArgumentValidator.Validate(argument.DescriptorArgument, argument.CallArgument, functionCallContext);
            if (!validationResult.IsSuccessful() && validationResult.ErrorMessage is not null)
            {
                errors.Add(new ValidationError(validationResult.ErrorMessage, [argument.DescriptorArgument.Name]));
            }
        }

        if (errors.Count > 0)
        {
            return Result.Invalid<FunctionAndTypeDescriptor>($"Validation of function {functionCallContext.FunctionCall.Name} failed, see validation errors for more details", errors);
        }

        return Result.Success(new FunctionAndTypeDescriptor(function, genericFunction, functionDescriptor.ReturnValueType));
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
