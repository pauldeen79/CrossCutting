namespace CrossCutting.Data.Sql.Tests;

public sealed class DatabaseEntityRetrieverTests : IDisposable
{
    private DatabaseEntityRetriever<MyEntity> Sut { get; }
    private DbConnection Connection { get; }
    private IDatabaseEntityMapper<MyEntity> MapperMock { get; }

    public DatabaseEntityRetrieverTests()
    {
        Connection = new DbConnection();
        MapperMock = Substitute.For<IDatabaseEntityMapper<MyEntity>>();

        Sut = new DatabaseEntityRetriever<MyEntity>(Connection, MapperMock);
    }

    [Fact]
    public async Task FindOneAync_Returns_MappedEntity_When_All_Goes_Well()
    {
        // Arrange
        Connection.AddResultForDataReader(new[] { new MyEntity { Property = "test" } });
        InitializeMapper();

        // Act
        var actual = (await Sut.FindOneAsync(new SqlDatabaseCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text))).EnsureValue().GetValueOrThrow();

        // Assert
        actual.ShouldNotBeNull();
        actual.Property.ShouldBe("test");
    }

    [Fact]
    public async Task FindManyAsync_Returns_MappedEntities_When_All_Goes_Well()
    {
        // Arrange
        Connection.AddResultForDataReader(new[]
        {
            new MyEntity { Property = "test1" },
            new MyEntity { Property = "test2" }
        });
        InitializeMapper();

        // Act
        var actual = (await Sut.FindManyAsync(new SqlDatabaseCommand("SELECT Property FROM MyEntity", DatabaseCommandType.Text))).EnsureValue().GetValueOrThrow();

        // Assert
        actual.ShouldNotBeNull().Count.ShouldBe(2);
        actual.First().ShouldNotBeNull();
        actual.First().Property.ShouldBe("test1");
        actual.Last().ShouldNotBeNull();
        actual.Last().Property.ShouldBe("test2");
    }

    [Fact]
    public async Task FindPagedAsync_Returns_MappedEntities_And_Other_Properties_When_All_Goes_Well()
    {
        // Arrange
        Connection.AddResultForDataReader(new[]
        {
            new MyEntity { Property = "test1" },
            new MyEntity { Property = "test2" }
        });
        Connection.AddResultForScalarCommand(1);
        InitializeMapper();
        var command = new PagedDatabaseCommand(new SqlDatabaseCommand("SELECT Property FROM MyEntity", DatabaseCommandType.Text),
                                               new SqlDatabaseCommand("SELECT COUNT(*) FROM MyEntity", DatabaseCommandType.Text),
                                               20,
                                               10);

        // Act
        var actual = (await Sut.FindPagedAsync(command)).EnsureValue().GetValueOrThrow();

        // Assert
        actual.ShouldNotBeNull().Count.ShouldBe(2);
        actual.First().ShouldNotBeNull();
        actual.First().Property.ShouldBe("test1");
        actual.Last().ShouldNotBeNull();
        actual.Last().Property.ShouldBe("test2");
        actual.TotalRecordCount.ShouldBe(1);
        actual.Offset.ShouldBe(20);
        actual.PageSize.ShouldBe(10);
    }

    private void InitializeMapper()
    {
        MapperMock.Map(Arg.Any<IDataReader>())
                  .Returns(x => new MyEntity { Property = x.ArgAt<IDataReader>(0).GetString("Property") });
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
