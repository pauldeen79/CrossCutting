namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class QueryDatabaseCommandProvider : IDatabaseCommandProvider<IQuery>
{
    private readonly IPagedDatabaseCommandProvider<IQueryContext> _pagedDatabaseCommandProvider;

    public QueryDatabaseCommandProvider(IPagedDatabaseCommandProvider<IQueryContext> pagedDatabaseCommandProvider)
    {
        ArgumentGuard.IsNotNull(pagedDatabaseCommandProvider, nameof(pagedDatabaseCommandProvider));

        _pagedDatabaseCommandProvider = pagedDatabaseCommandProvider;
    }

    public async Task<Result<IDatabaseCommand>> CreateAsync(IQuery source, DatabaseOperation operation, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(() => Result.Validate(() => operation == DatabaseOperation.Select, "Only select operation is supported"))
            .Add("Command", () => _pagedDatabaseCommandProvider.CreatePagedAsync(source.WithContext(null), operation, 0, 0, token))
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccess(results => results.GetValue<IPagedDatabaseCommand>("Command").DataCommand);
}
