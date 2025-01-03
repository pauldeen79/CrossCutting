namespace CrossCutting.Utilities.Parsers;

public class FunctionDescriptorProvider : IFunctionDescriptorProvider
{
    private readonly IFunction[] _functions;

    public FunctionDescriptorProvider(IEnumerable<IFunction> functions)
    {
        ArgumentGuard.IsNotNull(functions, nameof(functions));

        _functions = functions.ToArray();
    }

    public IReadOnlyCollection<FunctionDescriptor> GetAll() => _functions.Select(CreateFunction).ToList();

    private static FunctionDescriptor CreateFunction(IFunction source)
    {
        var builder = new FunctionDescriptorBuilder();

        var type = source.GetType();
        var nameAttribute = type.GetCustomAttribute<FunctionNameAttribute>();
        builder.Name = nameAttribute?.Name ?? type.Name.ReplaceSuffix("Function", string.Empty, StringComparison.Ordinal);
        builder.Description = type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty;
        builder.Arguments = type.GetCustomAttributes<FunctionArgumentAttribute>().Select(CreateFunctionArgument).ToList();

        return builder.Build();
    }

    private static FunctionDescriptorArgumentBuilder CreateFunctionArgument(FunctionArgumentAttribute attribute)
        => new FunctionDescriptorArgumentBuilder()
            .WithName(attribute.Name)
            .WithDescription(attribute.Description)
            .WithTypeName(attribute.TypeName)
            .WithIsRequired(attribute.IsRequired);
}
