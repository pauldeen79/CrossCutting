namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpression
{
    Result<Type> Validate(ExpressionEvaluatorContext context);

    Result<object?> Evaluate(ExpressionEvaluatorContext context);
}
