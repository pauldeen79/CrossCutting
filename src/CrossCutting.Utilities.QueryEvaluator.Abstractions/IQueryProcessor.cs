namespace CrossCutting.Utilities.QueryEvaluator.Abstractions;

public interface IQueryProcessor
{
    Task<Result<TResult>> FindOneAsync<TResult>(IQuery query, object? context, CancellationToken token)
        where TResult : class;
    Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(IQuery query, object? context, CancellationToken token)
        where TResult : class;
    Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(IQuery query, object? context, CancellationToken token)
        where TResult : class;
}
