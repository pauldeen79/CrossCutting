namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionString
{
    Result Validate(ExpressionStringEvaluatorState state);

    Result<object?> Evaluate(ExpressionStringEvaluatorState state);
}
