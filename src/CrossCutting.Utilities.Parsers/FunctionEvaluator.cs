using CrossCutting.Common.Results;

namespace CrossCutting.Utilities.Parsers;

public class FunctionEvaluator : IFunctionEvaluator
{
    private readonly IFunctionDescriptorProvider _functionDescriptorProvider;
    private readonly IFunctionCallArgumentValidator _functionCallArgumentValidator;
    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly IEnumerable<IFunction> _functions;
    private readonly IEnumerable<IGenericFunction> _genericFunctions;

    private IReadOnlyCollection<FunctionDescriptor>? _descriptors;
    private IReadOnlyCollection<FunctionDescriptor> Descriptors
    {
        get
        {
            if (_descriptors is null)
            {
                _descriptors = _functionDescriptorProvider.GetAll();
            }
            return _descriptors;
        }
    }
    
    public FunctionEvaluator(IFunctionDescriptorProvider functionDescriptorProvider, IFunctionCallArgumentValidator functionCallArgumentValidator, IExpressionEvaluator expressionEvaluator, IEnumerable<IFunction> functions, IEnumerable<IGenericFunction> genericFunctions)
    {
        ArgumentGuard.IsNotNull(functionDescriptorProvider, nameof(functionDescriptorProvider));
        ArgumentGuard.IsNotNull(functionCallArgumentValidator, nameof(functionCallArgumentValidator));
        ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));
        ArgumentGuard.IsNotNull(functions, nameof(functions));
        ArgumentGuard.IsNotNull(genericFunctions, nameof(genericFunctions));

        _functionDescriptorProvider = functionDescriptorProvider;
        _functionCallArgumentValidator = functionCallArgumentValidator;
        _expressionEvaluator = expressionEvaluator;
        _functions = functions;
        _genericFunctions = genericFunctions;
    }

    public Result<object?> Evaluate(FunctionCall functionCall, FunctionEvaluatorSettings settings, object? context)
    {
        if (functionCall is null)
        {
            return Result.Invalid<object?>("Function call is required");
        }

        var functionCallContext = new FunctionCallContext(functionCall, this, _expressionEvaluator, settings, context);

        return ResolveFunction(functionCallContext).Transform(result => EvaluateFunction(result, functionCallContext));
    }

    private static Result<object?> EvaluateFunction(FunctionAndTypeDescriptor result, FunctionCallContext functionCallContext)
    {
        if (result.GenericFunction is not null)
        {
            //TODO: Use reflection magic to make method generic, and call it (using reflection)
            return result.GenericFunction!.EvaluateGeneric<object?>(functionCallContext);
        }

        // We can safely assume that Function is not null, because the c'tor has verified this
        return result.Function!.Evaluate(functionCallContext);
    }

    public Result<T> EvaluateTyped<T>(FunctionCall functionCall, FunctionEvaluatorSettings settings, object? context)
    {
        if (functionCall is null)
        {
            return Result.Invalid<T>("Function call is required");
        }

        var functionCallContext = new FunctionCallContext(functionCall, this, _expressionEvaluator, settings, context);

        var functionResult = ResolveFunction(functionCallContext);

        if (functionResult.Value?.GenericFunction is not null)
        {
            //TODO: Use reflection magic to make method generic, and call it (using reflection)
            return functionResult.Value.GenericFunction.EvaluateGeneric<object?>(functionCallContext).TryCast<T>();
        }

        if (functionResult.Value?.Function is ITypedFunction<T> typedFunction)
        {
            return typedFunction.EvaluateTyped(functionCallContext);
        }

        // We can safely assume that Function is not null, because the c'tor has verified this
        return functionResult.Transform(result => result.Function!.Evaluate(functionCallContext).TryCast<T>());
    }

    public Result<Type> Validate(FunctionCall functionCall, FunctionEvaluatorSettings settings, object? context)
    {
        if (functionCall is null)
        {
            return Result.Invalid<Type>("Function call is required");
        }

        var functionCallContext = new FunctionCallContext(functionCall, this, _expressionEvaluator, settings, context);

        return ResolveFunction(functionCallContext)
            .Transform(result => (result.Value?.Function as IValidatableFunction)?.Validate(functionCallContext) ?? Result.FromExistingResult(result, result.Value?.ReturnValueType!));
    }

    private Result<FunctionAndTypeDescriptor> ResolveFunction(FunctionCallContext functionCallContext)
    {
        var functionsByName = Descriptors
            .Where(x => x.Name.Equals(functionCallContext.FunctionCall.Name, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (functionsByName.Length == 0)
        {
            return Result.Invalid<FunctionAndTypeDescriptor>($"Unknown function: {functionCallContext.FunctionCall.Name}");
        }

        var functionsWithRightArgumentCount = functionsByName.Length == 1
            ? functionsByName.Where(x => functionCallContext.FunctionCall.Arguments.Count >= x.Arguments.Count(x => x.IsRequired)).ToArray()
            : functionsByName.Where(x => functionCallContext.FunctionCall.Arguments.Count == x.Arguments.Count).ToArray();

        return functionsWithRightArgumentCount.Length switch
        {
            0 => Result.Invalid<FunctionAndTypeDescriptor>($"No overload of the {functionCallContext.FunctionCall.Name} function takes {functionCallContext.FunctionCall.Arguments.Count} arguments"),
            1 => GetFunctionByDescriptor(functionCallContext, functionsWithRightArgumentCount[0]),
            _ => Result.Invalid<FunctionAndTypeDescriptor>($"Function {functionCallContext.FunctionCall.Name} with {functionCallContext.FunctionCall.Arguments.Count} arguments could not be identified uniquely")
        };
    }

    private Result<FunctionAndTypeDescriptor> GetFunctionByDescriptor(FunctionCallContext functionCallContext, FunctionDescriptor functionDescriptor)
    {
        IGenericFunction? genericFunction = null;

        var function = _functions.FirstOrDefault(x => x.GetType() == functionDescriptor.FunctionType);

        if (function is null)
        {
            genericFunction = _genericFunctions.FirstOrDefault(x => x.GetType() == functionDescriptor.FunctionType);
            if (genericFunction is null)
            {
                return Result.Error<FunctionAndTypeDescriptor>($"Could not find function with type name {functionDescriptor.FunctionType.FullName}");
            }
        }

        var arguments = functionDescriptor.Arguments.Zip(functionCallContext.FunctionCall.Arguments, (descriptor, call) => new { DescriptorArgument = descriptor, CallArgument = call });

        var errors = new List<Result>();
        foreach (var argument in arguments)
        {
            var validationResult = _functionCallArgumentValidator.Validate(argument.DescriptorArgument, argument.CallArgument, functionCallContext);
            if (!validationResult.IsSuccessful())
            {
                errors.Add(validationResult);
            }
        }

        if (errors.Count > 0)
        {
            return Result.Invalid<FunctionAndTypeDescriptor>($"Could not evaluate function {functionCallContext.FunctionCall.Name}, see inner results for more details", errors);
        }

        return Result.Success(new FunctionAndTypeDescriptor(function, genericFunction, functionDescriptor.ReturnValueType));
    }
}
