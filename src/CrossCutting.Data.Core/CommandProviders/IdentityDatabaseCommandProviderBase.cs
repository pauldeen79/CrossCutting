namespace CrossCutting.Data.Core.CommandProviders;

public abstract class IdentityDatabaseCommandProviderBase<T>(IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders) : IDatabaseCommandProvider<T>
    where T : class
{
    private readonly IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> _settingsProviders = settingsProviders;

    public Result<IDatabaseCommand> Create(T source, DatabaseOperation operation)
    {
        if (operation != DatabaseOperation.Select)
        {
            return Result.Invalid<IDatabaseCommand>("Only select operation is supported");
        }

        var settingsResult = GetSettings().EnsureValue();
        if (!settingsResult.IsSuccessful())
        {
            return Result.FromExistingResult<IDatabaseCommand>(settingsResult);
        }

        var settings = settingsResult.Value!;
        return Result.Success(new SelectCommandBuilder()
            .Select(settings.Fields)
            .From(settings.TableName)
            .Where(string.Join(" AND ", GetFields().Select(x => $"[{x.FieldName}] = @{x.ParameterName}")))
            .AppendParameters(source)
            .Build());
    }

    private Result<IPagedDatabaseEntityRetrieverSettings> GetSettings()
    {
        foreach (var settingsProvider in _settingsProviders)
        {
            if (settingsProvider.TryGet<T>(out var settings) && settings is not null)
            {
                return Result.Success(settings);
            }
        }

        return Result.Error<IPagedDatabaseEntityRetrieverSettings>($"Could not obtain paged database entity retriever settings for type [{typeof(T).FullName}]");
    }

    protected abstract IEnumerable<IdentityDatabaseCommandProviderField> GetFields();
}
