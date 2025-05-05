namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface INonGenericMember : IMember
{
    Result<object?> Evaluate(FunctionCallContext context);
}
