using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.Commands;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRespositoryTests
    {
        private TestRepository Sut { get; }
        private Mock<IDatabaseCommandProcessor<TestEntity>> CommandProcessorMock { get; }
        private Mock<IDatabaseEntityRetriever<TestEntity>> EntityRetrieverMock { get; }
        private Mock<IDatabaseCommandProvider<TestEntityIdentity>> IdentityCommandProviderMock { get; }
        private Mock<IDatabaseCommandProvider<TestEntity>> EntityCommandProviderMock { get; }

        public TestRespositoryTests()
        {
            CommandProcessorMock = new Mock<IDatabaseCommandProcessor<TestEntity>>();
            EntityRetrieverMock = new Mock<IDatabaseEntityRetriever<TestEntity>>();
            IdentityCommandProviderMock = new Mock<IDatabaseCommandProvider<TestEntityIdentity>>();
            EntityCommandProviderMock = new Mock<IDatabaseCommandProvider<TestEntity>>();
            Sut = new TestRepository(CommandProcessorMock.Object,
                                     EntityRetrieverMock.Object,
                                     IdentityCommandProviderMock.Object,
                                     EntityCommandProviderMock.Object);
        }

        [Fact]
        public void Can_Add_Entity_To_Repository()
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
            var commandMock = new Mock<IDatabaseCommand>();
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
            var actual = Sut.FindMany();

            // Assert
            actual.Should().BeSameAs(expected);
        }

        [Fact]
        public void Can_FindAll()
        {
            // Arrange
            var expected = new[] { new TestEntity("code", "codeType", "description") };
            EntityCommandProviderMock.Setup(x => x.Create(DatabaseOperation.Select)).Returns(new SqlTextCommand("SELECT ...", DatabaseOperation.Select));
            IdentityCommandProviderMock.Setup(x => x.Create(DatabaseOperation.Select)).Returns(new SqlTextCommand("SELECT ...", DatabaseOperation.Select));
            EntityRetrieverMock.Setup(x => x.FindMany(It.Is<IDatabaseCommand>(x => x.Operation == DatabaseOperation.Select))).Returns(expected);

            // Act
            var actual = Sut.FindAll();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
