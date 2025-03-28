namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionDescriptorProvider : IFunctionDescriptorProvider
{
    private readonly IFunctionDescriptorMapper _functionDescriptorMapper;
    private readonly IFunction[] _functions;
    private readonly IGenericFunction[] _genericFunctions;

    public FunctionDescriptorProvider(IFunctionDescriptorMapper functionDescriptorMapper, IEnumerable<IFunction> functions, IEnumerable<IGenericFunction> genericFunctions)
    {
        ArgumentGuard.IsNotNull(functionDescriptorMapper, nameof(functionDescriptorMapper));
        ArgumentGuard.IsNotNull(functions, nameof(functions));
        ArgumentGuard.IsNotNull(genericFunctions, nameof(genericFunctions));

        _functionDescriptorMapper = functionDescriptorMapper;
        _functions = functions.ToArray();
        _genericFunctions = genericFunctions.ToArray();
    }

    public IReadOnlyCollection<FunctionDescriptor> GetAll()
        => _functions.SelectMany(x => _functionDescriptorMapper.Map(x, null))
            .Concat(_genericFunctions.SelectMany(x => _functionDescriptorMapper.Map(x, null)))
            .ToList();
}
