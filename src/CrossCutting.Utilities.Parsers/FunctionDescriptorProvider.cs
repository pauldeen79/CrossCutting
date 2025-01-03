namespace CrossCutting.Utilities.Parsers;

public class FunctionDescriptorProvider : IFunctionDescriptorProvider
{
    private readonly IReadOnlyCollection<FunctionDescriptor> _functionDescriptors;

    public FunctionDescriptorProvider(IEnumerable<FunctionDescriptor> functionDescriptors)
    {
        ArgumentGuard.IsNotNull(functionDescriptors, nameof(functionDescriptors));

        _functionDescriptors = new ReadOnlyCollection<FunctionDescriptor>(functionDescriptors.ToList());
    }

    public IReadOnlyCollection<FunctionDescriptor> GetAll() => _functionDescriptors;
}
