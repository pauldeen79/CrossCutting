namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionString
{
    int Order { get; }

    Result Validate(ExpressionStringEvaluatorState state);

    Result<object?> Evaluate(ExpressionStringEvaluatorState state);
}
