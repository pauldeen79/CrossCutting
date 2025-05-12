namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(ICollection.Count))]
[MemberInstanceType(typeof(ICollection))]
[MemberResultType(typeof(int))]
public class CollectionCountProperty : IProperty
{
    public Task<Result<object?>> EvaluateAsync(FunctionCallContext context)
        => Task.Run(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            return context.GetInstanceValueResult<ICollection>().Transform<object?>(result => result.Count);
        });
}
