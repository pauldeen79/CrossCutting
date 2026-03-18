namespace CrossCutting.Utilities.ExpressionEvaluator.Sql;

public sealed class SqlExpressionData
{
    public string Expression { get; }
    public IReadOnlyCollection<KeyValuePair<string, object?>> Parameters { get; }

    public SqlExpressionData(string expression, IReadOnlyCollection<KeyValuePair<string, object?>>? parameters = null)
    {
        ArgumentGuard.IsNotNull(expression, nameof(expression));

        Expression = expression;
        Parameters = parameters ?? new ReadOnlyCollection<KeyValuePair<string, object?>>([]);
    }
}
