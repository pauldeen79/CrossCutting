namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(ICollection.Count))]
[MemberInstanceType(typeof(ICollection))]
[MemberResultType(typeof(int))]
public class CollectionCountProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<ICollection>()
            .Transform(x => Result.Success<object?>(x.GetValueOrThrow().Count));
    }
}
