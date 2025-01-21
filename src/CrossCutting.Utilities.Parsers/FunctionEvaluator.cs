using CrossCutting.Common.Extensions;

namespace CrossCutting.Utilities.Parsers;

public class FunctionEvaluator : IFunctionEvaluator
{
    private readonly IFunctionDescriptorProvider _functionDescriptorProvider;
    private readonly IEnumerable<IFunction> _functions;
    private readonly IFunctionCallArgumentValidator _functionCallArgumentValidator;

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
    
    public FunctionEvaluator(IFunctionDescriptorProvider functionDescriptorProvider, IFunctionCallArgumentValidator functionCallArgumentValidator, IEnumerable<IFunction> functions)
    {
        ArgumentGuard.IsNotNull(functionDescriptorProvider, nameof(functionDescriptorProvider));
        ArgumentGuard.IsNotNull(functionCallArgumentValidator, nameof(functionCallArgumentValidator));
        ArgumentGuard.IsNotNull(functions, nameof(functions));

        _functionDescriptorProvider = functionDescriptorProvider;
        _functionCallArgumentValidator = functionCallArgumentValidator;
        _functions = functions;
    }

    public Result<object?> Evaluate(FunctionCall functionCall, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
    {
        if (functionCall is null)
        {
            return Result.Invalid<object?>("Function call is required");
        }

        var functionCallContext = new FunctionCallContext(functionCall, this, expressionEvaluator, formatProvider, context);

        return ResolveFunction(functionCallContext).Transform(result => result.Evaluate(functionCallContext));
    }

    public Result<T> EvaluateTyped<T>(FunctionCall functionCall, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
    {
        if (functionCall is null)
        {
            return Result.Invalid<T>("Function call is required");
        }

        var functionCallContext = new FunctionCallContext(functionCall, this, expressionEvaluator, formatProvider, context);

        var functionResult = ResolveFunction(functionCallContext);
        if (functionResult.Value is ITypedFunction<T> typedFunction)
        {
            return typedFunction.EvaluateTyped(functionCallContext);
        }

        return functionResult.Transform(result => result.Evaluate(functionCallContext).TryCast<T>());
    }

    public Result<Type> Validate(FunctionCall functionCall, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
    {
        if (functionCall is null)
        {
            return Result.Invalid<Type>("Function call is required");
        }

        var functionCallContext = new FunctionCallContext(functionCall, this, expressionEvaluator, formatProvider, context);

        return ResolveFunction(functionCallContext)
            .Transform(result => (result.Value as IValidatableFunction)?.Validate(functionCallContext) ?? Result.FromExistingResult(result, TryGetTypeFromResult(result)));
    }

    private static Type TryGetTypeFromResult(Result<IFunction> result)
    {
        if (result.InnerResults.Count != 1)
        {
            //TODO: Find first ITypedExpression<T> implementation, then get the generic argument, and return this type.
            return null!;
        }

        return result.InnerResults.First().TryCastValueAs<Type>()!;
    }

    private Result<IFunction> ResolveFunction(FunctionCallContext functionCallContext)
    {
        var functionsByName = Descriptors.Where(x => x.Name.Equals(functionCallContext.FunctionCall.Name, StringComparison.OrdinalIgnoreCase)).ToArray();
        if (functionsByName.Length == 0)
        {
            return Result.Invalid<IFunction>($"Unknown function: {functionCallContext.FunctionCall.Name}");
        }

        var functionsWithRightArgumentCount = functionsByName.Length == 1
            ? functionsByName.Where(x => x.Arguments.Count(x => x.IsRequired) <= functionCallContext.FunctionCall.Arguments.Count).ToArray()
            : functionsByName.Where(x => x.Arguments.Count == functionCallContext.FunctionCall.Arguments.Count).ToArray();

        return functionsWithRightArgumentCount.Length switch
        {
            0 => Result.Invalid<IFunction>($"No overload of the {functionCallContext.FunctionCall.Name} function takes {functionCallContext.FunctionCall.Arguments.Count} arguments"),
            1 => GetFunctionByDescriptor(functionCallContext, functionsWithRightArgumentCount[0]),
            _ => Result.Invalid<IFunction>($"Function {functionCallContext.FunctionCall.Name} with {functionCallContext.FunctionCall.Arguments.Count} arguments could not be identified uniquely")
        };
    }

    private Result<IFunction> GetFunctionByDescriptor(FunctionCallContext functionCallContext, FunctionDescriptor functionDescriptor)
    {
        var function = _functions.FirstOrDefault(x => x.GetType() == functionDescriptor.FunctionType);
        if (function is null)
        {
            return Result.Error<IFunction>($"Could not find function with type name {functionDescriptor.FunctionType.FullName}");
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
            return Result.Invalid<IFunction>($"Could not evaluate function {functionCallContext.FunctionCall.Name}, see inner results for more details", errors);
        }

        return Result.Success([Result.Success(functionDescriptor.ReturnValueType!)], function);
    }
}
