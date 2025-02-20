namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestEntityDatabaseCommandEntityProviderTests
{
    [Theory]
    [InlineData(DatabaseOperation.Insert, "[InsertEntity]")]
    [InlineData(DatabaseOperation.Update, "[UpdateEntity]")]
    [InlineData(DatabaseOperation.Delete, "[DeleteEntity]")]
    public void CommandDelegate_Returns_Correct_Command(DatabaseOperation operation, string expectedCommandText)
    {
        // Arrange
        var sut = new TestEntityDatabaseCommandEntityProvider();
        var entity = new TestEntityBuilder(new TestEntity("", "", ""));

        // Act
        var actual = sut.CreateCommand.Invoke(entity, operation);

        // Assert
        actual.CommandText.ShouldBe(expectedCommandText);
    }

    [Theory]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Select)]
    public void CommandDelegate_Throws_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
    {
        // Arrange
        var sut = new TestEntityDatabaseCommandEntityProvider();
        var entity = new TestEntityBuilder(new TestEntity("", "", ""));

        // Act
        Action a = () => sut.CreateCommand.Invoke(entity, operation);
        a.ShouldThrow<ArgumentOutOfRangeException>()
         .ParamName.ShouldBe("operation");
    }

    [Theory]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Update)]
    [InlineData(DatabaseOperation.Delete)]
    public void ResultEntityDelegate_Returns_Correct_ResultEntity(DatabaseOperation operation)
    {
        // Arrange
        var sut = new TestEntityDatabaseCommandEntityProvider();
        var entity = new TestEntityBuilder(new TestEntity("", "", ""));

        // Act
        sut.CreateResultEntity.ShouldNotBeNull();
        if (sut.CreateResultEntity is not null)
        {
            var actual = sut.CreateResultEntity.Invoke(entity, operation);

            // Assert
            actual.ShouldBe(entity);
        }
    }

    [Theory]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Select)]
    public void ResultEntityDelegate_Throws_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
    {
        // Arrange
        var sut = new TestEntityDatabaseCommandEntityProvider();
        var entity = new TestEntityBuilder(new TestEntity("", "", ""));

        // Act
        sut.CreateResultEntity.ShouldNotBeNull();
        if (sut.CreateResultEntity is not null)
        {
            Action a = () => sut.CreateResultEntity?.Invoke(entity, operation);
            a.ShouldThrow<ArgumentOutOfRangeException>()
             .ParamName.ShouldBe("operation");
        }
    }

    [Theory]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Update)]
    [InlineData(DatabaseOperation.Delete)]
    public void AfterReadDelegate_Returns_Correct_ResultEntity(DatabaseOperation operation)
    {
        // Arrange
        var sut = new TestEntityDatabaseCommandEntityProvider();
        var entity = new TestEntityBuilder(new TestEntity("", "", ""));
        var readerMock = Substitute.For<IDataReader>();
        readerMock.FieldCount.Returns(3);
        readerMock.GetName(Arg.Any<int>()).Returns(x =>
            x.ArgAt<int>(0) switch
            {
                0 => "Code",
                1 => "CodeType",
                2 => "Description",
                _ => string.Empty,
            }
        );
        readerMock.GetOrdinal(Arg.Any<string>()).Returns(x =>
            x.ArgAt<string>(0) switch
            {
                "Code" => 0,
                "CodeType" => 1,
                "Description" => 2,
                _ => -1,
            }
        );
        readerMock.GetString(Arg.Any<int>()).Returns(x =>
            x.ArgAt<int>(0) switch
            {
                0 => "new code",
                1 => "new code type",
                2 => "new description",
                _ => string.Empty,
            }
        );

        // Act
        sut.AfterRead.ShouldNotBeNull();
        if (sut.AfterRead is not null)
        {
            var actual = sut.AfterRead.Invoke(entity, operation, readerMock);

            // Assert
            actual.ShouldBe(entity);
            if (operation != DatabaseOperation.Delete)
            {
                actual.Code.ShouldBe("new code");
                actual.CodeType.ShouldBe("new code type");
                actual.Description.ShouldBe("new description");
            }
        }
    }

    [Theory]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Select)]
    public void AfterReadDelegate_Throws_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
    {
        // Arrange
        var sut = new TestEntityDatabaseCommandEntityProvider();
        var entity = new TestEntityBuilder(new TestEntity("", "", ""));
        var readerMock = Substitute.For<IDataReader>();

        // Act
        sut.AfterRead.ShouldNotBeNull();
        if (sut.AfterRead is not null)
        {
            Action a = () => sut.AfterRead?.Invoke(entity, operation, readerMock);
            a.ShouldThrow<ArgumentOutOfRangeException>()
             .ParamName.ShouldBe("operation");
        }
    }

    [Fact]
    public void CreateBuilderDelegate_Creates_Builder_From_Entity()
    {
        // Arrange
        var sut = new TestEntityDatabaseCommandEntityProvider();
        var entity = new TestEntity("A", "B", "C", true);

        // Act
        sut.CreateBuilder.ShouldNotBeNull();
        if (sut.CreateBuilder is not null)
        {
            var actual = sut.CreateBuilder.Invoke(entity);

            // Assert
            actual.ShouldNotBeNull();
            actual.Code.ShouldBe(entity.Code);
            actual.CodeType.ShouldBe(entity.CodeType);
            actual.Description.ShouldBe(entity.Description);
        }
    }

    [Fact]
    public void CreateEntityDelegate_Creates_Entity_From_Builder()
    {
        // Arrange
        var sut = new TestEntityDatabaseCommandEntityProvider();
        var builder = new TestEntityBuilder(new TestEntity("A", "B", "C", true));

        // Act
        sut.CreateEntity.ShouldNotBeNull();
        if (sut.CreateEntity is not null)
        {
            var actual = sut.CreateEntity.Invoke(builder);

            // Assert
            actual.ShouldNotBeNull();
            actual.Code.ShouldBe(builder.Code);
            actual.CodeType.ShouldBe(builder.CodeType);
            actual.Description.ShouldBe(builder.Description);
        }
    }
}
