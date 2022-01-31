namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestEntityDatabaseCommandProvider : SelectDatabaseCommandProvider, IDatabaseCommandProvider<TestEntity>
{
    public TestEntityDatabaseCommandProvider() : base(new TestEntityDatabaseEntityRetrieverSettings())
    {
    }

    public IDatabaseCommand Create(TestEntity source, DatabaseOperation operation)
        => operation switch
        {
            DatabaseOperation.Insert => new SqlTextCommand("INSERT INTO...", DatabaseOperation.Insert),
            DatabaseOperation.Update => new SqlTextCommand("UPDATE...", DatabaseOperation.Update),
            DatabaseOperation.Delete => new SqlTextCommand("DELETE...", DatabaseOperation.Delete),
            _ => throw new ArgumentOutOfRangeException(nameof(operation), $"Unsupported operation: {operation}"),
        };
}
