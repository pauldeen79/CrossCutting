namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionDescriptorMapper : IFunctionDescriptorMapper
{
    public IEnumerable<FunctionDescriptor> Map(object source, Type? customFunctionType)
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
                .WithReturnValueType(type.GetCustomAttribute<FunctionResultTypeAttribute>()?.Type ?? GetTypedResultType(type))
                .AddArguments(type.GetCustomAttributes<FunctionArgumentAttribute>().Select(CreateFunctionArgument))
                .AddTypeArguments(type.GetCustomAttributes<FunctionTypeArgumentAttribute>().Select(CreateFunctionTypeArgument))
                .AddResults(type.GetCustomAttributes<FunctionResultAttribute>().Select(CreateFunctionResult))
                .Build();
        }
    }

    private static Type? GetTypedResultType(Type type)
    {
        // Only when you implement one ITypedFunction<T>!
        var typedFunctions = type.GetInterfaces().Where(x => x.FullName.StartsWith("CrossCutting.Utilities.Parsers.Contracts.ITypedFunction")).ToArray();
        if (typedFunctions.Length == 1)
        {
            return typedFunctions[0].GenericTypeArguments[0];
        }

        return null;
    }

    private static FunctionDescriptorArgumentBuilder CreateFunctionArgument(FunctionArgumentAttribute attribute)
        => new FunctionDescriptorArgumentBuilder()
            .WithName(attribute.Name)
            .WithDescription(attribute.Description)
            .WithType(attribute.Type)
            .WithIsRequired(attribute.IsRequired);

    private static FunctionDescriptorTypeArgumentBuilder CreateFunctionTypeArgument(FunctionTypeArgumentAttribute attribute)
        => new FunctionDescriptorTypeArgumentBuilder()
            .WithName(attribute.Name)
            .WithDescription(attribute.Description);

    private static FunctionDescriptorResultBuilder CreateFunctionResult(FunctionResultAttribute attribute)
        => new FunctionDescriptorResultBuilder()
            .WithDescription(attribute.Description)
            .WithStatus(attribute.Status)
            .WithValue(attribute.Value)
            .WithValueType(attribute.ValueType);
}
