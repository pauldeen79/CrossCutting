namespace CrossCutting.Utilities.ExpressionEvaluator;

public class MemberResolver : IMemberResolver
{
    private readonly IMemberCallArgumentValidator _memberCallArgumentValidator;
    private readonly IEnumerable<IMember> _members;

    public MemberResolver(
        IMemberDescriptorProvider memberDescriptorProvider,
        IMemberCallArgumentValidator memberCallArgumentValidator,
        IEnumerable<IMember> members)
    {
        memberDescriptorProvider = ArgumentGuard.IsNotNull(memberDescriptorProvider, nameof(memberDescriptorProvider));
        ArgumentGuard.IsNotNull(memberCallArgumentValidator, nameof(memberCallArgumentValidator));
        ArgumentGuard.IsNotNull(members, nameof(members));

        _memberCallArgumentValidator = memberCallArgumentValidator;
        _descriptors = new Lazy<Result<IReadOnlyCollection<MemberDescriptor>>>(memberDescriptorProvider.GetAll);
        _members = members;
    }

    private readonly Lazy<Result<IReadOnlyCollection<MemberDescriptor>>> _descriptors;

    public Result<IReadOnlyCollection<MemberDescriptor>> Descriptors => _descriptors.Value;

    public async Task<Result<MemberAndTypeDescriptor>> ResolveAsync(FunctionCallContext functionCallContext)
    {
        functionCallContext = ArgumentGuard.IsNotNull(functionCallContext, nameof(functionCallContext));

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            if (!Descriptors.IsSuccessful())
            {
                return Result.FromExistingResult<MemberAndTypeDescriptor>(Descriptors);
            }

            var functionsByName = Descriptors
                .GetValueOrThrow()
                .Where(x =>
                    x.Name.Equals(functionCallContext.FunctionCall.Name, StringComparison.OrdinalIgnoreCase)
                    && IsMemberTypeValid(x.MemberType, functionCallContext.MemberType)
                    && IsInstanceTypeValid(x.InstanceType, functionCallContext.InstanceValue, functionCallContext.ResultType, functionCallContext.MemberType))
                .ToArray();

            if (functionsByName.Length == 0)
            {
                return Result.NotFound<MemberAndTypeDescriptor>($"Unknown function: {functionCallContext.FunctionCall.Name}");
            }

            var functionsWithRightArgumentCount = functionsByName.Length == 1
                ? functionsByName.Where(x => functionCallContext.FunctionCall.Arguments.Count >= x.Arguments.Count(x => x.IsRequired)).ToArray()
                : functionsByName.Where(x => functionCallContext.FunctionCall.Arguments.Count == x.Arguments.Count).ToArray();

            return functionsWithRightArgumentCount.Length switch
            {
                0 => Result.NotFound<MemberAndTypeDescriptor>($"No overload of the {functionCallContext.FunctionCall.Name} function takes {functionCallContext.FunctionCall.Arguments.Count} arguments"),
                1 => await GetFunctionByDescriptor(functionCallContext, functionsWithRightArgumentCount[0]).ConfigureAwait(false),
                _ => Result.NotFound<MemberAndTypeDescriptor>($"Function {functionCallContext.FunctionCall.Name} with {functionCallContext.FunctionCall.Arguments.Count} arguments could not be identified uniquely"),
            };
        }
        catch (Exception ex)
        {
            return Result.Error<MemberAndTypeDescriptor>(ex, "Exception occured");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    private static bool IsMemberTypeValid(MemberType descriptorMemberType, MemberType contextMemberType)
    {
        if (contextMemberType == MemberType.Function)
        {
            return descriptorMemberType.In(MemberType.Function, MemberType.GenericFunction);
        }

        return descriptorMemberType == contextMemberType;
    }

    private static bool IsInstanceTypeValid(Type? instanceType, object? instanceValue, Type? resultType, MemberType memberType)
    {
        if (memberType == MemberType.Constructor)
        {
            return instanceType is not null && instanceValue is null;
        }

        if (instanceType is null)
        {
            // Function/GenericFunction
            return instanceValue is null;
        }

        if (resultType is not null)
        {
            // Validate
            return instanceType!.IsAssignableFrom(resultType);
        }

        // Evaluate
        return instanceType!.IsInstanceOfType(instanceValue);
    }

    private async Task<Result<MemberAndTypeDescriptor>> GetFunctionByDescriptor(FunctionCallContext functionCallContext, MemberDescriptor memberDescriptor)
    {
        var member = _members.FirstOrDefault(x => x.GetType() == memberDescriptor.ImplementationType);

        if (member is null)
        {
            return Result.NotFound<MemberAndTypeDescriptor>($"Could not find member with type name {memberDescriptor.ImplementationType.FullName}");
        }

        return await _memberCallArgumentValidator.ValidateAsync(memberDescriptor, member, functionCallContext).ConfigureAwait(false);
    }
}
