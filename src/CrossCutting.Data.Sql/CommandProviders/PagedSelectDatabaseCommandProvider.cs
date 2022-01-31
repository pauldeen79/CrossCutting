namespace CrossCutting.Data.Sql.CommandProviders;

public class PagedSelectDatabaseCommandProvider : IPagedDatabaseCommandProvider
{
    private IPagedDatabaseEntityRetrieverSettings Settings { get; }

    public PagedSelectDatabaseCommandProvider(IPagedDatabaseEntityRetrieverSettings settings)
    {
        Settings = settings;
    }

    public IPagedDatabaseCommand CreatePaged(DatabaseOperation operation, int offset, int pageSize)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only Select operation is supported");
        }

        return new PagedSelectCommandBuilder()
            .Select(Settings.Fields)
            .From(Settings.TableName)
            .Where(Settings.DefaultWhere)
            .OrderBy(Settings.DefaultOrderBy)
            .Skip(offset)
            .Take(((int?)pageSize).IfNotGreaterThan(Settings.OverridePageSize))
            .Build();
    }
}
