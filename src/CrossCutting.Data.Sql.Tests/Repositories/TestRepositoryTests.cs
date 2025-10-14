namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestRepositoryTests : TestBase<TestRepository>
{
    private IDatabaseEntityRetriever<TestEntity> EntityRetrieverMock => Fixture.Freeze<IDatabaseEntityRetriever<TestEntity>>();
    private IDatabaseCommandProvider<TestEntityIdentity> IdentityCommandProviderMock => Fixture.Freeze<IDatabaseCommandProvider<TestEntityIdentity>>();

    [Fact]
    public async Task Can_Find_Entity_Using_Identity()
    {
        // Arrange
        var expected = new TestEntity("code", "codeType", "description");
        IdentityCommandProviderMock.CreateAsync(Arg.Any<TestEntityIdentity>(), DatabaseOperation.Select).Returns(Result.Success<IDatabaseCommand>(new SqlTextCommand("SELECT ...", DatabaseOperation.Select)));
        EntityRetrieverMock.FindOneAsync(Arg.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select)).Returns(Result.Success(expected));

        // Act
        var actual = (await Sut.FindAsync(new TestEntityIdentity(expected))).EnsureValue().GetValueOrThrow();

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Can_FindOne_Entity_Using_Repository()
    {
        // Arrange
        var expected = new TestEntity("code", "codeType", "description");
        EntityRetrieverMock.FindOneAsync(Arg.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select)).Returns(Result.Success(expected));

        // Act
        var actual = (await Sut.FindOneAsync()).EnsureValue().GetValueOrThrow();

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Can_FindMany_Entities_Using_Repository()
    {
        // Arrange
        var expected = new[]
        {
            new TestEntity("code1", "codeType1", "description1"),
            new TestEntity("code2", "codeType2", "description2")
        };
        EntityRetrieverMock.FindManyAsync(Arg.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select)).Returns(Result.Success<IReadOnlyCollection<TestEntity>>(expected));

        // Act
        var actual = (await Sut.FindManyAsync("Value")).EnsureValue().GetValueOrThrow();

        // Assert
        actual.ShouldBeSameAs(expected);
    }

    [Fact]
    public async Task Can_FindPaged_Entities_Using_Repository()
    {
        // Arrange
        var expected = new[]
        {
            new TestEntity("code1", "codeType1", "description1"),
            new TestEntity("code2", "codeType2", "description2")
        };
        EntityRetrieverMock.FindPagedAsync(Arg.Is<IPagedDatabaseCommand>(x => x.DataCommand.Operation == DatabaseOperation.Select)).Returns(Result.Success<IPagedResult<TestEntity>>(new PagedResult<TestEntity>(expected, 2, 0, 10)));

        // Act
        var actual = (await Sut.FindPagedAsync(0, 10)).EnsureValue().GetValueOrThrow();

        // Assert
        actual.ToArray().ShouldBeEquivalentTo(expected);
    }
}
