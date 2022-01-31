namespace CrossCutting.Data.Core.CommandProviders;

public class SelectDatabaseCommandProvider : IDatabaseCommandProvider
{
    private IDatabaseEntityRetrieverSettings Settings { get; }

    public SelectDatabaseCommandProvider(IDatabaseEntityRetrieverSettings settings)
    {
        Settings = settings;
    }

    public IDatabaseCommand Create(DatabaseOperation operation)
    {
        if (operation != DatabaseOperation.Select)
        {
            throw new ArgumentOutOfRangeException(nameof(operation), "Only Select operation is supported");
        }

        return new SelectCommandBuilder()
            .Select(Settings.Fields)
            .From(Settings.TableName)
            .Where(Settings.DefaultWhere)
            .OrderBy(Settings.DefaultOrderBy)
            .Build();
    }
}
