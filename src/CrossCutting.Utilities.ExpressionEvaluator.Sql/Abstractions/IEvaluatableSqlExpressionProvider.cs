namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Abstractions;

public interface IEvaluatableSqlExpressionProvider
{
    Task<Result<SqlExpression>> GetExpressionAsync(
        IEvaluatable evaluatable,
        IFieldNameProvider fieldNameProvider,
        object? context,
        CancellationToken token);
}
