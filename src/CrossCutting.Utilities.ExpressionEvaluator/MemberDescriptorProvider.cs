namespace CrossCutting.Utilities.ExpressionEvaluator;

public class MemberDescriptorProvider(IMemberDescriptorMapper functionDescriptorMapper, IEnumerable<IMember> members) : IMemberDescriptorProvider
{
    private readonly IMemberDescriptorMapper _functionDescriptorMapper = ArgumentGuard.IsNotNull(functionDescriptorMapper, nameof(functionDescriptorMapper));
    private readonly IMember[] _members = ArgumentGuard.IsNotNull(members, nameof(members)).ToArray();

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
