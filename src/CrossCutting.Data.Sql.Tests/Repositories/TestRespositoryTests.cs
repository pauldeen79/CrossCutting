using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRespositoryTests
    {
        private TestRepository Sut { get; }
        private Mock<IDatabaseCommandProcessor<TestEntity>> AddProcessorMock { get; }
        private Mock<IDatabaseCommandProcessor<TestEntity>> UpdateProcessorMock { get; }
        private Mock<IDatabaseCommandProcessor<TestEntity>> DeleteProcessorMock { get; }

        public TestRespositoryTests()
        {
            AddProcessorMock = new Mock<IDatabaseCommandProcessor<TestEntity>>();
            UpdateProcessorMock = new Mock<IDatabaseCommandProcessor<TestEntity>>();
            DeleteProcessorMock = new Mock<IDatabaseCommandProcessor<TestEntity>>();
            Sut = new TestRepository(AddProcessorMock.Object, UpdateProcessorMock.Object, DeleteProcessorMock.Object);
        }

        [Fact]
        public void Can_Add_Entity_To_Repository()
        {
            // Arrange
            var input = new TestEntity("01", "Test", "first entity", false);
            AddProcessorMock.Setup(x => x.InvokeCommand(input)).Returns(input);

            // Act
            var result = Sut.Add(input);

            // Assert
            result.Should().Be(input);
            AddProcessorMock.Verify(x => x.InvokeCommand(input), Times.Once);
        }

        [Fact]
        public void Can_Update_Entity_To_Repository()
        {
            // Arrange
            var input = new TestEntity("01", "Test", "first entity", true);
            UpdateProcessorMock.Setup(x => x.InvokeCommand(input)).Returns(input);

            // Act
            var result = Sut.Update(input);

            // Assert
            result.Should().Be(input);
            UpdateProcessorMock.Verify(x => x.InvokeCommand(input), Times.Once);
        }

        [Fact]
        public void Can_Delete_Entity_To_Repository()
        {
            // Arrange
            var input = new TestEntity("01", "Test", "first entity", true);
            DeleteProcessorMock.Setup(x => x.InvokeCommand(input)).Returns(input);

            // Act
            var result = Sut.Delete(input);

            // Assert
            result.Should().Be(input);
            DeleteProcessorMock.Verify(x => x.InvokeCommand(input), Times.Once);
        }
    }
}
