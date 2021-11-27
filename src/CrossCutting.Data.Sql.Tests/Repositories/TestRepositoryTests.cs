using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.Commands;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepositoryTests : TestBase<TestRepository>
    {
        private Mock<IDatabaseEntityRetriever<TestEntity>> EntityRetrieverMock => Fixture.Freeze<Mock<IDatabaseEntityRetriever<TestEntity>>>();
        private Mock<IDatabaseCommandProvider<TestEntityIdentity>> IdentityCommandProviderMock => Fixture.Freeze<Mock<IDatabaseCommandProvider<TestEntityIdentity>>>();

        [Fact]
        public void Can_Find_Entity_Using_Identity()
        {
            // Arrange
            var expected = new TestEntity("code", "codeType", "description");
            IdentityCommandProviderMock.Setup(x => x.Create(It.IsAny<TestEntityIdentity>(), DatabaseOperation.Select)).Returns(new SqlTextCommand("SELECT ...", DatabaseOperation.Select));
            EntityRetrieverMock.Setup(x => x.FindOne(It.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select))).Returns(expected);

            // Act
            var actual = Sut.Find(new TestEntityIdentity(expected));

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void Can_FindOne_Entity_Using_Repository()
        {
            // Arrange
            var expected = new TestEntity("code", "codeType", "description");
            EntityRetrieverMock.Setup(x => x.FindOne(It.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select))).Returns(expected);

            // Act
            var actual = Sut.FindOne();

            // Assert
            actual.Should().Be(expected);
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
            EntityRetrieverMock.Setup(x => x.FindMany(It.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select))).Returns(expected);

            // Act
            var actual = Sut.FindMany("Value");

            // Assert
            actual.Should().BeSameAs(expected);
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
            EntityRetrieverMock.Setup(x => x.FindPaged(It.Is<IPagedDatabaseCommand>(x => x.DataCommand.Operation == DatabaseOperation.Select))).Returns(new PagedResult<TestEntity>(expected, 2, 0, 10));

            // Act
            var actual = Sut.FindPaged(0, 10);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
