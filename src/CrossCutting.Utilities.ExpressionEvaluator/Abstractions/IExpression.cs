namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpression
{
    int Order { get; }

    Result<Type> Validate(ExpressionEvaluatorContext context);

    Result<object?> Evaluate(ExpressionEvaluatorContext context);
}
