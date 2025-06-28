namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory.Abstractions;

public interface IDataProvider
{
    Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(Query query)
        where TResult : class;
}
