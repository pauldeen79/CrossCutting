namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IMathematicExpressionEvaluator
{
    Result<Type> Validate(ExpressionEvaluatorContext context);

    Result<object?> Evaluate(ExpressionEvaluatorContext context);
}
