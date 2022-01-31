namespace CrossCutting.Data.Sql.Tests.Repositories;

[ExcludeFromCodeCoverage]
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
        var actual = sut.CommandDelegate.Invoke(entity, operation);

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
        sut.Invoking(x => x.CommandDelegate.Invoke(entity, operation))
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
        sut.ResultEntityDelegate.Should().NotBeNull();
        if (sut.ResultEntityDelegate != null)
        {
            var actual = sut.ResultEntityDelegate.Invoke(entity, operation);

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
        sut.ResultEntityDelegate.Should().NotBeNull();
        if (sut.ResultEntityDelegate != null)
        {
            sut.Invoking(x => x.ResultEntityDelegate?.Invoke(entity, operation))
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
        var readerMock = new Mock<IDataReader>();
        readerMock.SetupGet(x => x.FieldCount).Returns(3);
        readerMock.Setup(x => x.GetName(It.IsAny<int>())).Returns<int>(index =>
            index switch
            {
                0 => "Code",
                1 => "CodeType",
                2 => "Description",
                _ => string.Empty,
            }
        );
        readerMock.Setup(x => x.GetOrdinal(It.IsAny<string>())).Returns<string>(name =>
            name switch
            {
                "Code" => 0,
                "CodeType" => 1,
                "Description" => 2,
                _ => -1,
            }
        );
        readerMock.Setup(x => x.GetString(It.IsAny<int>())).Returns<int>(index =>
            index switch
            {
                0 => "new code",
                1 => "new code type",
                2 => "new description",
                _ => string.Empty,
            }
        );

        // Act
        sut.AfterReadDelegate.Should().NotBeNull();
        if (sut.AfterReadDelegate != null)
        {
            var actual = sut.AfterReadDelegate.Invoke(entity, operation, readerMock.Object);

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
        var readerMock = new Mock<IDataReader>();

        // Act
        sut.AfterReadDelegate.Should().NotBeNull();
        if (sut.AfterReadDelegate != null)
        {
            sut.Invoking(x => x.AfterReadDelegate?.Invoke(entity, operation, readerMock.Object))
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
        sut.CreateBuilderDelegate.Should().NotBeNull();
        if (sut.CreateBuilderDelegate != null)
        {
            var actual = sut.CreateBuilderDelegate.Invoke(entity);

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
        sut.CreateEntityDelegate.Should().NotBeNull();
        if (sut.CreateEntityDelegate != null)
        {
            var actual = sut.CreateEntityDelegate.Invoke(builder);

            // Assert
            actual.Should().NotBeNull();
            actual.Code.Should().Be(builder.Code);
            actual.CodeType.Should().Be(builder.CodeType);
            actual.Description.Should().Be(builder.Description);
        }
    }
}
