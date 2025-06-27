namespace CrossCutting.Utilities.QueryEvaluator.Abstractions;

public interface IQueryProcessor
{
    Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(Query query, CancellationToken cancellationToken)
        where TResult : class;
}
