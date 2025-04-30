namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionResolver : IFunctionResolver
{
    private readonly IFunctionCallArgumentValidator _functionCallArgumentValidator;
    private readonly IEnumerable<IFunction> _functions;
    private readonly IEnumerable<IGenericFunction> _genericFunctions;

    public FunctionResolver(
        IFunctionDescriptorProvider functionDescriptorProvider,
        IFunctionCallArgumentValidator functionCallArgumentValidator,
        IEnumerable<IFunction> functions,
        IEnumerable<IGenericFunction> genericFunctions)
    {
        functionDescriptorProvider = ArgumentGuard.IsNotNull(functionDescriptorProvider, nameof(functionDescriptorProvider));
        ArgumentGuard.IsNotNull(functionCallArgumentValidator, nameof(functionCallArgumentValidator));
        ArgumentGuard.IsNotNull(functions, nameof(functions));
        ArgumentGuard.IsNotNull(genericFunctions, nameof(genericFunctions));

        _functionCallArgumentValidator = functionCallArgumentValidator;
        _descriptors = new Lazy<IReadOnlyCollection<FunctionDescriptor>>(functionDescriptorProvider.GetAll);
        _functions = functions;
        _genericFunctions = genericFunctions;
    }

    private readonly Lazy<IReadOnlyCollection<FunctionDescriptor>> _descriptors;

    public IReadOnlyCollection<FunctionDescriptor> Descriptors => _descriptors.Value;

    public Result<FunctionAndTypeDescriptor> Resolve(FunctionCallContext functionCallContext)
    {
        functionCallContext = ArgumentGuard.IsNotNull(functionCallContext, nameof(functionCallContext));

        var functionsByName = Descriptors
            .Where(x => x.Name.Equals(functionCallContext.FunctionCall.Name, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (functionsByName.Length == 0)
        {
            return Result.NotFound<FunctionAndTypeDescriptor>($"Unknown function: {functionCallContext.FunctionCall.Name}");
        }

        var functionsWithRightArgumentCount = functionsByName.Length == 1
            ? functionsByName.Where(x => functionCallContext.FunctionCall.Arguments.Count >= x.Arguments.Count(x => x.IsRequired)).ToArray()
            : functionsByName.Where(x => functionCallContext.FunctionCall.Arguments.Count == x.Arguments.Count).ToArray();

        return functionsWithRightArgumentCount.Length switch
        {
            0 => Result.NotFound<FunctionAndTypeDescriptor>($"No overload of the {functionCallContext.FunctionCall.Name} function takes {functionCallContext.FunctionCall.Arguments.Count} arguments"),
            1 => GetFunctionByDescriptor(functionCallContext, functionsWithRightArgumentCount[0]),
            _ => Result.NotFound<FunctionAndTypeDescriptor>($"Function {functionCallContext.FunctionCall.Name} with {functionCallContext.FunctionCall.Arguments.Count} arguments could not be identified uniquely")
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
                return Result.NotFound<FunctionAndTypeDescriptor>($"Could not find function with type name {functionDescriptor.FunctionType.FullName}");
            }
        }

        return FunctionCallArgumentValidator.Validate(_functionCallArgumentValidator, functionCallContext, functionDescriptor, genericFunction, function);
    }
}
