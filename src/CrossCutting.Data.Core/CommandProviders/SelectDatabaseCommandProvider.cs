namespace CrossCutting.Data.Core.CommandProviders;

public class SelectDatabaseCommandProvider(IEnumerable<IDatabaseEntityRetrieverSettingsProvider> settingsProviders) : IDatabaseCommandProvider
{
    private readonly IEnumerable<IDatabaseEntityRetrieverSettingsProvider> _settingsProviders = settingsProviders;

    public Result<IDatabaseCommand> Create<TSource>(DatabaseOperation operation)
    {
        if (operation != DatabaseOperation.Select)
        {
            return Result.Invalid<IDatabaseCommand>("Only Select operation is supported");
        }

        var settingsResult = GetSettings<TSource>().EnsureValue();
        if (!settingsResult.IsSuccessful())
        {
            return Result.FromExistingResult<IDatabaseCommand>(settingsResult);
        }

        var settings = settingsResult.Value!;
        return Result.Success(new SelectCommandBuilder()
            .Select(settings.Fields)
            .From(settings.TableName)
            .Where(settings.DefaultWhere)
            .OrderBy(settings.DefaultOrderBy)
            .Build());
    }

    private Result<IDatabaseEntityRetrieverSettings> GetSettings<TSource>()
    {
        foreach (var settingsProvider in _settingsProviders)
        {
            if (settingsProvider.TryGet<TSource>(out var settings) && settings is not null)
            {
                return Result.Success(settings);
            }
        }

        return Result.Error<IDatabaseEntityRetrieverSettings>($"Could not obtain database entity retriever settings for type [{typeof(TSource).FullName}]");
    }
}
