namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory;

public class QueryProcessor : IQueryProcessor
{
    private readonly IPaginator _paginator;
    private readonly IDataFactory _dataFactory;

    public QueryProcessor(IPaginator paginator, IDataFactory dataFactory)
    {
        ArgumentGuard.IsNotNull(paginator, nameof(paginator));
        ArgumentGuard.IsNotNull(dataFactory, nameof(dataFactory));

        _paginator = paginator;
        _dataFactory = dataFactory;
    }

    public async Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(IQuery query, object? context, CancellationToken token)
        where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        var result = await GetDataAsync<TResult>(query, context).ConfigureAwait(false);
        if (!result.IsSuccessful())
        {
            return Result.FromExistingResult<IReadOnlyCollection<TResult>>(result);
        }

        return Result.Success<IReadOnlyCollection<TResult>>((await _paginator.GetPagedDataAsync
        (
            new SingleEntityQuery(null, null, query.Conditions, query.SortOrders),
            result.Value!,
            token).ConfigureAwait(false)
        ).ToList());
    }

    public async Task<Result<TResult>> FindOneAsync<TResult>(IQuery query, object? context, CancellationToken token)
        where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        var result = await GetDataAsync<TResult>(query, context).ConfigureAwait(false);
        if (!result.IsSuccessful())
        {
            return Result.FromExistingResult<TResult>(result);
        }

        var firstItem = (await _paginator.GetPagedDataAsync
        (
            new SingleEntityQuery(null, null, query.Conditions, query.SortOrders),
            result.Value!,
            token
        ).ConfigureAwait(false)).FirstOrDefault();

        return firstItem is null
            ? Result.NotFound<TResult>()
            : Result.Success(firstItem);
    }

    public async Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(IQuery query, object? context, CancellationToken token)
        where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        var result = await GetDataAsync<TResult>(query, context).ConfigureAwait(false);
        if (!result.IsSuccessful())
        {
            return Result.FromExistingResult<IPagedResult<TResult>>(result);
        }
        var filteredRecords = result.Value!.ToArray();

        return Result.Success<IPagedResult<TResult>>(new PagedResult<TResult>
        (
            await _paginator.GetPagedDataAsync(query, filteredRecords, token).ConfigureAwait(false),
            filteredRecords.Length,
            query.Offset.GetValueOrDefault(),
            query.Limit.GetValueOrDefault()
        ));
    }

    private async Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(IQuery query, object? context)
        where TResult : class
        => (await _dataFactory.GetDataAsync<TResult>(query, context).ConfigureAwait(false)).EnsureNotNull().EnsureValue();
}
