namespace CrossCutting.Utilities.QueryEvaluator.Abstractions;

public interface IQueryProcessor
{
    Task<Result<TResult>> FindOneAsync<TResult>(Query query, CancellationToken cancellationToken)
        where TResult : class;
    Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(Query query, CancellationToken cancellationToken)
        where TResult : class;
    Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(Query query, CancellationToken cancellationToken)
        where TResult : class;
}
