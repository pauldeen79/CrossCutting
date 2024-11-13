namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestEntityDatabaseCommandProvider(IEnumerable<IDatabaseEntityRetrieverSettingsProvider> settingsProviders) : SelectDatabaseCommandProvider(settingsProviders), IDatabaseCommandProvider<TestEntity>
{
    public IDatabaseCommand Create(TestEntity source, DatabaseOperation operation)
        => operation switch
        {
            DatabaseOperation.Insert => new SqlTextCommand("INSERT INTO...", DatabaseOperation.Insert),
            DatabaseOperation.Update => new SqlTextCommand("UPDATE...", DatabaseOperation.Update),
            DatabaseOperation.Delete => new SqlTextCommand("DELETE...", DatabaseOperation.Delete),
            _ => throw new ArgumentOutOfRangeException(nameof(operation), $"Unsupported operation: {operation}"),
        };
}
