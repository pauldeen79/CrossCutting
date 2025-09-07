namespace CrossCutting.Data.Sql.CommandProviders;

public class PagedSelectDatabaseCommandProvider(IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders) : IPagedDatabaseCommandProvider
{
    private readonly IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> _settingsProviders = settingsProviders;

    public Result<IPagedDatabaseCommand> CreatePaged<TSource>(DatabaseOperation operation, int offset, int pageSize)
    {
        if (operation != DatabaseOperation.Select)
        {
            return Result.Invalid<IPagedDatabaseCommand>("Only Select operation is supported");
        }

        var settingsResult = GetSettings<TSource>().EnsureValue();
        if (!settingsResult.IsSuccessful())
        {
            return Result.FromExistingResult<IPagedDatabaseCommand>(settingsResult);
        }

        var settings = settingsResult.Value!;
        return Result.Success(new PagedSelectCommandBuilder()
            .Select(settings.Fields)
            .From(settings.TableName)
            .Where(settings.DefaultWhere)
            .OrderBy(settings.DefaultOrderBy)
            .Skip(offset)
            .Take(((int?)pageSize).IfNotGreaterThan(settings.OverridePageSize))
            .Build());
    }

    private Result<IPagedDatabaseEntityRetrieverSettings> GetSettings<TSource>()
    {
        foreach (var settingsProvider in _settingsProviders)
        {
            if (settingsProvider.TryGet<TSource>(out var settings) && settings is not null)
            {
                return Result.Success(settings);
            }
        }

        return Result.Error<IPagedDatabaseEntityRetrieverSettings>($"Could not obtain paged database entity retriever settings for type [{typeof(TSource).FullName}]");
    }
}
