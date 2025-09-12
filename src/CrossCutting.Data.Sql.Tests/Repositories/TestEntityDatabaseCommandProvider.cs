namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestEntityDatabaseCommandProvider(IEnumerable<IDatabaseEntityRetrieverSettingsProvider> settingsProviders) : SelectDatabaseCommandProvider(settingsProviders), IDatabaseCommandProvider<TestEntity>
{
    public Result<IDatabaseCommand> Create(TestEntity source, DatabaseOperation operation)
        => operation switch
        {
            DatabaseOperation.Insert => Result.Success<IDatabaseCommand>(new SqlTextCommand("INSERT INTO...", DatabaseOperation.Insert)),
            DatabaseOperation.Update => Result.Success<IDatabaseCommand>(new SqlTextCommand("UPDATE...", DatabaseOperation.Update)),
            DatabaseOperation.Delete => Result.Success<IDatabaseCommand>(new SqlTextCommand("DELETE...", DatabaseOperation.Delete)),
            _ => Result.Invalid<IDatabaseCommand>($"Unsupported operation: {operation}")
        };
}
