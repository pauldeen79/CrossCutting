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

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            if (!Descriptors.IsSuccessful())
            {
                return Result.FromExistingResult<MemberAndTypeDescriptor>(Descriptors);
            }

            var functionsByName = Descriptors
                .GetValueOrThrow()
                .Where(x => x.Name.Equals(functionCallContext.FunctionCall.Name, StringComparison.OrdinalIgnoreCase))
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
        }
        catch (Exception ex)
        {
            return Result.Error<MemberAndTypeDescriptor>(ex, "Error occured while resolving function, see Exception for more details");
        }
#pragma warning restore CA1031 // Do not catch general exception types
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
