namespace CrossCutting.Utilities.Parsers;

public class FunctionDescriptorMapper : IFunctionDescriptorMapper
{
    public IEnumerable<FunctionDescriptor> Map(IFunction source, Type? customFunctionType)
    {
        source = ArgumentGuard.IsNotNull(source, nameof(source));

        var type = source.GetType();

        if (source is IDynamicDescriptorsFunction dynamicDescriptorsFunction)
        {
            foreach (var descriptor in dynamicDescriptorsFunction.GetDescriptors())
            {
                yield return descriptor;
            }
        }
        else
        {
            yield return new FunctionDescriptorBuilder()
                .WithName(type.GetCustomAttribute<FunctionNameAttribute>()?.Name ?? type.Name.ReplaceSuffix("Function", string.Empty, StringComparison.Ordinal))
                .WithDescription(type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty)
                .WithFunctionType(customFunctionType ?? type)
                .WithReturnValueType(type.GetCustomAttribute<FunctionResultTypeAttribute>()?.Type)
                .AddArguments(type.GetCustomAttributes<FunctionArgumentAttribute>().Select(CreateFunctionArgument))
                .AddResults(type.GetCustomAttributes<FunctionResultAttribute>().Select(CreateFunctionResult))
                .Build();
        }
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
