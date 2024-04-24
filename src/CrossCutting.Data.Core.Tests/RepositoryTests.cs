namespace CrossCutting.Data.Core.Tests;

public class RepositoryTests : TestBase<Repository<TestEntity, TestEntityIdentity>>
{
    private IDatabaseCommandProcessor<TestEntity> CommandProcessorMock => Fixture.Freeze<IDatabaseCommandProcessor<TestEntity>>();
    private IDatabaseEntityRetriever<TestEntity> EntityRetrieverMock => Fixture.Freeze<IDatabaseEntityRetriever<TestEntity>>();
    private IDatabaseCommandProvider<TestEntityIdentity> IdentitySelectCommandProviderMock => Fixture.Freeze<IDatabaseCommandProvider<TestEntityIdentity>>();
    private IDatabaseCommandProvider EntitySelectCommandProviderMock => Fixture.Freeze<IDatabaseCommandProvider>();
    private IPagedDatabaseCommandProvider PagedEntitySelectCommandProviderMock => Fixture.Freeze<IPagedDatabaseCommandProvider>();
    private IDatabaseCommandProvider<TestEntity> EntityCommandProviderMock => Fixture.Freeze<IDatabaseCommandProvider<TestEntity>>();

    [Fact]
    public void Add_Adds_Entity_To_Database()
    {
        // Arrange
        var commandMock = Substitute.For<IDatabaseCommand>();
        commandMock.Operation.Returns(DatabaseOperation.Insert);
        var entity = new TestEntity("01", "Test", "first entity", false);
        CommandProcessorMock.ExecuteCommand(commandMock, entity).Returns(new DatabaseCommandResult<TestEntity>(entity));
        EntityCommandProviderMock.Create(Arg.Any<TestEntity>(), DatabaseOperation.Insert).Returns(commandMock);

        // Act
        var result = Sut.Add(entity);

        // Assert
        result.Should().Be(entity);
        CommandProcessorMock.Received().ExecuteCommand(commandMock, entity);
    }

    [Fact]
    public async Task AddAsync_Adds_Entity_To_Database()
    {
        // Arrange
        var commandMock = Substitute.For<IDatabaseCommand>();
        commandMock.Operation.Returns(DatabaseOperation.Insert);
        var entity = new TestEntity("01", "Test", "first entity", false);
        CommandProcessorMock.ExecuteCommandAsync(commandMock, entity).Returns(new DatabaseCommandResult<TestEntity>(entity));
        EntityCommandProviderMock.Create(Arg.Any<TestEntity>(), DatabaseOperation.Insert).Returns(commandMock);

        // Act
        var result = await Sut.AddAsync(entity);

        // Assert
        result.Should().Be(entity);
        await CommandProcessorMock.Received().ExecuteCommandAsync(commandMock, entity);
    }

    [Fact]
    public void Can_Update_Entity_To_Repository()
    {
        // Arrange
        var entity = new TestEntity("01", "Test", "first entity", true);
        var commandMock = Substitute.For<IDatabaseCommand>();
        commandMock.Operation.Returns(DatabaseOperation.Update);
        CommandProcessorMock.ExecuteCommand(commandMock, entity).Returns(new DatabaseCommandResult<TestEntity>(entity));
        EntityCommandProviderMock.Create(Arg.Any<TestEntity>(), DatabaseOperation.Update).Returns(commandMock);

        // Act
        var result = Sut.Update(entity);

        // Assert
        result.Should().Be(entity);
        CommandProcessorMock.Received().ExecuteCommand(commandMock, entity);
    }

    [Fact]
    public async Task Can_Update_Entity_To_Repository_Async()
    {
        // Arrange
        var entity = new TestEntity("01", "Test", "first entity", true);
        var commandMock = Substitute.For<IDatabaseCommand>();
        commandMock.Operation.Returns(DatabaseOperation.Update);
        CommandProcessorMock.ExecuteCommandAsync(commandMock, entity).Returns(new DatabaseCommandResult<TestEntity>(entity));
        EntityCommandProviderMock.Create(Arg.Any<TestEntity>(), DatabaseOperation.Update).Returns(commandMock);

        // Act
        var result = await Sut.UpdateAsync(entity);

        // Assert
        result.Should().Be(entity);
        await CommandProcessorMock.Received().ExecuteCommandAsync(commandMock, entity);
    }

    [Fact]
    public void Can_Delete_Entity_To_Repository()
    {
        // Arrange
        var entity = new TestEntity("01", "Test", "first entity", true);
        var commandMock = Fixture.Freeze<IDatabaseCommand>();
        commandMock.Operation.Returns(DatabaseOperation.Delete);
        CommandProcessorMock.ExecuteCommand(commandMock, entity).Returns(new DatabaseCommandResult<TestEntity>(entity));
        EntityCommandProviderMock.Create(Arg.Any<TestEntity>(), DatabaseOperation.Delete).Returns(commandMock);

        // Act
        var result = Sut.Delete(entity);

        // Assert
        result.Should().Be(entity);
        CommandProcessorMock.Received().ExecuteCommand(commandMock, entity);
    }

    [Fact]
    public async Task Can_Delete_Entity_To_Repository_Async()
    {
        // Arrange
        var entity = new TestEntity("01", "Test", "first entity", true);
        var commandMock = Fixture.Freeze<IDatabaseCommand>();
        commandMock.Operation.Returns(DatabaseOperation.Delete);
        CommandProcessorMock.ExecuteCommandAsync(commandMock, entity).Returns(new DatabaseCommandResult<TestEntity>(entity));
        EntityCommandProviderMock.Create(Arg.Any<TestEntity>(), DatabaseOperation.Delete).Returns(commandMock);

        // Act
        var result = await Sut.DeleteAsync(entity);

        // Assert
        result.Should().Be(entity);
        await CommandProcessorMock.Received().ExecuteCommandAsync(commandMock, entity);
    }

    [Fact]
    public void Can_Find_Entity_Using_Identity()
    {
        // Arrange
        var expected = new TestEntity("code", "codeType", "description");
        IdentitySelectCommandProviderMock.Create(Arg.Any<TestEntityIdentity>(), DatabaseOperation.Select).Returns(new SqlTextCommand("SELECT ...", DatabaseOperation.Select));
        EntityRetrieverMock.FindOne(Arg.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select)).Returns(expected);

        // Act
        var actual = Sut.Find(new TestEntityIdentity(expected));

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public async Task Can_Find_Entity_Using_Identity_Async()
    {
        // Arrange
        var expected = new TestEntity("code", "codeType", "description");
        IdentitySelectCommandProviderMock.Create(Arg.Any<TestEntityIdentity>(), DatabaseOperation.Select).Returns(new SqlTextCommand("SELECT ...", DatabaseOperation.Select));
        EntityRetrieverMock.FindOneAsync(Arg.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select)).Returns(expected);

        // Act
        var actual = await Sut.FindAsync(new TestEntityIdentity(expected));

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void Can_FindAll()
    {
        // Arrange
        var expected = new[] { new TestEntity("code", "codeType", "description") };
        EntitySelectCommandProviderMock.Create<TestEntity>(DatabaseOperation.Select).Returns(new SqlTextCommand("SELECT ...", DatabaseOperation.Select));
        EntityRetrieverMock.FindMany(Arg.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select)).Returns(expected);

        // Act
        var actual = Sut.FindAll();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Can_FindAll_Async()
    {
        // Arrange
        var expected = new[] { new TestEntity("code", "codeType", "description") };
        EntitySelectCommandProviderMock.Create<TestEntity>(DatabaseOperation.Select).Returns(new SqlTextCommand("SELECT ...", DatabaseOperation.Select));
        EntityRetrieverMock.FindManyAsync(Arg.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select)).Returns(expected);

        // Act
        var actual = await Sut.FindAllAsync();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Can_FindAll_Paged()
    {
        // Arrange
        var expected = new PagedResult<TestEntity>([new TestEntity("code", "codeType", "description")], 10, 1, 10);
        PagedEntitySelectCommandProviderMock.CreatePaged<TestEntity>(DatabaseOperation.Select, 1, 10).Returns(new PagedDatabaseCommand(new SqlTextCommand("SELECT ...", DatabaseOperation.Select), new SqlTextCommand("SELECT COUNT(*) FROM...", DatabaseOperation.Unspecified), 1, 10));
        EntityRetrieverMock.FindPaged(Arg.Is<IPagedDatabaseCommand>(x => x.DataCommand.Operation == DatabaseOperation.Select)).Returns(expected);

        // Act
        var actual = Sut.FindAllPaged(1, 10);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Can_FindAll_Paged_Async()
    {
        // Arrange
        var expected = new PagedResult<TestEntity>([new TestEntity("code", "codeType", "description")], 10, 1, 10);
        PagedEntitySelectCommandProviderMock.CreatePaged<TestEntity>(DatabaseOperation.Select, 1, 10).Returns(new PagedDatabaseCommand(new SqlTextCommand("SELECT ...", DatabaseOperation.Select), new SqlTextCommand("SELECT COUNT(*) FROM...", DatabaseOperation.Unspecified), 1, 10));
        EntityRetrieverMock.FindPagedAsync(Arg.Is<IPagedDatabaseCommand>(x => x.DataCommand.Operation == DatabaseOperation.Select)).Returns(expected);

        // Act
        var actual = await Sut.FindAllPagedAsync(1, 10);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
