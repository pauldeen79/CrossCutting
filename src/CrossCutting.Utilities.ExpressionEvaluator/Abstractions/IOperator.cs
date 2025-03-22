namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

internal interface IOperator
{
    Result<bool> Evaluate(Condition condition, ExpressionEvaluatorContext context);
}
