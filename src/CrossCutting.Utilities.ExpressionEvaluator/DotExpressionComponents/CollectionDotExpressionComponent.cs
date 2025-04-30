namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class CollectionDotExpressionComponent : DotExpressionComponentBase<ICollection>, IDynamicDescriptorsProvider
{
    private static readonly MemberDescriptor _countDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(ICollection.Count))
        .WithInstanceType(typeof(ICollection))
        .WithMemberType(MemberType.Property)
        .WithReturnValueType(typeof(int))
        .WithImplementationType(typeof(CollectionDotExpressionComponent));

    public override int Order => 12;

    public CollectionDotExpressionComponent() : base(new DotExpressionDescriptor<ICollection>(new Dictionary<string, DotExpressionDelegates<ICollection>>()
    {
        { nameof(ICollection.Count), new DotExpressionDelegates<ICollection>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Count)) },
    }))
    {
    }

    public IEnumerable<MemberDescriptor> GetDescriptors()
        => [_countDescriptor];
}
