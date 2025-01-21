namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionString
{
    Result<Type> Validate(ExpressionStringEvaluatorState state);

    Result<object?> Evaluate(ExpressionStringEvaluatorState state);
}
