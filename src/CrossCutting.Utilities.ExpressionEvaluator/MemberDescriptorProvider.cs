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

        foreach (var member in _members)
        {
            var result = Result.WrapException(() => _functionDescriptorMapper.Map(member, null));
            if (!result.IsSuccessful())
            {
                return result;
            }

            descriptors.AddRange(result.GetValueOrThrow());
        }

        return Result.Success<IReadOnlyCollection<MemberDescriptor>>(descriptors);
    }
}
