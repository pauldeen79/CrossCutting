namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface INonFunctionMember : IMember
{
    Result<object?> Evaluate(FunctionCallContext context);
}
