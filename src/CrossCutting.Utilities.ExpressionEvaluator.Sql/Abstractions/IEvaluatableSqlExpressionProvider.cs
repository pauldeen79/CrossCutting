namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Abstractions;

public interface IEvaluatableSqlExpressionProvider
{
    Task<Result<SqlExpressionData>> GetExpressionAsync(
        IEvaluatable<bool> condition,
        IFieldNameProvider fieldNameProvider,
        object? context,
        CancellationToken token);
}
