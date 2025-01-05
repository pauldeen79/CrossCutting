namespace CrossCutting.Utilities.Parsers;

public class FunctionDescriptorProvider : IFunctionDescriptorProvider
{
    private readonly IFunction[] _functions;

    public FunctionDescriptorProvider(IEnumerable<IFunction> functions)
    {
        ArgumentGuard.IsNotNull(functions, nameof(functions));

        _functions = functions.ToArray();
    }

    public IReadOnlyCollection<FunctionDescriptor> GetAll()
        => _functions.Select(CreateFunction).ToList();

    private static FunctionDescriptor CreateFunction(IFunction source)
    {
        var type = source.GetType();

        return new FunctionDescriptorBuilder()
            .WithName(type.GetCustomAttribute<FunctionNameAttribute>()?.Name ?? type.Name.ReplaceSuffix("Function", string.Empty, StringComparison.Ordinal))
            .WithDescription(type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty)
            .WithType(type)
            .AddArguments(type.GetCustomAttributes<FunctionArgumentAttribute>().Select(CreateFunctionArgument))
            .AddResults(type.GetCustomAttributes<FunctionResultAttribute>().Select(CreateFunctionResult))
            .Build();
    }

    private static FunctionDescriptorArgumentBuilder CreateFunctionArgument(FunctionArgumentAttribute attribute)
        => new FunctionDescriptorArgumentBuilder()
            .WithName(attribute.Name)
            .WithDescription(attribute.Description)
            .WithType(attribute.Type)
            .WithIsRequired(attribute.IsRequired);

    private static FunctionDescriptorResultBuilder CreateFunctionResult(FunctionResultAttribute attribute)
        => new FunctionDescriptorResultBuilder()
            .WithDescription(attribute.Description)
            .WithStatus(attribute.Status)
            .WithValue(attribute.Value)
            .WithValueType(attribute.ValueType);
}
