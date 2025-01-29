namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionString
{
    Result<Type> Validate(ExpressionStringEvaluatorContext context);

    Result<object?> Evaluate(ExpressionStringEvaluatorContext context);
}
