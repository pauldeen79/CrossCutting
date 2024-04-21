namespace CrossCutting.Data.Sql.Tests;

public sealed class DatabaseEntityRetrieverTests : IDisposable
{
    private DatabaseEntityRetriever<MyEntity> Sut { get; }
    private DbConnection Connection { get; }
    private IDatabaseEntityMapper<MyEntity> MapperMock { get; }
    private ISqlCommandWrapperFactory SqlCommandWrapperFactoryMock { get; }

    public DatabaseEntityRetrieverTests()
    {
        Connection = new DbConnection();
        MapperMock = Substitute.For<IDatabaseEntityMapper<MyEntity>>();
        SqlCommandWrapperFactoryMock = Substitute.For<ISqlCommandWrapperFactory>();

        SqlCommandWrapperFactoryMock.Create(Arg.Any<IDbCommand>()).Returns(x => new SqlCommandWrapper(
            x.ArgAt<IDbCommand>(0),
            (cmd, _) => Task.FromResult(cmd.ExecuteNonQuery()),
            (cmd, _, _) => Task.FromResult(new SqlDataReaderWrapper(
                cmd.ExecuteReader(),
                (reader, _) => Task.FromResult(reader.Read()),
                (reader, _) => Task.FromResult(reader.NextResult()),
                (reader) => { reader.Close(); return Task.CompletedTask; })
            ),
            (cmd, _) => Task.FromResult(cmd.ExecuteScalar())));

        Sut = new DatabaseEntityRetriever<MyEntity>(Connection, MapperMock, SqlCommandWrapperFactoryMock);
    }

    [Fact]
    public void FindOne_Returns_MappedEntity_When_All_Goes_Well()
    {
        // Arrange
        Connection.AddResultForDataReader(new[] { new MyEntity { Property = "test" } });
        InitializeMapper();

        // Act
        var actual = Sut.FindOne(new SqlDatabaseCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text));

        // Assert
        actual.Should().NotBeNull();
        actual?.Property.Should().Be("test");
    }

    [Fact]
    public async Task FindOneAync_Returns_MappedEntity_When_All_Goes_Well()
    {
        // Arrange
        Connection.AddResultForDataReader(new[] { new MyEntity { Property = "test" } });
        InitializeMapper();

        // Act
        var actual = await Sut.FindOneAsync(new SqlDatabaseCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text));

        // Assert
        actual.Should().NotBeNull();
        actual?.Property.Should().Be("test");
    }

    [Fact]
    public void FindMany_Returns_MappedEntities_When_All_Goes_Well()
    {
        // Arrange
        Connection.AddResultForDataReader(new[]
        {
            new MyEntity { Property = "test1" },
            new MyEntity { Property = "test2" }
        });
        InitializeMapper();

        // Act
        var actual = Sut.FindMany(new SqlDatabaseCommand("SELECT Property FROM MyEntity", DatabaseCommandType.Text));

        // Assert
        actual.Should().NotBeNull().And.HaveCount(2);
        actual.First().Should().NotBeNull();
        actual.First().Property.Should().Be("test1");
        actual.Last().Should().NotBeNull();
        actual.Last().Property.Should().Be("test2");
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
        var actual = await Sut.FindManyAsync(new SqlDatabaseCommand("SELECT Property FROM MyEntity", DatabaseCommandType.Text));

        // Assert
        actual.Should().NotBeNull().And.HaveCount(2);
        actual.First().Should().NotBeNull();
        actual.First().Property.Should().Be("test1");
        actual.Last().Should().NotBeNull();
        actual.Last().Property.Should().Be("test2");
    }

    [Fact]
    public void FindPaged_Returns_MappedEntities_And_Other_Properties_When_All_Goes_Well()
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
        var actual = Sut.FindPaged(command);

        // Assert
        actual.Should().NotBeNull().And.HaveCount(2);
        actual.First().Should().NotBeNull();
        actual.First().Property.Should().Be("test1");
        actual.Last().Should().NotBeNull();
        actual.Last().Property.Should().Be("test2");
        actual.TotalRecordCount.Should().Be(1);
        actual.Offset.Should().Be(20);
        actual.PageSize.Should().Be(10);
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
        var actual = await Sut.FindPagedAsync(command);

        // Assert
        actual.Should().NotBeNull().And.HaveCount(2);
        actual.First().Should().NotBeNull();
        actual.First().Property.Should().Be("test1");
        actual.Last().Should().NotBeNull();
        actual.Last().Property.Should().Be("test2");
        actual.TotalRecordCount.Should().Be(1);
        actual.Offset.Should().Be(20);
        actual.PageSize.Should().Be(10);
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
