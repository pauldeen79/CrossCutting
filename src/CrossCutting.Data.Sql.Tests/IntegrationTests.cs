namespace CrossCutting.Data.Sql.Tests;

public sealed class IntegrationTests : IDisposable
{
    private readonly TestRepository _repository;
    private readonly IDatabaseEntityMapper<TestEntity> _mapper;
    private readonly DbConnection _connection;

    public IntegrationTests()
    {
        var settings = new TestEntityDatabaseEntityRetrieverSettings();
        _connection = new DbConnection();
        _mapper = new TestEntityMapper();
        _repository = new TestRepository
        (
            new DatabaseCommandProcessor<TestEntity, TestEntityBuilder>(_connection, new TestEntityDatabaseCommandEntityProvider()),
            new DatabaseEntityRetriever<TestEntity>(_connection, _mapper),
            new TestEntityIdentityDatabaseCommandProvider(settings),
            new PagedSelectDatabaseCommandProvider(settings),
            new SelectDatabaseCommandProvider(settings),
            new TestEntityDatabaseCommandProvider()
        );
    }

    [Fact]
    public void Can_Add_Entity()
    {
        // Arrange
        var entity = new TestEntity("A", "B", "C", false);
        _connection.AddResultForDataReader(cmd => cmd.CommandText == "INSERT INTO...", new[] { new TestEntity("A", "B", "C", true) });

        // Act
        var actual = _repository.Add(entity);

        // Assert
        actual.Code.Should().Be(entity.Code);
        actual.CodeType.Should().Be(entity.CodeType);
        actual.Description.Should().Be(entity.Description);
        actual.IsExistingEntity.Should().BeTrue();
    }

    [Fact]
    public void Can_Update_Entity()
    {
        // Arrange
        var entity = new TestEntity("A", "B", "C", true);
        _connection.AddResultForDataReader(cmd => cmd.CommandText == "UPDATE...", new[] { new TestEntity("A1", "B1", "C1", true) });

        // Act
        var actual = _repository.Update(entity);

        // Assert
        actual.Code.Should().Be(entity.Code + "1");
        actual.CodeType.Should().Be(entity.CodeType + "1");
        actual.Description.Should().Be(entity.Description + "1");
        actual.IsExistingEntity.Should().BeTrue();
    }

    [Fact]
    public void Can_Delete_Entity()
    {
        // Arrange
        var entity = new TestEntity("A", "B", "C", true);
        _connection.AddResultForDataReader(cmd => cmd.CommandText == "DELETE...", new[] { new TestEntity("A1", "B1", "C1", true) }); //suffixes get ignored because Delete does not read result

        // Act
        var actual = _repository.Delete(entity);

        // Assert
        actual.Code.Should().Be(entity.Code);
        actual.CodeType.Should().Be(entity.CodeType);
        actual.Description.Should().Be(entity.Description);
        actual.IsExistingEntity.Should().BeTrue();
    }

    [Fact]
    public void Can_Find_Entity_By_Identity()
    {
        // Arrange
        var expectedResult = new TestEntity("A", "B", "C", true);
        var identity = new TestEntityIdentity(expectedResult);
        _connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT"), new[] { expectedResult });

        // Act
        var actual = _repository.Find(identity);

        // Assert
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Can_Find_All_Entities()
    {
        // Arrange
        var expectedResult = new[] { new TestEntity("A", "B", "C", true) };
        _connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT"), expectedResult);

        // Act
        var actual = _repository.FindAll();

        // Assert
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Can_Find_All_Entities_Paged()
    {
        // Arrange
        var expectedResult = new[] { new TestEntity("A", "B", "C", true) };
        _connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT"), expectedResult);
        _connection.AddResultForScalarCommand(cmd => cmd.CommandText.StartsWith("SELECT COUNT(*)"), 1);

        // Act
        var actual = _repository.FindAllPaged(0, 1);

        // Assert
        actual.Should().BeEquivalentTo(expectedResult);
    }

    public void Dispose() => _connection.Dispose();
}
