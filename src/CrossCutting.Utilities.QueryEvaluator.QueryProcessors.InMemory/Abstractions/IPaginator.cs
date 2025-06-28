namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory.Abstractions;

public interface IPaginator
{
    IEnumerable<T> GetPagedData<T>(Query query, IEnumerable<T> filteredRecords)
        where T : class;
}
