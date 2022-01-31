namespace CrossCutting.Data.Sql.Tests;

public sealed class DatabaseCommandProcessorTests : IDisposable
{
    private DatabaseCommandProcessor<MyEntity> Sut { get; }
    private DbConnection Connection { get; }
    private Mock<IDatabaseCommandEntityProvider<MyEntity>> ProviderMock { get; }

    public DatabaseCommandProcessorTests()
    {
        Connection = new DbConnection();
        ProviderMock = new Mock<IDatabaseCommandEntityProvider<MyEntity>>();
        Sut = new DatabaseCommandProcessor<MyEntity>(Connection, ProviderMock.Object);
    }

    [Fact]
    public void ExecuteScalar_Returns_Correct_Value_When_Command_Is_Not_Null()
    {
        // Arrange
        Connection.AddResultForScalarCommand(12345);
        var command = new SqlDatabaseCommand("Select 12345", DatabaseCommandType.Text, DatabaseOperation.Unspecified);

        // Act
        var actual = Sut.ExecuteScalar(command);

        // Assert
        actual.Should().Be(12345);
    }

    [Fact]
    public void ExecuteNonQuery_Returns_Correct_Value_When_Command_Is_Not_Null()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(12345);
        var command = new SqlDatabaseCommand("Select 12345", DatabaseCommandType.Text, DatabaseOperation.Unspecified);

        // Act
        var actual = Sut.ExecuteNonQuery(command);

        // Assert
        actual.Should().Be(12345);
    }

    [Fact]
    public void InvokeCommand_Throws_When_ResultEntityDelegate_Returns_Null()
    {
        // Arrange
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
#pragma warning disable CS8603 // Possible null reference return.
        ProviderMock.SetupGet(x => x.ResultEntityDelegate).Returns((_, _) => null);
#pragma warning restore CS8603 // Possible null reference return.

        // Act
        Sut.Invoking(x => x.ExecuteCommand(command, new MyEntity { Property = "filled" }))
           .Should().Throw<InvalidOperationException>()
           .WithMessage("Instance should be supplied, or result entity delegate should deliver an instance");
    }

    [Fact]
    public void InvokeCommand_Does_Not_Throw_When_OperationValidationDelegate_Returns_True()
    {
        // Arrange
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);

        // Act
        Sut.Invoking(x => x.ExecuteCommand(command, new MyEntity { Property = "filled" }))
           .Should().NotThrow<InvalidOperationException>();
    }

    [Fact]
    public void InvokeCommand_Throws_When_Instance_Validation_Fails()
    {
        // Arrange
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);

        // Act
        Sut.Invoking(x => x.ExecuteCommand(command, new MyEntity { Property = null }))
           .Should().Throw<ValidationException>()
           .WithMessage("The Property field is required.");
    }

    [Fact]
    public void InvokeCommand_No_AfterReadDelegate_Throws_When_ExecuteNonQuery_Returns_0()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(0); // 0 rows affected
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);

        // Act
        Sut.Invoking(x => x.ExecuteCommand(command, new MyEntity { Property = "test" }).HandleResult("MyEntity entity was not added"))
           .Should().Throw<DataException>()
           .WithMessage("MyEntity entity was not added");
    }

    [Fact]
    public void InvokeCommand_No_AfterReadDelegate_Does_Not_Throw_When_ExecuteNonQuery_Returns_1()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(1); // 1 row affected
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);

        // Act
        Sut.Invoking(x => x.ExecuteCommand(command, new MyEntity { Property = "test" }))
           .Should().NotThrow<DataException>();
    }

    [Fact]
    public void InvokeCommand_AfterReadDelegate_Throws_When_ExecuteReader_Read_Returns_False()
    {
        // Arrange
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.SetupGet(x => x.AfterReadDelegate).Returns(new Func<MyEntity, DatabaseOperation, IDataReader, MyEntity>((x, _, _) => x));

        // Act
        Sut.Invoking(x => x.ExecuteCommand(command, new MyEntity { Property = "test" }).HandleResult("MyEntity entity was not added"))
           .Should().Throw<DataException>()
           .WithMessage("MyEntity entity was not added");
    }

    [Fact]
    public void InvokeCommand_AfterReadDelegate_Does_Not_Throw_When_ExecuteReader_Read_Returns_True()
    {
        // Arrange
        Connection.AddResultForDataReader(new[] { new MyEntity { Property = "test" } });
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.SetupGet(x => x.AfterReadDelegate).Returns(new Func<MyEntity, DatabaseOperation, IDataReader, MyEntity>((x, _, _) => x));

        // Act
        var actual = Sut.ExecuteCommand(command, new MyEntity { Property = "test" }).HandleResult("MyEntity entity was not added");

        // Assert
        actual.Property.Should().Be("test");
    }

    [Fact]
    public void InvokeCommand_Builder_To_Entity_Conversion_Throws_When_Builder_Could_Not_Be_Constructed()
    {
        // Arrange
        var builderProviderMock = new Mock<IDatabaseCommandEntityProvider<TestEntity, TestEntityBuilder>>();
        var builderSut = new DatabaseCommandProcessor<TestEntity, TestEntityBuilder>(Connection, builderProviderMock.Object);
        var entity = new TestEntity("A", "B", "C", true);

        // Act & Assert
        builderSut.Invoking(x => x.ExecuteCommand(new Mock<IDatabaseCommand>().Object, entity))
                  .Should().Throw<InvalidOperationException>()
                  .WithMessage("Builder instance was not constructed, create builder delegate should deliver an instance");
    }

    [Fact]
    public void InvokeCommand_Entity_To_Builder_Conversion_Throws_When_Entity_Could_Not_Be_Constructed()
    {
        // Arrange
        var builderProviderMock = new Mock<IDatabaseCommandEntityProvider<TestEntity, TestEntityBuilder>>();
        builderProviderMock.SetupGet(x => x.CreateBuilderDelegate)
                           .Returns(new Func<TestEntity, TestEntityBuilder>(entity => new TestEntityBuilder(entity)));
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        var builderSut = new DatabaseCommandProcessor<TestEntity, TestEntityBuilder>(Connection, builderProviderMock.Object);
        var entity = new TestEntity("A", "B", "C", true);

        // Act & Assert
        builderSut.Invoking(x => x.ExecuteCommand(command, entity))
                  .Should().Throw<InvalidOperationException>()
                  .WithMessage("Could not cast type [CrossCutting.Data.Sql.Tests.Repositories.TestEntityBuilder] to [CrossCutting.Data.Sql.Tests.Repositories.TestEntity]");
    }

    [Fact]
    public void InvokeCommand_Works_With_Builder_To_Entity_Conversion_When_Conversion_Succeeds()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(12345); //using ExecuteNonQuery flow, need to give valid result
        var builderProviderMock = new Mock<IDatabaseCommandEntityProvider<TestEntity, TestEntityBuilder>>();
        builderProviderMock.SetupGet(x => x.CreateBuilderDelegate)
                           .Returns(new Func<TestEntity, TestEntityBuilder>(entity => new TestEntityBuilder(entity)));
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        builderProviderMock.SetupGet(x => x.CreateEntityDelegate)
                           .Returns(x => x.Build());
        var builderSut = new DatabaseCommandProcessor<TestEntity, TestEntityBuilder>(Connection, builderProviderMock.Object);
        var entity = new TestEntity("A", "B", "C", true);

        // Act
        var actual = builderSut.ExecuteCommand(command, entity).HandleResult("Something went wrong");

        // Assert
        actual.Code.Should().Be(entity.Code);
        actual.CodeType.Should().Be(entity.CodeType);
        actual.Description.Should().Be(entity.Description);
        actual.IsExistingEntity.Should().Be(entity.IsExistingEntity);
    }

    public void Dispose()
    {
        Connection.Dispose();
    }

    public class MyEntity
    {
        [Required]
        public string? Property { get; set; }
    }
}
