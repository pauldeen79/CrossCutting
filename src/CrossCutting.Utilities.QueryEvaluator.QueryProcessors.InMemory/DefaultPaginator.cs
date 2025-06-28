namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory;

public class DefaultPaginator : IPaginator
{
    public IEnumerable<T> GetPagedData<T>(Query query, IEnumerable<T> filteredRecords)
        where T : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        IEnumerable<T> result = filteredRecords;

        if (query.OrderByFields.Count > 0)
        {
            result = result.OrderBy(x => new OrderByWrapper(x, query.OrderByFields));
        }

        if (query.Offset is not null)
        {
            result = result.Skip(query.Offset.Value);
        }
        if (query.Limit is not null)
        {
            result = result.Take(query.Limit.Value);
        }

        return result;
    }
}
