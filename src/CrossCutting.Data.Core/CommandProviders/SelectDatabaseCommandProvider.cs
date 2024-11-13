namespace CrossCutting.Data.Core.CommandProviders;

public class SelectDatabaseCommandProvider(IEnumerable<IDatabaseEntityRetrieverSettingsProvider> settingsProviders) : IDatabaseCommandProvider
{
    private readonly IEnumerable<IDatabaseEntityRetrieverSettingsProvider> _settingsProviders = settingsProviders;

    public IDatabaseCommand Create<TSource>(DatabaseOperation operation)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only Select operation is supported");
        }

        var settings = GetSettings<TSource>();
        return new SelectCommandBuilder()
            .Select(settings.Fields)
            .From(settings.TableName)
            .Where(settings.DefaultWhere)
            .OrderBy(settings.DefaultOrderBy)
            .Build();
    }

    private IDatabaseEntityRetrieverSettings GetSettings<TSource>()
    {
        foreach (var settingsProvider in _settingsProviders)
        {
            if (settingsProvider.TryGet<TSource>(out var settings) && settings is not null)
            {
                return settings;
            }
        }

        throw new InvalidOperationException($"Could not obtain database entity retriever settings for type [{typeof(TSource).FullName}]");
    }
}
