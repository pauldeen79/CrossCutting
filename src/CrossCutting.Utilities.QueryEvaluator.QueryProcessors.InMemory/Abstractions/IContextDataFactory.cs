namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory.Abstractions;

public interface IContextDataFactory : IDataFactory
{
    Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(Query query, object? context)
        where TResult : class;
}
