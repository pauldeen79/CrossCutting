namespace CrossCutting.Data.Sql.CommandProviders;

public class PagedSelectDatabaseCommandProvider(IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders) : IPagedDatabaseCommandProvider
{
    private readonly IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> _settingsProviders = settingsProviders;

    public async Task<Result<IPagedDatabaseCommand>> CreatePagedAsync<TSource>(DatabaseOperation operation, int offset, int pageSize, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(() => Result.Validate(() => operation == DatabaseOperation.Select, "Only Select operation is supported"))
            .Add("Settings", GetSettings<TSource>)
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccess(results =>
            {
                var settings = results.GetValue<IPagedDatabaseEntityRetrieverSettings>("Settings");

                return new PagedSelectCommandBuilder()
                    .Select(settings.Fields)
                    .From(settings.TableName)
                    .Where(settings.DefaultWhere)
                    .OrderBy(settings.DefaultOrderBy)
                    .Skip(offset)
                    .Take(((int?)pageSize).IfNotGreaterThan(settings.OverridePageSize))
                    .Build();
            });

    private Result<IPagedDatabaseEntityRetrieverSettings> GetSettings<TSource>()
        => _settingsProviders
            .Select(x => x.Get<TSource>().EnsureValue())
            .WhenNotContinue(() => Result.Error<IPagedDatabaseEntityRetrieverSettings>($"Could not obtain paged database entity retriever settings for type [{typeof(TSource).FullName}]"));
}
