namespace CrossCutting.Utilities.Parsers;

public class FunctionDescriptorProvider : IFunctionDescriptorProvider
{
    private readonly IFunctionDescriptorMapper _functionDescriptorMapper;
    private readonly IFunction[] _functions;

    public FunctionDescriptorProvider(IFunctionDescriptorMapper functionDescriptorMapper, IEnumerable<IFunction> functions)
    {
        ArgumentGuard.IsNotNull(functionDescriptorMapper, nameof(functionDescriptorMapper));
        ArgumentGuard.IsNotNull(functions, nameof(functions));

        _functionDescriptorMapper = functionDescriptorMapper;
        _functions = functions.ToArray();
    }

    public IReadOnlyCollection<FunctionDescriptor> GetAll()
        => _functions.SelectMany(x => _functionDescriptorMapper.Map(x, null)).ToList();
}
