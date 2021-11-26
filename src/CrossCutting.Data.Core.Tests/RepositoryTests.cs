using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;
using CrossCutting.Data.Core.Tests.TestFixtures;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Data.Core.Tests
{
    [ExcludeFromCodeCoverage]
    public class RepositoryTests : TestBase<Repository<TestEntity, TestEntityIdentity>>
    {
        private Mock<IDatabaseCommandProcessor<TestEntity>> CommandProcessorMock => Fixture.Freeze<Mock<IDatabaseCommandProcessor<TestEntity>>>();
        private Mock<IDatabaseEntityRetriever<TestEntity>> EntityRetrieverMock => Fixture.Freeze<Mock<IDatabaseEntityRetriever<TestEntity>>>();
        private Mock<IDatabaseCommandProvider<TestEntityIdentity>> IdentitySelectCommandProviderMock => Fixture.Freeze<Mock<IDatabaseCommandProvider<TestEntityIdentity>>>();
        private Mock<IDatabaseCommandProvider> EntitySelectCommandProviderMock => Fixture.Freeze<Mock<IDatabaseCommandProvider>>();
        private Mock<IPagedDatabaseCommandProvider> PagedEntitySelectCommandProviderMock => Fixture.Freeze<Mock<IPagedDatabaseCommandProvider>>();
        private Mock<IDatabaseCommandProvider<TestEntity>> EntityCommandProviderMock => Fixture.Freeze<Mock<IDatabaseCommandProvider<TestEntity>>>();

        [Fact]
        public void Add_Adds_Entity_To_Database()
        {
            // Arrange
            var commandMock = new Mock<IDatabaseCommand>();
            commandMock.SetupGet(x => x.Operation).Returns(DatabaseOperation.Insert);
            var entity = new TestEntity("01", "Test", "first entity", false);
            CommandProcessorMock.Setup(x => x.ExecuteCommand(commandMock.Object, entity)).Returns(new DatabaseCommandResult<TestEntity>(entity));
            EntityCommandProviderMock.Setup(x => x.Create(It.IsAny<TestEntity>(), DatabaseOperation.Insert)).Returns(commandMock.Object);

            // Act
            var result = Sut.Add(entity);

            // Assert
            result.Should().Be(entity);
            CommandProcessorMock.Verify(x => x.ExecuteCommand(commandMock.Object, entity), Times.Once);
        }

        [Fact]
        public void Can_Update_Entity_To_Repository()
        {
            // Arrange
            var entity = new TestEntity("01", "Test", "first entity", true);
            var commandMock = new Mock<IDatabaseCommand>();
            commandMock.SetupGet(x => x.Operation).Returns(DatabaseOperation.Update);
            CommandProcessorMock.Setup(x => x.ExecuteCommand(commandMock.Object, entity)).Returns(new DatabaseCommandResult<TestEntity>(entity));
            EntityCommandProviderMock.Setup(x => x.Create(It.IsAny<TestEntity>(), DatabaseOperation.Update)).Returns(commandMock.Object);

            // Act
            var result = Sut.Update(entity);

            // Assert
            result.Should().Be(entity);
            CommandProcessorMock.Verify(x => x.ExecuteCommand(commandMock.Object, entity), Times.Once);
        }

        [Fact]
        public void Can_Delete_Entity_To_Repository()
        {
            // Arrange
            var entity = new TestEntity("01", "Test", "first entity", true);
            var commandMock = Fixture.Freeze<Mock<IDatabaseCommand>>();
            commandMock.SetupGet(x => x.Operation).Returns(DatabaseOperation.Delete);
            CommandProcessorMock.Setup(x => x.ExecuteCommand(commandMock.Object, entity)).Returns(new DatabaseCommandResult<TestEntity>(entity));
            EntityCommandProviderMock.Setup(x => x.Create(It.IsAny<TestEntity>(), DatabaseOperation.Delete)).Returns(commandMock.Object);

            // Act
            var result = Sut.Delete(entity);

            // Assert
            result.Should().Be(entity);
            CommandProcessorMock.Verify(x => x.ExecuteCommand(commandMock.Object, entity), Times.Once);
        }

        [Fact]
        public void Can_Find_Entity_Using_Identity()
        {
            // Arrange
            var expected = new TestEntity("code", "codeType", "description");
            IdentitySelectCommandProviderMock.Setup(x => x.Create(It.IsAny<TestEntityIdentity>(), DatabaseOperation.Select)).Returns(new SqlTextCommand("SELECT ...", DatabaseOperation.Select));
            EntityRetrieverMock.Setup(x => x.FindOne(It.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select))).Returns(expected);

            // Act
            var actual = Sut.Find(new TestEntityIdentity(expected));

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void Can_FindAll()
        {
            // Arrange
            var expected = new[] { new TestEntity("code", "codeType", "description") };
            EntitySelectCommandProviderMock.Setup(x => x.Create(DatabaseOperation.Select)).Returns(new SqlTextCommand("SELECT ...", DatabaseOperation.Select));
            EntityRetrieverMock.Setup(x => x.FindMany(It.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select))).Returns(expected);

            // Act
            var actual = Sut.FindAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Can_FindAll_Paged()
        {
            // Arrange
            var expected = new PagedResult<TestEntity>(new[] { new TestEntity("code", "codeType", "description") }, 10, 1, 10);
            PagedEntitySelectCommandProviderMock.Setup(x => x.CreatePaged(DatabaseOperation.Select, 1, 10)).Returns(new PagedDatabaseCommand(new SqlTextCommand("SELECT ...", DatabaseOperation.Select), new SqlTextCommand("SELECT COUNT(*) FROM...", DatabaseOperation.Unspecified), 1, 10));
            EntityRetrieverMock.Setup(x => x.FindPaged(It.Is<IPagedDatabaseCommand>(x => x.DataCommand.Operation == DatabaseOperation.Select))).Returns(expected);

            // Act
            var actual = Sut.FindAllPaged(1, 10);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
