namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ExpressionEvaluator : IExpressionEvaluator
{
    public Result<object?> Evaluate(string expression, ExpressionEvaluatorSettings settings, object? context)
    {
        throw new NotImplementedException();
    }

    public Result<Type> Validate(string expression, ExpressionEvaluatorSettings settings, object? context)
    {
        throw new NotImplementedException();
    }
}
