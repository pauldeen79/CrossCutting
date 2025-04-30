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

    public IReadOnlyCollection<MemberDescriptor> GetAll()
        => _members.SelectMany(x => _functionDescriptorMapper.Map(x, null)).ToList();
}
