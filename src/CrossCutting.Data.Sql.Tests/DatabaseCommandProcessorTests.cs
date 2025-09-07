namespace CrossCutting.Data.Sql.Tests;

public sealed class DatabaseCommandProcessorTests : IDisposable
{
    private DatabaseCommandProcessor<MyEntity> Sut { get; }
    private DbConnection Connection { get; }
    private IDatabaseCommandEntityProvider<MyEntity> ProviderMock { get; }

    public DatabaseCommandProcessorTests()
    {
        Connection = new DbConnection();
        ProviderMock = Substitute.For<IDatabaseCommandEntityProvider<MyEntity>>();

        Sut = new DatabaseCommandProcessor<MyEntity>(Connection, ProviderMock);
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
        actual.ShouldBe(12345);
    }

    [Fact]
    public async Task ExecuteScalarAsync_Returns_Correct_Value_When_Command_Is_Not_Null()
    {
        // Arrange
        Connection.AddResultForScalarCommand(12345);
        var command = new SqlDatabaseCommand("Select 12345", DatabaseCommandType.Text, DatabaseOperation.Unspecified);

        // Act
        var actual = await Sut.ExecuteScalarAsync(command);

        // Assert
        actual.ShouldBe(12345);
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
        actual.ShouldBe(12345);
    }

    [Fact]
    public async Task ExecuteNonQueryAsync_Returns_Correct_Value_When_Command_Is_Not_Null()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(12345);
        var command = new SqlDatabaseCommand("Select 12345", DatabaseCommandType.Text, DatabaseOperation.Unspecified);

        // Act
        var actual = await Sut.ExecuteNonQueryAsync(command);

        // Assert
        actual.ShouldBe(12345);
    }

    [Fact]
    public void InvokeCommand_Throws_When_ResultEntityDelegate_Returns_Null()
    {
        // Arrange
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.CreateResultEntity.Returns((_, _) => null!);

        // Act
        Action a = () => Sut.ExecuteCommand(command, new MyEntity { Property = "filled" });
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Instance should be supplied, or result entity delegate should deliver an instance");
    }

    [Fact]
    public async Task InvokeCommandAsync_Throws_When_ResultEntityDelegate_Returns_Null()
    {
        // Arrange
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.CreateResultEntity.Returns((_, _) => null!);

        // Act
        Task t = Sut.ExecuteCommandAsync(command, new MyEntity { Property = "filled" });
        (await t.ShouldThrowAsync<InvalidOperationException>())
         .Message.ShouldBe("Instance should be supplied, or result entity delegate should deliver an instance");
    }

    [Fact]
    public void InvokeCommand_Does_Not_Throw_When_OperationValidationDelegate_Returns_True()
    {
        // Arrange
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<MyEntity>?));
        ProviderMock.CreateEntity.Returns(default(CreateEntityHandler<MyEntity, MyEntity>?));

        // Act
        Action a = () => Sut.ExecuteCommand(command, new MyEntity { Property = "filled" });
        a.ShouldNotThrow();
    }

    [Fact]
    public async Task InvokeCommandAsync_Does_Not_Throw_When_OperationValidationDelegate_Returns_True()
    {
        // Arrange
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<MyEntity>?));
        ProviderMock.CreateEntity.Returns(default(CreateEntityHandler<MyEntity, MyEntity>?));

        // Act
        Task t = Sut.ExecuteCommandAsync(command, new MyEntity { Property = "filled" });
        await t.ShouldNotThrowAsync();
    }

    [Fact]
    public void InvokeCommand_Throws_When_Instance_Validation_Fails()
    {
        // Arrange
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<MyEntity>?));
        ProviderMock.CreateEntity.Returns(default(CreateEntityHandler<MyEntity, MyEntity>?));

        // Act
        Action a = () => Sut.ExecuteCommand(command, new MyEntity { Property = null });
        a.ShouldThrow<ValidationException>()
         .Message.ShouldBe("The Property field is required.");
    }

    [Fact]
    public async Task InvokeCommandAsync_Throws_When_Instance_Validation_Fails()
    {
        // Arrange
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<MyEntity>?));
        ProviderMock.CreateEntity.Returns(default(CreateEntityHandler<MyEntity, MyEntity>?));

        // Act
        Task t = Sut.ExecuteCommandAsync(command, new MyEntity { Property = null });
        (await t.ShouldThrowAsync<ValidationException>())
         .Message.ShouldBe("The Property field is required.");
    }

    [Fact]
    public void InvokeCommand_No_AfterReadDelegate_Returns_Error_When_ExecuteNonQuery_Returns_0()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(0); // 0 rows affected
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.AfterRead.Returns(default(AfterReadHandler<MyEntity>?));
        ProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<MyEntity>?));
        ProviderMock.CreateEntity.Returns(default(CreateEntityHandler<MyEntity, MyEntity>?));

        // Act
        var result = Sut.ExecuteCommand(command, new MyEntity { Property = "test" }).HandleResult("MyEntity entity was not added");

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("MyEntity entity was not added");
    }

    [Fact]
    public async Task InvokeCommandAsync_No_AfterReadDelegate_Returns_Error_When_ExecuteNonQuery_Returns_0()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(0); // 0 rows affected
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.AfterRead.Returns(default(AfterReadHandler<MyEntity>?));
        ProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<MyEntity>?));
        ProviderMock.CreateEntity.Returns(default(CreateEntityHandler<MyEntity, MyEntity>?));

        // Act
        var result = (await Sut.ExecuteCommandAsync(command, new MyEntity { Property = "test" })).HandleResult("MyEntity entity was not added");

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("MyEntity entity was not added");
    }

    [Fact]
    public void InvokeCommand_No_AfterReadDelegate_Does_Not_Throw_When_ExecuteNonQuery_Returns_1()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(1); // 1 row affected
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.AfterRead.Returns(default(AfterReadHandler<MyEntity>?));
        ProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<MyEntity>?));

        // Act
        Action a = () => Sut.ExecuteCommand(command, new MyEntity { Property = "test" });
        a.ShouldNotThrow();
    }

    [Fact]
    public void InvokeCommandAsync_No_AfterReadDelegate_Does_Not_Throw_When_ExecuteNonQuery_Returns_1()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(1); // 1 row affected
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.AfterRead.Returns(default(AfterReadHandler<MyEntity>?));

        // Act
        Task t = Sut.ExecuteCommandAsync(command, new MyEntity { Property = "test" });
        t.ShouldNotThrowAsync();
    }

    [Fact]
    public void InvokeCommand_AfterReadDelegate_Returns_Error_When_ExecuteReader_Read_Returns_False()
    {
        // Arrange
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<MyEntity>?));
        ProviderMock.CreateEntity.Returns(default(CreateEntityHandler<MyEntity, MyEntity>?));
        ProviderMock.AfterRead.Returns(new AfterReadHandler<MyEntity>((x, _, _) => x));

        // Act
        var result = Sut.ExecuteCommand(command, new MyEntity { Property = "test" }).HandleResult("MyEntity entity was not added");

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("MyEntity entity was not added");
    }

    [Fact]
    public async Task InvokeCommandAsync_AfterReadDelegate_Returns_Error_When_ExecuteReader_Read_Returns_False()
    {
        // Arrange
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<MyEntity>?));
        ProviderMock.CreateEntity.Returns(default(CreateEntityHandler<MyEntity, MyEntity>?));
        ProviderMock.AfterRead.Returns(new AfterReadHandler<MyEntity>((x, _, _) => x));

        // Act
        var result = (await Sut.ExecuteCommandAsync(command, new MyEntity { Property = "test" })).HandleResult("MyEntity entity was not added");

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("MyEntity entity was not added");
    }

    [Fact]
    public void InvokeCommand_AfterReadDelegate_Does_Not_Throw_When_ExecuteReader_Read_Returns_True()
    {
        // Arrange
        Connection.AddResultForDataReader(new[] { new MyEntity { Property = "test" } });
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<MyEntity>?));
        ProviderMock.CreateEntity.Returns(default(CreateEntityHandler<MyEntity, MyEntity>?));
        ProviderMock.AfterRead.Returns(new AfterReadHandler<MyEntity>((x, _, _) => x));

        // Act
        var actual = Sut.ExecuteCommand(command, new MyEntity { Property = "test" }).HandleResult("MyEntity entity was not added").EnsureValue().GetValueOrThrow();

        // Assert
        actual.Property.ShouldBe("test");
    }

    [Fact]
    public async Task InvokeCommandAsync_AfterReadDelegate_Does_Not_Throw_When_ExecuteReader_Read_Returns_True()
    {
        // Arrange
        Connection.AddResultForDataReader(new[] { new MyEntity { Property = "test" } });
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        ProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<MyEntity>?));
        ProviderMock.CreateEntity.Returns(default(CreateEntityHandler<MyEntity, MyEntity>?));
        ProviderMock.AfterRead.Returns(new AfterReadHandler<MyEntity>((x, _, _) => x));

        // Act
        var actual = (await Sut.ExecuteCommandAsync(command, new MyEntity { Property = "test" })).HandleResult("MyEntity entity was not added").EnsureValue().GetValueOrThrow();

        // Assert
        actual.Property.ShouldBe("test");
    }

    [Fact]
    public void InvokeCommand_Builder_To_Entity_Conversion_Throws_When_Builder_Could_Not_Be_Constructed()
    {
        // Arrange
        var builderProviderMock = Substitute.For<IDatabaseCommandEntityProvider<TestEntity, TestEntityBuilder>>();
        var builderSut = new DatabaseCommandProcessor<TestEntity, TestEntityBuilder>(Connection, builderProviderMock);
        var entity = new TestEntity("A", "B", "C", true);

        // Act & Assert4
        Action a = () => builderSut.ExecuteCommand(Substitute.For<IDatabaseCommand>(), entity);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Builder instance was not constructed, create builder delegate should deliver an instance");
    }

    [Fact]
    public async Task InvokeCommandAsync_Builder_To_Entity_Conversion_Throws_When_Builder_Could_Not_Be_Constructed()
    {
        // Arrange
        var builderProviderMock = Substitute.For<IDatabaseCommandEntityProvider<TestEntity, TestEntityBuilder>>();
        var builderSut = new DatabaseCommandProcessor<TestEntity, TestEntityBuilder>(Connection, builderProviderMock);
        var entity = new TestEntity("A", "B", "C", true);

        // Act & Assert
        Task t = builderSut.ExecuteCommandAsync(Substitute.For<IDatabaseCommand>(), entity);
        (await t.ShouldThrowAsync<InvalidOperationException>())
         .Message.ShouldBe("Builder instance was not constructed, create builder delegate should deliver an instance");
    }

    [Fact]
    public void InvokeCommand_Entity_To_Builder_Conversion_Throws_When_Entity_Could_Not_Be_Constructed()
    {
        // Arrange
        var builderProviderMock = Substitute.For<IDatabaseCommandEntityProvider<TestEntity, TestEntityBuilder>>();
        builderProviderMock.CreateBuilder.Returns(new CreateBuilderHandler<TestEntity, TestEntityBuilder>(entity => new TestEntityBuilder(entity)));
        builderProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<TestEntityBuilder>?));
        builderProviderMock.CreateEntity.Returns(default(CreateEntityHandler<TestEntityBuilder, TestEntity>?));

        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        var builderSut = new DatabaseCommandProcessor<TestEntity, TestEntityBuilder>(Connection, builderProviderMock);
        var entity = new TestEntity("A", "B", "C", true);

        // Act & Assert
        Action a = () => builderSut.ExecuteCommand(command, entity);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Could not cast type [CrossCutting.Data.Sql.Tests.Repositories.TestEntityBuilder] to [CrossCutting.Data.Sql.Tests.Repositories.TestEntity]");
    }

    [Fact]
    public async Task InvokeCommandAsync_Entity_To_Builder_Conversion_Throws_When_Entity_Could_Not_Be_Constructed()
    {
        // Arrange
        var builderProviderMock = Substitute.For<IDatabaseCommandEntityProvider<TestEntity, TestEntityBuilder>>();
        builderProviderMock.CreateBuilder.Returns(new CreateBuilderHandler<TestEntity, TestEntityBuilder>(entity => new TestEntityBuilder(entity)));
        builderProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<TestEntityBuilder>?));
        builderProviderMock.CreateEntity.Returns(default(CreateEntityHandler<TestEntityBuilder, TestEntity>?));

        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        var builderSut = new DatabaseCommandProcessor<TestEntity, TestEntityBuilder>(Connection, builderProviderMock);
        var entity = new TestEntity("A", "B", "C", true);

        // Act & Assert
        var t = builderSut.ExecuteCommandAsync(command, entity);
        (await t.ShouldThrowAsync<InvalidOperationException>())
         .Message.ShouldBe("Could not cast type [CrossCutting.Data.Sql.Tests.Repositories.TestEntityBuilder] to [CrossCutting.Data.Sql.Tests.Repositories.TestEntity]");
    }

    [Fact]
    public void InvokeCommand_Works_With_Builder_To_Entity_Conversion_When_Conversion_Succeeds()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(12345); //using ExecuteNonQuery flow, need to give valid result
        var builderProviderMock = Substitute.For<IDatabaseCommandEntityProvider<TestEntity, TestEntityBuilder>>();
        builderProviderMock.CreateBuilder.Returns(new CreateBuilderHandler<TestEntity, TestEntityBuilder>(entity => new TestEntityBuilder(entity)));
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        builderProviderMock.CreateEntity.Returns(x => x.Build());
        builderProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<TestEntityBuilder>?));
        builderProviderMock.AfterRead.Returns(default(AfterReadHandler<TestEntityBuilder>?));

        var builderSut = new DatabaseCommandProcessor<TestEntity, TestEntityBuilder>(Connection, builderProviderMock);
        var entity = new TestEntity("A", "B", "C", true);

        // Act
        var actual = builderSut.ExecuteCommand(command, entity).HandleResult("Something went wrong").EnsureValue().GetValueOrThrow();

        // Assert
        actual.Code.ShouldBe(entity.Code);
        actual.CodeType.ShouldBe(entity.CodeType);
        actual.Description.ShouldBe(entity.Description);
        actual.IsExistingEntity.ShouldBe(entity.IsExistingEntity);
    }

    [Fact]
    public async Task InvokeCommandAsync_Works_With_Builder_To_Entity_Conversion_When_Conversion_Succeeds()
    {
        // Arrange
        Connection.AddResultForNonQueryCommand(12345); //using ExecuteNonQuery flow, need to give valid result
        var builderProviderMock = Substitute.For<IDatabaseCommandEntityProvider<TestEntity, TestEntityBuilder>>();
        builderProviderMock.CreateBuilder.Returns(new CreateBuilderHandler<TestEntity, TestEntityBuilder>(entity => new TestEntityBuilder(entity)));
        var command = new SqlDatabaseCommand("INSERT INTO ...", DatabaseCommandType.Text, DatabaseOperation.Insert);
        builderProviderMock.CreateEntity.Returns(x => x.Build());
        builderProviderMock.CreateResultEntity.Returns(default(CreateResultEntityHandler<TestEntityBuilder>?));
        builderProviderMock.AfterRead.Returns(default(AfterReadHandler<TestEntityBuilder>?));

        var builderSut = new DatabaseCommandProcessor<TestEntity, TestEntityBuilder>(Connection, builderProviderMock);
        var entity = new TestEntity("A", "B", "C", true);

        // Act
        var actual = (await builderSut.ExecuteCommandAsync(command, entity)).HandleResult("Something went wrong").EnsureValue().GetValueOrThrow();

        // Assert
        actual.Code.ShouldBe(entity.Code);
        actual.CodeType.ShouldBe(entity.CodeType);
        actual.Description.ShouldBe(entity.Description);
        actual.IsExistingEntity.ShouldBe(entity.IsExistingEntity);
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
