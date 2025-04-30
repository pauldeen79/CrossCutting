namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMember
{
    Result<object?> Evaluate(FunctionCallContext context);
}
