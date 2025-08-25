namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberArgument("Type", typeof(Type))]
[MemberArgument("Expression", typeof(object))]
public class ConvertFunction : IFunction
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add<Type>(context, 0, "Type", token)
            .Add(context, 1, "Expression", token)
            .Build().ConfigureAwait(false))
            .OnSuccess(results => Result.WrapException(() => Convert.ChangeType(results.GetValue("Expression")!, results.GetValue<Type>("Type"))));
}
