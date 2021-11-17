using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
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
        private Mock<IDatabaseEntityRetriever<TestEntity>> RetrieverMock { get; }

        public TestRespositoryTests()
        {
            CommandProcessorMock = new Mock<IDatabaseCommandProcessor<TestEntity>>();
            RetrieverMock = new Mock<IDatabaseEntityRetriever<TestEntity>>();
            Sut = new TestRepository(CommandProcessorMock.Object,
                                     RetrieverMock.Object);
        }

        [Fact]
        public void Can_Add_Entity_To_Repository()
        {
            // Arrange
            var input = new TestEntity("01", "Test", "first entity", false);
            CommandProcessorMock.Setup(x => x.InvokeCommand(input)).Returns(new DatabaseCommandResult<TestEntity>(input));

            // Act
            var result = Sut.Add(input);

            // Assert
            result.Should().Be(input);
            CommandProcessorMock.Verify(x => x.InvokeCommand(input), Times.Once);
        }

        [Fact]
        public void Can_Update_Entity_To_Repository()
        {
            // Arrange
            var input = new TestEntity("01", "Test", "first entity", true);
            CommandProcessorMock.Setup(x => x.InvokeCommand(input)).Returns(new DatabaseCommandResult<TestEntity>(input));

            // Act
            var result = Sut.Update(input);

            // Assert
            result.Should().Be(input);
            CommandProcessorMock.Verify(x => x.InvokeCommand(input), Times.Once);
        }

        [Fact]
        public void Can_Delete_Entity_To_Repository()
        {
            // Arrange
            var input = new TestEntity("01", "Test", "first entity", true);
            CommandProcessorMock.Setup(x => x.InvokeCommand(input)).Returns(new DatabaseCommandResult<TestEntity>(input));

            // Act
            var result = Sut.Delete(input);

            // Assert
            result.Should().Be(input);
            CommandProcessorMock.Verify(x => x.InvokeCommand(input), Times.Once);
        }

        [Fact]
        public void Can_FindOne_Entity_Using_Repository()
        {
            // Arrange
            var expected = new TestEntity("code", "codeType", "description");
            RetrieverMock.Setup(x => x.FindOne(It.IsAny<IDatabaseCommand>())).Returns(expected);

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
            RetrieverMock.Setup(x => x.FindMany(It.IsAny<IDatabaseCommand>())).Returns(expected);

            // Act
            var actual = Sut.FindMany();

            // Assert
            actual.Should().BeSameAs(expected);
        }
    }
}
