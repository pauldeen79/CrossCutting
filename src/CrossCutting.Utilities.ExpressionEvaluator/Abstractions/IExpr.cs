namespace CrossCutting.Utilities.ExpressionEvaluator;

internal interface IExpr
{
    Result<object?> Evaluate(ExpressionEvaluatorContext context, Func<string, Result<object?>> @delegate);
}
