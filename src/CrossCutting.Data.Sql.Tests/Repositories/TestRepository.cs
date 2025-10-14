namespace CrossCutting.Data.Sql.Tests.Repositories;

public interface ITestRepository : IRepository<TestEntity, TestEntityIdentity>
{
    Task<Result<TestEntity>> FindOneAsync();
    Task<Result<IReadOnlyCollection<TestEntity>>> FindManyAsync(string value);
    Task<Result<IPagedResult<TestEntity>>> FindPagedAsync(int offset, int pageSize);
}

public class TestRepository(IDatabaseCommandProcessor<TestEntity> commandProcessor,
                            IDatabaseEntityRetriever<TestEntity> entityRetriever,
                            IDatabaseCommandProvider<TestEntityIdentity> identitySelectCommandProvider,
                            IPagedDatabaseCommandProvider pagedEntitySelectCommandProvider,
                            IDatabaseCommandProvider entitySelectCommandProvider,
                            IDatabaseCommandProvider<TestEntity> entityCommandProvider) : Repository<TestEntity, TestEntityIdentity>(commandProcessor, entityRetriever, identitySelectCommandProvider, pagedEntitySelectCommandProvider, entitySelectCommandProvider, entityCommandProvider), ITestRepository
{

    // for test purposes only. normally you would add arguments here (request/query)
    public Task<Result<TestEntity>> FindOneAsync()
        => EntityRetriever.FindOneAsync(new SelectCommandBuilder()
            .Select("*")
            .WithTop(1)
            .From("MyTable")
            .Where("Field = Value")
            .Build());

    // for test purposes only. normally you would add arguments here (request/query)
    public Task<Result<IReadOnlyCollection<TestEntity>>> FindManyAsync(string value)
        => EntityRetriever.FindManyAsync(new SelectCommandBuilder()
            .Select("*")
            .From("MyTable")
            .Where("Field = @Value")
            .AppendParameter("Value", value)
            .Build());

    // for test purposes only. normally you would add arguments here (request/query)
    public Task<Result<IPagedResult<TestEntity>>> FindPagedAsync(int offset, int pageSize)
        => EntityRetriever.FindPagedAsync(new PagedSelectCommandBuilder()
            .Select("*")
            .From("MyTable")
            .OrderBy("Name")
            .Skip(offset)
            .Take(pageSize)
            .Build());
}
