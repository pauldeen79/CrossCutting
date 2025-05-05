namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(ICollection.Count))]
[MemberInstanceType(typeof(ICollection))]
[MemberResultType(typeof(int))]
public class CollectionCountProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResult<ICollection>())
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<ICollection>(Constants.Instance).Count));
    }
}
