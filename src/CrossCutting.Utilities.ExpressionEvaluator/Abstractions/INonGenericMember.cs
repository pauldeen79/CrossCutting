namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface INonGenericMember : IMember
{
    Task<Result<object?>> EvaluateAsync(FunctionCallContext context);
}
