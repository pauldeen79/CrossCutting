namespace CrossCutting.Utilities.ExpressionEvaluator;

public class MemberDescriptorMapper : IMemberDescriptorMapper, IMemberDescriptorCallback
{
    public Result<IReadOnlyCollection<MemberDescriptor>> Map(object source, Type? customImplementationType)
    {
        source = ArgumentGuard.IsNotNull(source, nameof(source));

        var type = source.GetType();
        var descriptors = new List<MemberDescriptor>();

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            if (source is IDynamicDescriptorsProvider dynamicDescriptorsFunction)
            {
                var descriptorsResult = dynamicDescriptorsFunction.GetDescriptors(this);
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
                    .WithReturnValueType(type.GetCustomAttribute<MemberResultTypeAttribute>()?.Type)
                    .AddArguments(type.GetCustomAttributes<MemberArgumentAttribute>().Select(CreateFunctionArgument))
                    .AddTypeArguments(type.GetCustomAttributes<MemberTypeArgumentAttribute>().Select(CreateFunctionTypeArgument))
                    .AddResults(type.GetCustomAttributes<MemberResultAttribute>().Select(CreateFunctionResult)));
            }

            return Result.Success<IReadOnlyCollection<MemberDescriptor>>(descriptors);
        }
        catch (Exception ex)
        {
            return Result.Error<IReadOnlyCollection<MemberDescriptor>>(ex, "Error occured while mapping memberdescriptor, see Exception for more details");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    Result<MemberDescriptor> IMemberDescriptorCallback.Map(Delegate @delegate)
    {
        @delegate = ArgumentGuard.IsNotNull(@delegate, nameof(@delegate));

        var declaringType = @delegate.Method.DeclaringType;
        var method = @delegate.GetMethodInfo();
        var instanceType = method.GetCustomAttribute<MemberInstanceTypeAttribute>()?.Type;

        if (method is null)
        {
            return Result.Invalid<MemberDescriptor>($"Could not find method {@delegate.Method.Name} on type {declaringType.FullName}");
        }

        if (instanceType is null)
        {
            return Result.Invalid<MemberDescriptor>($"Method {@delegate.Method.Name} on type {declaringType.FullName} does not have a {nameof(MemberInstanceTypeAttribute)}, this is required");
        }

        return Result.Success<MemberDescriptor>(new MemberDescriptorBuilder()
            .WithMemberType(MemberType.Method)
            .WithName(method.GetCustomAttribute<MemberNameAttribute>()?.Name ?? @delegate.Method.Name)
            .WithDescription(method.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty)
            .WithImplementationType(declaringType)
            .WithInstanceType(instanceType)
            .WithReturnValueType(method.GetCustomAttribute<MemberResultTypeAttribute>()?.Type)
            .AddArguments(new MemberDescriptorArgumentBuilder().WithName(Constants.DotArgument).WithIsRequired().WithType(instanceType))
            .AddArguments(method.GetCustomAttributes<MemberArgumentAttribute>().Select(CreateFunctionArgument))
            // Note that TypeArguments are skipped, this is not supported at this time.
            // Besides, the MemberTypeArgumentAttribute can only be placed on a Class, at this time, to prevent mistakes ;-)
            .AddResults(method.GetCustomAttributes<MemberResultAttribute>().Select(CreateFunctionResult)));
    }

    private static MemberType GetMemberType(object source)
        => source switch
        {
            IGenericFunction => MemberType.GenericFunction,
            IFunction => MemberType.Function,
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
