namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Abstractions;

public interface IEvaluatableSqlExpressionProvider
{
    Task<Result> GetExpressionAsync(SelectCommandBuilder selectCommandBuilder,
                                    object? context,
                                    IEvaluatable<bool> condition,
                                    IFieldNameProvider fieldNameProvider,
                                    CancellationToken token);
}
