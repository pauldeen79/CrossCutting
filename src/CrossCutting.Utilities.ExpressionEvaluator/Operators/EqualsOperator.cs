namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

internal class EqualsOperator : IOperator
{
    public Result<bool> Evaluate(Condition condition, ExpressionEvaluatorContext context)
    {
        if (condition.Operator != "==")
        {
            return Result.Continue<bool>();
        }

        return new ResultDictionaryBuilder()
            .Add("LeftExpression", () => context.Evaluator.Evaluate(condition.LeftExpression, context.Settings, context.Context))
            .Add("RightExpression", () => context.Evaluator.Evaluate(condition.RightExpression, context.Settings, context.Context))
            .Build()
            .OnSuccess(results => Equal.Evaluate(results.GetValue("LeftExpression"), results.GetValue("RightExpression"), context.Settings.StringComparison));
    }
}
