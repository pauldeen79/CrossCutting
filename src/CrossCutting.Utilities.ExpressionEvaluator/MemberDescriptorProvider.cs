namespace CrossCutting.Utilities.ExpressionEvaluator;

public class MemberDescriptorProvider : IMemberDescriptorProvider
{
    private readonly IMemberDescriptorMapper _functionDescriptorMapper;
    private readonly IEnumerable<IMember> _members;

    public MemberDescriptorProvider(IMemberDescriptorMapper functionDescriptorMapper, IEnumerable<IMember> members)
    {
        ArgumentGuard.IsNotNull(functionDescriptorMapper, nameof(functionDescriptorMapper));
        ArgumentGuard.IsNotNull(members, nameof(members));

        _functionDescriptorMapper = functionDescriptorMapper;
        _members = members;
    }

    public Result<IReadOnlyCollection<MemberDescriptor>> GetAll()
    {
        var descriptors = new List<MemberDescriptor>();

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            foreach (var member in _members)
            {
                var result = _functionDescriptorMapper.Map(member, null);
                if (!result.IsSuccessful())
                {
                    return result;
                }

                descriptors.AddRange(result.GetValueOrThrow());
            }

            return Result.Success<IReadOnlyCollection<MemberDescriptor>>(descriptors);
        }
        catch (Exception ex)
        {
            return Result.Error<IReadOnlyCollection<MemberDescriptor>>(ex, "Error occured while getting member descriptors, see Exception property for more details");
        }
#pragma warning restore CA1031 // Do not catch general exception types

    }
}
