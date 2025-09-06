namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory.Abstractions;

public interface IDataProvider
{
    Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(IQuery query, object? context)
        where TResult : class;
}
