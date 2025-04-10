namespace CrossCutting.Utilities.ExpressionEvaluator;

internal interface IOperatorExpression
{
    Result<object?> Evaluate(ExpressionEvaluatorContext context, Func<string, Result<object?>> @delegate);
}
