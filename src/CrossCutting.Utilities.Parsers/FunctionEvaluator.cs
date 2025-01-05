namespace CrossCutting.Utilities.Parsers;

public class FunctionEvaluator : IFunctionEvaluator
{
    private readonly IFunctionDescriptorProvider _functionDescriptorProvider;
    private readonly IEnumerable<IFunction> _functions;

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
    
    public FunctionEvaluator(IFunctionDescriptorProvider functionDescriptorProvider, IEnumerable<IFunction> functions)
    {
        ArgumentGuard.IsNotNull(functionDescriptorProvider, nameof(functionDescriptorProvider));
        ArgumentGuard.IsNotNull(functions, nameof(functions));

        _functionDescriptorProvider = functionDescriptorProvider;
        _functions = functions;
    }

    public Result<object?> Evaluate(FunctionCall functionCall, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        => ResolveFunction(functionCall)
            .Transform(result => result.Evaluate(new FunctionCallRequest(functionCall, this, expressionEvaluator, formatProvider, context)));

    public Result Validate(FunctionCall functionCall, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        => ResolveFunction(functionCall)
            .Transform(result => result.Validate(new FunctionCallRequest(functionCall, this, expressionEvaluator, formatProvider, context)));

    private Result<IFunction> ResolveFunction(FunctionCall functionCall)
    {
        if (functionCall is null)
        {
            return Result.Invalid<IFunction>("Function call is required");
        }

        var functionsByName = Descriptors.Where(x => x.Name.Equals(functionCall.Name, StringComparison.OrdinalIgnoreCase)).ToArray();
        return functionsByName.Length switch
        {
            0 => Result.Invalid<IFunction>($"Unknown function: {functionCall.Name}"),
            1 => GetFunctionByDescriptor(functionsByName[0]),
            _ => GetFunctionOverloadByDescriptors(functionCall, functionsByName)
        };
    }

    private Result<IFunction> GetFunctionByDescriptor(FunctionDescriptor functionDescriptor)
    {
        var function = _functions.FirstOrDefault(x => x.GetType().FullName == functionDescriptor.TypeName);
        if (function is null)
        {
            return Result.Error<IFunction>($"Could not find function with type name {functionDescriptor.TypeName}");
        }

        return Result.Success(function);
    }

    private Result<IFunction> GetFunctionOverloadByDescriptors(FunctionCall functionCall, FunctionDescriptor[] functionsByName)
    {
        var functionsWithRightArgumentCount = functionsByName.Where(x => x.Arguments.Count == functionCall.Arguments.Count).ToArray();

        return functionsWithRightArgumentCount.Length switch
        {
            0 => Result.Invalid<IFunction>($"No overload of the {functionCall.Name} function takes {functionCall.Arguments.Count} arguments"),
            1 => GetFunctionByDescriptor(functionsWithRightArgumentCount[0]),
            _ => Result.Invalid<IFunction>($"Function {functionCall.Name} with {functionCall.Arguments.Count} arguments could not be identified uniquely")
        };
    }
}
