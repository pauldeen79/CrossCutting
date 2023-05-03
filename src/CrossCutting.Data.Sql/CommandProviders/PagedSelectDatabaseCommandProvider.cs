namespace CrossCutting.Data.Sql.CommandProviders;

public class PagedSelectDatabaseCommandProvider : IPagedDatabaseCommandProvider
{
    private readonly IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> _settingsProviders;

    public PagedSelectDatabaseCommandProvider(IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders)
        => _settingsProviders = settingsProviders;

    public IPagedDatabaseCommand CreatePaged<TSource>(DatabaseOperation operation, int offset, int pageSize)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only Select operation is supported");
        }

        var settings = GetSettings<TSource>();
        return new PagedSelectCommandBuilder()
            .Select(settings.Fields)
            .From(settings.TableName)
            .Where(settings.DefaultWhere)
            .OrderBy(settings.DefaultOrderBy)
            .Skip(offset)
            .Take(((int?)pageSize).IfNotGreaterThan(settings.OverridePageSize))
            .Build();
    }

    private IPagedDatabaseEntityRetrieverSettings GetSettings<TSource>()
    {
        foreach (var settingsProvider in _settingsProviders)
        {
            if (settingsProvider.TryGet<TSource>(out var settings) && settings is not null)
            {
                return settings;
            }
        }

        throw new InvalidOperationException($"Could not obtain paged database entity retriever settings for type [{typeof(TSource).FullName}]");
    }
}
