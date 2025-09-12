namespace CrossCutting.Data.Sql.Tests;

public sealed class IntegrationTests : IDisposable
{
    private readonly ITestRepository _repository;
    private readonly DbConnection _connection;
    private readonly ServiceProvider _serviceProvider;
    private readonly IServiceScope _scope;

    public IntegrationTests()
    {
        _connection = new DbConnection();
        _serviceProvider = new ServiceCollection()
            .AddCrossCuttingDataSql()
            .AddSingleton<IDatabaseCommandProvider<TestEntity>, TestEntityDatabaseCommandProvider>()
            .AddSingleton<IDatabaseCommandProvider<TestEntityIdentity>, TestEntityIdentityDatabaseCommandProvider>()
            .AddSingleton<IDatabaseCommandEntityProvider<TestEntity, TestEntityBuilder>, TestEntityDatabaseCommandEntityProvider>()
            .AddSingleton<IDatabaseEntityRetrieverSettingsProvider, TestEntityDatabaseEntityRetrieverSettingsProvider>()
            .AddSingleton<IPagedDatabaseEntityRetrieverSettingsProvider, TestEntityDatabaseEntityRetrieverSettingsProvider>()
            .AddSingleton<IDatabaseEntityMapper<TestEntity>, TestEntityMapper>()
            .AddScoped<System.Data.Common.DbConnection>(_ => _connection)
            .AddScoped<IDatabaseCommandProcessor<TestEntity>, DatabaseCommandProcessor<TestEntity, TestEntityBuilder>>()
            .AddScoped<IDatabaseEntityRetriever<TestEntity>, DatabaseEntityRetriever<TestEntity>>()
            .AddScoped<ITestRepository, TestRepository>()
            .BuildServiceProvider(true);
        _scope = _serviceProvider.CreateScope();
        _repository = _scope.ServiceProvider.GetRequiredService<ITestRepository>();
    }

    [Fact]
    public void Can_Add_Entity()
    {
        // Arrange
        var entity = new TestEntity("A", "B", "C", false);
        _connection.AddResultForDataReader(cmd => cmd.CommandText == "INSERT INTO...",
                                           new[] { new TestEntity("A", "B", "C", true) });

        // Act
        var actual = _repository.Add(entity).EnsureValue().GetValueOrThrow();

        // Assert
        actual.Code.ShouldBe(entity.Code);
        actual.CodeType.ShouldBe(entity.CodeType);
        actual.Description.ShouldBe(entity.Description);
        actual.IsExistingEntity.ShouldBeTrue();
    }

    [Fact]
    public void Can_Update_Entity()
    {
        // Arrange
        var entity = new TestEntity("A", "B", "C", true);
        _connection.AddResultForDataReader(cmd => cmd.CommandText == "UPDATE...",
                                           new[] { new TestEntity("A1", "B1", "C1", true) });

        // Act
        var actual = _repository.Update(entity).EnsureValue().GetValueOrThrow();

        // Assert
        actual.Code.ShouldBe(entity.Code + "1");
        actual.CodeType.ShouldBe(entity.CodeType + "1");
        actual.Description.ShouldBe(entity.Description + "1");
        actual.IsExistingEntity.ShouldBeTrue();
    }

    [Fact]
    public void Can_Delete_Entity()
    {
        // Arrange
        var entity = new TestEntity("A", "B", "C", true);
        _connection.AddResultForDataReader(cmd => cmd.CommandText == "DELETE...",
                                           new[] { new TestEntity("A1", "B1", "C1", true) }); //suffixes get ignored because Delete does not read result

        // Act
        var actual = _repository.Delete(entity).EnsureValue().GetValueOrThrow();

        // Assert
        actual.Code.ShouldBe(entity.Code);
        actual.CodeType.ShouldBe(entity.CodeType);
        actual.Description.ShouldBe(entity.Description);
        actual.IsExistingEntity.ShouldBeTrue();
    }

    [Fact]
    public void Can_Find_Entity_By_Identity()
    {
        // Arrange
        var expectedResult = new TestEntity("A", "B", "C", true);
        var identity = new TestEntityIdentity(expectedResult);
        _connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT"),
                                           new[] { expectedResult });

        // Act
        var actual = _repository.Find(identity).EnsureValue().GetValueOrThrow();

        // Assert
        actual.ShouldBeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Can_Find_All_Entities()
    {
        // Arrange
        var expectedResult = new[] { new TestEntity("A", "B", "C", true) };
        _connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT"), expectedResult);

        // Act
        var actual = _repository.FindAll().EnsureValue().GetValueOrThrow();

        // Assert
        actual.ToArray().ShouldBeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Can_Find_All_Entities_Paged()
    {
        // Arrange
        var expectedResult = new[] { new TestEntity("A", "B", "C", true) };
        _connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT"), expectedResult);
        _connection.AddResultForScalarCommand(cmd => cmd.CommandText.StartsWith("SELECT COUNT(*)"), 1);

        // Act
        var actual = _repository.FindAllPaged(0, 1).EnsureValue().GetValueOrThrow();

        // Assert
        actual.ToArray().ShouldBeEquivalentTo(expectedResult);
    }

    public void Dispose()
    {
        _scope.Dispose();
        _serviceProvider.Dispose();
        _connection.Dispose();
    }
}
