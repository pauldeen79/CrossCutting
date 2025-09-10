namespace CrossCutting.Data.Core.CommandProviders;

public abstract class IdentityDatabaseCommandProviderBase<T>(IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders) : IDatabaseCommandProvider<T>
    where T : class
{
    private readonly IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> _settingsProviders = settingsProviders;

    public Result<IDatabaseCommand> Create(T source, DatabaseOperation operation)
        => new ResultDictionaryBuilder()
            .Add("Validate", () => Result.Validate(() => operation == DatabaseOperation.Select, "Only select operation is supported"))
            .Add("Settings", GetSettings)
            .Build()
            .OnSuccess(results =>
            {
                var settings = results.GetValue<IPagedDatabaseEntityRetrieverSettings>("Settings");

                return new SelectCommandBuilder()
                    .Select(settings.Fields)
                    .From(settings.TableName)
                    .Where(string.Join(" AND ", GetFields().Select(x => $"[{x.FieldName}] = @{x.ParameterName}")))
                    .AppendParameters(source)
                    .Build();
            });

    private Result<IPagedDatabaseEntityRetrieverSettings> GetSettings()
        => _settingsProviders
            .Select(x => x.Get<T>().EnsureValue())
            .WhenNotContinue(() => Result.Error<IPagedDatabaseEntityRetrieverSettings>($"Could not obtain paged database entity retriever settings for type [{typeof(T).FullName}]"));

    protected abstract IEnumerable<IdentityDatabaseCommandProviderField> GetFields();
}
