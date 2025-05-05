namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(bool))]
[MemberArgument("Expression", typeof(object))]
public class IsNullFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var expressionResult = context.GetArgumentValueResult(0, "Expression");
        if (!expressionResult.IsSuccessful())
        {
            return expressionResult;
        }

        return Result.Success<object?>(expressionResult.Value is null);
    }
}
