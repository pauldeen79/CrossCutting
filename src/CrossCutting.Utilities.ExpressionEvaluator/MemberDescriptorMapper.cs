namespace CrossCutting.Utilities.ExpressionEvaluator;

public class MemberDescriptorMapper : IMemberDescriptorMapper
{
    public Result<IReadOnlyCollection<MemberDescriptor>> Map(object source, Type? customImplementationType)
    {
        source = ArgumentGuard.IsNotNull(source, nameof(source));

        var type = source.GetType();
        var descriptors = new List<MemberDescriptor>();

        return Result.WrapException(() =>
        {
            if (source is IDynamicDescriptorsProvider dynamicDescriptorsFunction)
            {
                var descriptorsResult = dynamicDescriptorsFunction.GetDescriptors();
                if (!descriptorsResult.IsSuccessful())
                {
                    return descriptorsResult;
                }

                descriptors.AddRange(descriptorsResult.GetValueOrThrow());
            }
            else
            {
                descriptors.Add(new MemberDescriptorBuilder()
                    .WithMemberType(GetMemberType(source))
                    .WithName(type.GetCustomAttribute<MemberNameAttribute>()?.Name ?? type.Name.ReplaceSuffix("Function", string.Empty, StringComparison.Ordinal))
                    .WithDescription(type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty)
                    .WithImplementationType(customImplementationType ?? type)
                    .WithInstanceType(type.GetCustomAttribute<MemberInstanceTypeAttribute>()?.Type)
                    .WithReturnValueType(type.GetCustomAttribute<MemberResultTypeAttribute>()?.Type)
                    .AddArguments(type.GetCustomAttributes<MemberArgumentAttribute>().Select(CreateFunctionArgument))
                    .AddTypeArguments(type.GetCustomAttributes<MemberTypeArgumentAttribute>().Select(CreateFunctionTypeArgument))
                    .AddResults(type.GetCustomAttributes<MemberResultAttribute>().Select(CreateFunctionResult)));
            }

            return Result.Success<IReadOnlyCollection<MemberDescriptor>>(descriptors);
        });
    }

    private static MemberType GetMemberType(object? source)
        => source switch
        {
            IGenericFunction => MemberType.GenericFunction,
            IFunction => MemberType.Function,
            IMethod => MemberType.Method,
            IProperty => MemberType.Property,
            IConstructor => MemberType.Constructor,
            _ => MemberType.Unknown
        };

    private static MemberDescriptorArgumentBuilder CreateFunctionArgument(MemberArgumentAttribute attribute)
        => new MemberDescriptorArgumentBuilder()
            .WithName(attribute.Name)
            .WithDescription(attribute.Description)
            .WithType(attribute.Type)
            .WithIsRequired(attribute.IsRequired);

    private static MemberDescriptorTypeArgumentBuilder CreateFunctionTypeArgument(MemberTypeArgumentAttribute attribute)
        => new MemberDescriptorTypeArgumentBuilder()
            .WithName(attribute.Name)
            .WithDescription(attribute.Description);

    private static MemberDescriptorResultBuilder CreateFunctionResult(MemberResultAttribute attribute)
        => new MemberDescriptorResultBuilder()
            .WithDescription(attribute.Description)
            .WithStatus(attribute.Status)
            .WithValue(attribute.Value)
            .WithValueType(attribute.ValueType);
}
