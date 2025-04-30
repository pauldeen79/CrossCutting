namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class CollectionDotExpressionComponent : DotExpressionComponentBase<ICollection>
{
    public override int Order => 12;

    public CollectionDotExpressionComponent() : base(new DotExpressionDescriptor<ICollection>(new Dictionary<string, DotExpressionDelegates<ICollection>>()
    {
        { "Count", new DotExpressionDelegates<ICollection>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Count)) },
    }))
    {
    }
}
