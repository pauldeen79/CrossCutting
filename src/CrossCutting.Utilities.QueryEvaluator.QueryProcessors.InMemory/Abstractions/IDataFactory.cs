namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory.Abstractions;

public interface IDataFactory
{
    Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(Query query)
        where TResult : class;
}
