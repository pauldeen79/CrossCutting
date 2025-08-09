namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory.Abstractions;

public interface IPaginator
{
    Task<IEnumerable<T>> GetPagedDataAsync<T>(IQuery query, IEnumerable<T> filteredRecords, CancellationToken token)
        where T : class;
}
