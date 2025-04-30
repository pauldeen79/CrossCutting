namespace CrossCutting.Utilities.ExpressionEvaluator;

public class MemberDescriptorMapper : IMemberDescriptorMapper
{
    public IEnumerable<MemberDescriptor> Map(object source, Type? customFunctionType)
    {
        source = ArgumentGuard.IsNotNull(source, nameof(source));

        var type = source.GetType();

        if (source is IDynamicDescriptorsProvider dynamicDescriptorsFunction)
        {
            foreach (var descriptor in dynamicDescriptorsFunction.GetDescriptors())
            {
                yield return descriptor;
            }
        }
        else
        {
            yield return new MemberDescriptorBuilder()
                .WithMemberType(GetMemberType(source))
                .WithName(type.GetCustomAttribute<FunctionNameAttribute>()?.Name ?? type.Name.ReplaceSuffix("Function", string.Empty, StringComparison.Ordinal))
                .WithDescription(type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty)
                .WithImplementationType(customFunctionType ?? type)
                .WithReturnValueType(type.GetCustomAttribute<FunctionResultTypeAttribute>()?.Type)
                .AddArguments(type.GetCustomAttributes<FunctionArgumentAttribute>().Select(CreateFunctionArgument))
                .AddTypeArguments(type.GetCustomAttributes<FunctionTypeArgumentAttribute>().Select(CreateFunctionTypeArgument))
                .AddResults(type.GetCustomAttributes<FunctionResultAttribute>().Select(CreateFunctionResult))
                .Build();
        }
    }

    private static MemberType GetMemberType(object source)
        => source switch
        {
            IGenericFunction => MemberType.GenericFunction,
            IFunction => MemberType.Function,
            _ => MemberType.Unknown
        };

    private static MemberDescriptorArgumentBuilder CreateFunctionArgument(FunctionArgumentAttribute attribute)
        => new MemberDescriptorArgumentBuilder()
            .WithName(attribute.Name)
            .WithDescription(attribute.Description)
            .WithType(attribute.Type)
            .WithIsRequired(attribute.IsRequired);

    private static MemberDescriptorTypeArgumentBuilder CreateFunctionTypeArgument(FunctionTypeArgumentAttribute attribute)
        => new MemberDescriptorTypeArgumentBuilder()
            .WithName(attribute.Name)
            .WithDescription(attribute.Description);

    private static MemberDescriptorResultBuilder CreateFunctionResult(FunctionResultAttribute attribute)
        => new MemberDescriptorResultBuilder()
            .WithDescription(attribute.Description)
            .WithStatus(attribute.Status)
            .WithValue(attribute.Value)
            .WithValueType(attribute.ValueType);
}
