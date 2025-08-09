namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IBinaryExpressionComponent
{
    bool HasBooleanResult { get; }

    Result<object?> Process(ExpressionTokenType type, ExpressionEvaluatorContext context, IReadOnlyDictionary<string, Result> results);

    bool Supports(ExpressionTokenType type);
}
