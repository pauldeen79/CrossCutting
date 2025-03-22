namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

internal sealed class SmallerThanOrEqualOperator : IOperator
{
    public Result<bool> Evaluate(Condition condition, ExpressionEvaluatorContext context)
    {
        if (condition.Operator != "<=")
        {
            return Result.Continue<bool>();
        }

        return new ResultDictionaryBuilder()
            .Add("LeftExpression", () => context.Evaluator.Evaluate(condition.LeftExpression, context.Settings, context.Context))
            .Add("RightExpression", () => context.Evaluator.Evaluate(condition.RightExpression, context.Settings, context.Context))
            .Build()
            .OnSuccess(results => SmallerOrEqualThan.Evaluate(results.GetValue("LeftExpression"), results.GetValue("RightExpression")));
    }
}
