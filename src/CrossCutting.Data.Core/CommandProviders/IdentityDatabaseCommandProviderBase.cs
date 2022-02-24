namespace CrossCutting.Data.Core.CommandProviders;

public abstract class IdentityDatabaseCommandProviderBase<T> : IDatabaseCommandProvider<T>
    where T : class
{
    private readonly IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> _settingsProviders;

    protected IdentityDatabaseCommandProviderBase(IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders)
        => _settingsProviders = settingsProviders;

    public IDatabaseCommand Create(T source, DatabaseOperation operation)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only select is supported");
        }
        var settings = GetSettings();
        return new SelectCommandBuilder()
            .Select(settings.Fields)
            .From(settings.TableName)
            .Where(string.Join(" AND ", GetFields().Select(x => $"[{x.FieldName}] = @{x.ParameterName}")))
            .AppendParameters(source)
            .Build();
    }

    private IPagedDatabaseEntityRetrieverSettings GetSettings()
    {
        foreach (var settingsProvider in _settingsProviders)
        {
            if (settingsProvider.TryGet<T>(out var settings) && settings != null)
            {
                return settings;
            }
        }

        throw new InvalidOperationException($"Could not obtain paged database entity retriever settings for type [{typeof(T).FullName}]");
    }

    protected abstract IEnumerable<IdentityDatabaseCommandProviderField> GetFields();
}
