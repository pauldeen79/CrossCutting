namespace CrossCutting.Data.Core.CommandProviders;

public class SelectDatabaseCommandProvider(IEnumerable<IDatabaseEntityRetrieverSettingsProvider> settingsProviders) : IDatabaseCommandProvider
{
    private readonly IDatabaseEntityRetrieverSettingsProvider[] _settingsProviders = ArgumentGuard.IsNotNull(settingsProviders, nameof(settingsProviders)).ToArray();

    public async Task<Result<IDatabaseCommand>> CreateAsync<TSource>(DatabaseOperation operation, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(() => Result.Validate(() => operation == DatabaseOperation.Select, "Only Select operation is supported"))
            .Add("Settings", GetSettings<TSource>)
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccess(results =>
            {
                var settings = results.GetValue<IDatabaseEntityRetrieverSettings>("Settings");
                
                return new SelectCommandBuilder()
                    .Select(settings.Fields)
                    .From(settings.TableName.FormatAsDatabaseIdentifier())
                    .Where(settings.DefaultWhere)
                    .OrderBy(settings.DefaultOrderBy.FormatAsDatabaseIdentifier())
                    .Build();
            });

    private Result<IDatabaseEntityRetrieverSettings> GetSettings<TSource>()
        => _settingsProviders
            .Select(x => x.Get<TSource>().EnsureValue())
            .WhenNotContinue(() => Result.Error<IDatabaseEntityRetrieverSettings>($"Could not obtain database entity retriever settings for type [{typeof(TSource).FullName}]"));
}
