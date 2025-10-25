namespace CrossCutting.Data.Core.CommandProviders;

public class SelectDatabaseCommandProvider(IEnumerable<IDatabaseEntityRetrieverSettingsProvider> settingsProviders) : IDatabaseCommandProvider
{
    private readonly IEnumerable<IDatabaseEntityRetrieverSettingsProvider> _settingsProviders = settingsProviders;

    public async Task<Result<IDatabaseCommand>> CreateAsync<TSource>(DatabaseOperation operation)
        => (await new AsyncResultDictionaryBuilder()
            .Add(() => Result.Validate(() => operation == DatabaseOperation.Select, "Only Select operation is supported"))
            .Add("Settings", GetSettings<TSource>)
            .BuildAsync().ConfigureAwait(false))
            .OnSuccess(results =>
            {
                var settings = results.GetValue<IDatabaseEntityRetrieverSettings>("Settings");
                
                return new SelectCommandBuilder()
                    .Select(settings.Fields)
                    .From(settings.TableName)
                    .Where(settings.DefaultWhere)
                    .OrderBy(settings.DefaultOrderBy)
                    .Build();
            });

    private Result<IDatabaseEntityRetrieverSettings> GetSettings<TSource>()
        => _settingsProviders
            .Select(x => x.Get<TSource>().EnsureNotNull().EnsureValue())
            .WhenNotContinue(() => Result.Error<IDatabaseEntityRetrieverSettings>($"Could not obtain database entity retriever settings for type [{typeof(TSource).FullName}]"));
}
