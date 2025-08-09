namespace CrossCutting.Utilities.QueryEvaluator.Abstractions;

public interface IQueryProcessor
{
    Task<Result<TResult>> FindOneAsync<TResult>(IQuery query, CancellationToken cancellationToken)
        where TResult : class;
    Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(IQuery query, CancellationToken cancellationToken)
        where TResult : class;
    Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(IQuery query, CancellationToken cancellationToken)
        where TResult : class;
}
