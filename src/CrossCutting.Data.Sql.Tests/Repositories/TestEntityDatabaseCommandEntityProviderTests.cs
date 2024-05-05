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
        actual.CommandText.Should().Be(expectedCommandText);
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
        sut.Invoking(x => x.CreateCommand.Invoke(entity, operation))
           .Should().Throw<ArgumentOutOfRangeException>()
           .And.ParamName.Should().Be("operation");
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
        sut.CreateResultEntity.Should().NotBeNull();
        if (sut.CreateResultEntity is not null)
        {
            var actual = sut.CreateResultEntity.Invoke(entity, operation);

            // Assert
            actual.Should().Be(entity);
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
        sut.CreateResultEntity.Should().NotBeNull();
        if (sut.CreateResultEntity is not null)
        {
            sut.Invoking(x => x.CreateResultEntity?.Invoke(entity, operation))
               .Should().Throw<ArgumentOutOfRangeException>()
               .And.ParamName.Should().Be("operation");
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
        sut.AfterRead.Should().NotBeNull();
        if (sut.AfterRead is not null)
        {
            var actual = sut.AfterRead.Invoke(entity, operation, readerMock);

            // Assert
            actual.Should().Be(entity);
            if (operation != DatabaseOperation.Delete)
            {
                actual.Code.Should().Be("new code");
                actual.CodeType.Should().Be("new code type");
                actual.Description.Should().Be("new description");
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
        sut.AfterRead.Should().NotBeNull();
        if (sut.AfterRead is not null)
        {
            sut.Invoking(x => x.AfterRead?.Invoke(entity, operation, readerMock))
               .Should().Throw<ArgumentOutOfRangeException>()
               .And.ParamName.Should().Be("operation");
        }
    }

    [Fact]
    public void CreateBuilderDelegate_Creates_Builder_From_Entity()
    {
        // Arrange
        var sut = new TestEntityDatabaseCommandEntityProvider();
        var entity = new TestEntity("A", "B", "C", true);

        // Act
        sut.CreateBuilder.Should().NotBeNull();
        if (sut.CreateBuilder is not null)
        {
            var actual = sut.CreateBuilder.Invoke(entity);

            // Assert
            actual.Should().NotBeNull();
            actual.Code.Should().Be(entity.Code);
            actual.CodeType.Should().Be(entity.CodeType);
            actual.Description.Should().Be(entity.Description);
        }
    }

    [Fact]
    public void CreateEntityDelegate_Creates_Entity_From_Builder()
    {
        // Arrange
        var sut = new TestEntityDatabaseCommandEntityProvider();
        var builder = new TestEntityBuilder(new TestEntity("A", "B", "C", true));

        // Act
        sut.CreateEntity.Should().NotBeNull();
        if (sut.CreateEntity is not null)
        {
            var actual = sut.CreateEntity.Invoke(builder);

            // Assert
            actual.Should().NotBeNull();
            actual.Code.Should().Be(builder.Code);
            actual.CodeType.Should().Be(builder.CodeType);
            actual.Description.Should().Be(builder.Description);
        }
    }
}
