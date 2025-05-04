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

    public Result<MemberAndTypeDescriptor> Resolve(FunctionCallContext functionCallContext)
    {
        functionCallContext = ArgumentGuard.IsNotNull(functionCallContext, nameof(functionCallContext));

        return Result.WrapException(() =>
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
                    && IsInstanceTypeValid(x.InstanceType, functionCallContext.InstanceValue))
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
                1 => GetFunctionByDescriptor(functionCallContext, functionsWithRightArgumentCount[0]),
                _ => Result.NotFound<MemberAndTypeDescriptor>($"Function {functionCallContext.FunctionCall.Name} with {functionCallContext.FunctionCall.Arguments.Count} arguments could not be identified uniquely")
            };
        });
    }

    private static bool IsMemberTypeValid(MemberType descriptorMemberType, MemberType contextMemberType)
    {
        if (contextMemberType == MemberType.Function)
        {
            return descriptorMemberType.In(MemberType.Function, MemberType.GenericFunction);
        }

        return descriptorMemberType == contextMemberType;
    }

    private static bool IsInstanceTypeValid(Type? instanceType, object? instanceValue)
    {
        if (instanceType is null)
        {
            // Function/GenericFunction
            return instanceValue is null;
        }

        if (instanceValue is null)
        {
            // There is an instance type, but the instance value is null.
            // This should not be possible, and the calling component should handle this.
            return false;
        }

        if (instanceValue is Type instanceValueType)
        {
            // Validate
            return instanceType!.IsAssignableFrom(instanceValueType);
        }

        // Evaluate
        return instanceType!.IsInstanceOfType(instanceValue);
    }

    private Result<MemberAndTypeDescriptor> GetFunctionByDescriptor(FunctionCallContext functionCallContext, MemberDescriptor memberDescriptor)
    {
        var member = _members.FirstOrDefault(x => x.GetType() == memberDescriptor.ImplementationType);

        if (member is null)
        {
            return Result.NotFound<MemberAndTypeDescriptor>($"Could not find member with type name {memberDescriptor.ImplementationType.FullName}");
        }

        return MemberCallArgumentValidator.Validate(_memberCallArgumentValidator, functionCallContext, memberDescriptor, member);
    }
}
