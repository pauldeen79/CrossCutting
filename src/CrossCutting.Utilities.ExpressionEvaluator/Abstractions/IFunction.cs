namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IFunction : IMember
{
    Result<object?> Evaluate(FunctionCallContext context);
}
