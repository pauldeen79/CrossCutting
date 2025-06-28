namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory.Abstractions;

public interface IContextDataProvider : IDataProvider
{
    Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(Query query, object? context)
        where TResult : class;
}
