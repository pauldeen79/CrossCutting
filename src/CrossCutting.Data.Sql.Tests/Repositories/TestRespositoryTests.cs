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
        private Mock<IDatabaseCommandProvider<TestEntityIdentity>> CommandProviderMock { get; }

        public TestRespositoryTests()
        {
            CommandProcessorMock = new Mock<IDatabaseCommandProcessor<TestEntity>>();
            EntityRetrieverMock = new Mock<IDatabaseEntityRetriever<TestEntity>>();
            CommandProviderMock = new Mock<IDatabaseCommandProvider<TestEntityIdentity>>();
            Sut = new TestRepository(CommandProcessorMock.Object,
                                     EntityRetrieverMock.Object,
                                     CommandProviderMock.Object);
        }

        [Fact]
        public void Can_Add_Entity_To_Repository()
        {
            // Arrange
            var input = new TestEntity("01", "Test", "first entity", false);
            CommandProcessorMock.Setup(x => x.InvokeCommand(input, DatabaseOperation.Insert)).Returns(new DatabaseCommandResult<TestEntity>(input));

            // Act
            var result = Sut.Add(input);

            // Assert
            result.Should().Be(input);
            CommandProcessorMock.Verify(x => x.InvokeCommand(input, DatabaseOperation.Insert), Times.Once);
        }

        [Fact]
        public void Can_Update_Entity_To_Repository()
        {
            // Arrange
            var input = new TestEntity("01", "Test", "first entity", true);
            CommandProcessorMock.Setup(x => x.InvokeCommand(input, DatabaseOperation.Update)).Returns(new DatabaseCommandResult<TestEntity>(input));

            // Act
            var result = Sut.Update(input);

            // Assert
            result.Should().Be(input);
            CommandProcessorMock.Verify(x => x.InvokeCommand(input, DatabaseOperation.Update), Times.Once);
        }

        [Fact]
        public void Can_Delete_Entity_To_Repository()
        {
            // Arrange
            var input = new TestEntity("01", "Test", "first entity", true);
            CommandProcessorMock.Setup(x => x.InvokeCommand(input, DatabaseOperation.Delete)).Returns(new DatabaseCommandResult<TestEntity>(input));

            // Act
            var result = Sut.Delete(input);

            // Assert
            result.Should().Be(input);
            CommandProcessorMock.Verify(x => x.InvokeCommand(input, DatabaseOperation.Delete), Times.Once);
        }

        [Fact]
        public void Can_Find_Entity_Using_Identity()
        {
            // Arrange
            var expected = new TestEntity("code", "codeType", "description");
            CommandProviderMock.Setup(x => x.Create(It.IsAny<TestEntityIdentity>(), DatabaseOperation.Select)).Returns(new SqlTextCommand("SELECT ...", DatabaseOperation.Select));
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
    }
}
