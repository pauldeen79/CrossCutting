namespace CrossCutting.Utilities.Parsers.Tests;

public class VariableProcessorTests
{
    public class Process : VariableProcessorTests
    {
        [Fact]
        public void Returns_Result_When_Variable_Is_Not_Unknown()
        {
            // Arrange
            var myVariable = Substitute.For<IVariable>();
            myVariable.Process(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.Success<object?>("result value"));
            var sut = new VariableProcessor([myVariable]);

            // Act
            var result = sut.Process("myVariable", null);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("result value");
        }

        [Fact]
        public void Returns_NotSupported_When_Variable_Is_Unknown()
        {
            // Arrange
            var sut = new VariableProcessor([]);

            // Act
            var result = sut.Process("myVariable", null);

            // Assert
            result.Status.Should().Be(ResultStatus.NotSupported);
            result.ErrorMessage.Should().Be("Unknown variable found: myVariable");
        }

        [Fact]
        public void Returns_Invalid_When_Variable_Is_Unknown()
        {
            // Arrange
            var sut = new VariableProcessor([]);

            // Act
            var result = sut.Process(string.Empty, null);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Variable is required");
        }
    }
}
