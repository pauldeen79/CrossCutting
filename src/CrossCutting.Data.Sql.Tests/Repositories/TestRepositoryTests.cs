﻿namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestRepositoryTests : TestBase<TestRepository>
{
    private IDatabaseEntityRetriever<TestEntity> EntityRetrieverMock => Fixture.Freeze<IDatabaseEntityRetriever<TestEntity>>();
    private IDatabaseCommandProvider<TestEntityIdentity> IdentityCommandProviderMock => Fixture.Freeze<IDatabaseCommandProvider<TestEntityIdentity>>();

    [Fact]
    public void Can_Find_Entity_Using_Identity()
    {
        // Arrange
        var expected = new TestEntity("code", "codeType", "description");
        IdentityCommandProviderMock.Create(Arg.Any<TestEntityIdentity>(), DatabaseOperation.Select).Returns(new SqlTextCommand("SELECT ...", DatabaseOperation.Select));
        EntityRetrieverMock.FindOne(Arg.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select)).Returns(expected);

        // Act
        var actual = Sut.Find(new TestEntityIdentity(expected));

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void Can_FindOne_Entity_Using_Repository()
    {
        // Arrange
        var expected = new TestEntity("code", "codeType", "description");
        EntityRetrieverMock.FindOne(Arg.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select)).Returns(expected);

        // Act
        var actual = Sut.FindOne();

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void Can_FindMany_Entities_Using_Repository()
    {
        // Arrange
        var expected = new[]
        {
            new TestEntity("code1", "codeType1", "description1"),
            new TestEntity("code2", "codeType2", "description2")
        };
        EntityRetrieverMock.FindMany(Arg.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select)).Returns(expected);

        // Act
        var actual = Sut.FindMany("Value");

        // Assert
        actual.ShouldBeSameAs(expected);
    }

    [Fact]
    public void Can_FindPaged_Entities_Using_Repository()
    {
        // Arrange
        var expected = new[]
        {
            new TestEntity("code1", "codeType1", "description1"),
            new TestEntity("code2", "codeType2", "description2")
        };
        EntityRetrieverMock.FindPaged(Arg.Is<IPagedDatabaseCommand>(x => x.DataCommand.Operation == DatabaseOperation.Select)).Returns(new PagedResult<TestEntity>(expected, 2, 0, 10));

        // Act
        var actual = Sut.FindPaged(0, 10);

        // Assert
        actual.ToArray().ShouldBeEquivalentTo(expected);
    }
}
