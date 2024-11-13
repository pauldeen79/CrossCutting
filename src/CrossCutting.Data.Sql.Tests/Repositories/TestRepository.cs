namespace CrossCutting.Data.Sql.Tests.Repositories;

public interface ITestRepository : IRepository<TestEntity, TestEntityIdentity>
{
    TestEntity? FindOne();
    IReadOnlyCollection<TestEntity> FindMany(string value);
    IPagedResult<TestEntity> FindPaged(int offset, int pageSize);
}

public class TestRepository(IDatabaseCommandProcessor<TestEntity> commandProcessor,
                      IDatabaseEntityRetriever<TestEntity> entityRetriever,
                      IDatabaseCommandProvider<TestEntityIdentity> identitySelectCommandProvider,
                      IPagedDatabaseCommandProvider pagedEntitySelectCommandProvider,
                      IDatabaseCommandProvider entitySelectCommandProvider,
                      IDatabaseCommandProvider<TestEntity> entityCommandProvider) : Repository<TestEntity, TestEntityIdentity>(commandProcessor, entityRetriever, identitySelectCommandProvider, pagedEntitySelectCommandProvider, entitySelectCommandProvider, entityCommandProvider), ITestRepository
{

    // for test purposes only. normally you would add arguments here (request/query)
    public TestEntity? FindOne()
        => EntityRetriever.FindOne(new SelectCommandBuilder()
            .Select("*")
            .WithTop(1)
            .From("MyTable")
            .Where("Field = Value")
            .Build());

    // for test purposes only. normally you would add arguments here (request/query)
    public IReadOnlyCollection<TestEntity> FindMany(string value)
        => EntityRetriever.FindMany(new SelectCommandBuilder()
            .Select("*")
            .From("MyTable")
            .Where("Field = @Value")
            .AppendParameter("Value", value)
            .Build());

    // for test purposes only. normally you would add arguments here (request/query)
    public IPagedResult<TestEntity> FindPaged(int offset, int pageSize)
        => EntityRetriever.FindPaged(new PagedSelectCommandBuilder()
            .Select("*")
            .From("MyTable")
            .OrderBy("Name")
            .Skip(offset)
            .Take(pageSize)
            .Build());
}
