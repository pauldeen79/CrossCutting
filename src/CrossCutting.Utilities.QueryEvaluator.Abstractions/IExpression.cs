namespace CrossCutting.Utilities.QueryEvaluator.Abstractions;

public partial interface IExpression
{
    Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);
}
