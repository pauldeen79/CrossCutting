namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberArgument("Type", typeof(Type))]
[MemberArgument("Expression", typeof(object))]
public class ConvertFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<Type>(context, 0, "Type")
            .Add(context, 1, "Expression")
            .Build()
            .OnSuccess(results => Result.WrapException(() => Result.Success<object?>(Convert.ChangeType(results.GetValue("Expression")!, results.GetValue<Type>("Type")))));
}
