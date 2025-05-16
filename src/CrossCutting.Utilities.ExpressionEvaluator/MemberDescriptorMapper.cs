namespace CrossCutting.Utilities.ExpressionEvaluator;

public class MemberDescriptorMapper : IMemberDescriptorMapper
{
    public Result<IReadOnlyCollection<MemberDescriptor>> Map(IMember source, Type? customImplementationType)
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
                var memberType = GetMemberType(source);
                descriptors.Add(new MemberDescriptorBuilder()
                    .WithMemberType(memberType)
                    .WithName(type.GetCustomAttribute<MemberNameAttribute>()?.Name ?? type.Name.ReplaceSuffix("Function", string.Empty, StringComparison.Ordinal))
                    .WithDescription(type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty)
                    .WithImplementationType(customImplementationType ?? type)
                    .WithInstanceType(memberType == MemberType.Constructor
                        ? type.GetCustomAttribute<MemberResultTypeAttribute>()?.Type
                        : type.GetCustomAttribute<MemberInstanceTypeAttribute>()?.Type)
                    .WithReturnValueType(type.GetCustomAttribute<MemberResultTypeAttribute>()?.Type ?? TryGetReturnValueType(type))
                    .AddArguments(type.GetCustomAttributes<MemberArgumentAttribute>().Select(CreateFunctionArgument))
                    .AddTypeArguments(type.GetCustomAttributes<MemberTypeArgumentAttribute>().Select(CreateFunctionTypeArgument))
                    .AddResults(type.GetCustomAttributes<MemberResultAttribute>().Select(CreateFunctionResult)));
            }

            return Result.Success<IReadOnlyCollection<MemberDescriptor>>(descriptors);
        });
    }

    private static Type? TryGetReturnValueType(Type type)
    {
        var typedFunctions = type.GetInterfaces().Where(x => x.FullName.StartsWith(typeof(IFunction).FullName, StringComparison.Ordinal) && x.IsGenericType && x.GenericTypeArguments.Length == 1).ToArray();
        if (typedFunctions.Length == 1)
        {
            return typedFunctions[0].GenericTypeArguments[0];
        }

        return null;
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
