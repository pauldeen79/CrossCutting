namespace CrossCutting.Utilities.Parsers.Tests;

public class VariableProcessorTests
{
    public class Evaluate : VariableProcessorTests
    {
        [Fact]
        public void Returns_Result_When_Variable_Is_Not_Unknown()
        {
            // Arrange
            var myVariable = Substitute.For<IVariable>();
            myVariable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.Success<object?>("result value"));
            var sut = new VariableProcessor([myVariable]);

            // Act
            var result = sut.Evaluate("myVariable", null);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("result value");
        }

        [Fact]
        public void Returns_Invalid_When_Variable_Is_Unknown()
        {
            // Arrange
            var sut = new VariableProcessor([]);

            // Act
            var result = sut.Evaluate("myVariable", null);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Unknown variable found: myVariable");
        }

        [Fact]
        public void Returns_Invalid_When_Variable_Is_Empty()
        {
            // Arrange
            var sut = new VariableProcessor([]);

            // Act
            var result = sut.Evaluate(string.Empty, null);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Variable is required");
        }
    }

    public class Validate : VariableProcessorTests
    {
        [Fact]
        public void Returns_Result_When_Variable_Is_Not_Unknown()
        {
            // Arrange
            var myVariable = Substitute.For<IVariable>();
            myVariable.Validate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.Success<object?>("result value"));
            var sut = new VariableProcessor([myVariable]);

            // Act
            var result = sut.Validate("myVariable", null);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Invalid_When_Variable_Is_Unknown()
        {
            // Arrange
            var sut = new VariableProcessor([]);

            // Act
            var result = sut.Validate("myVariable", null);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Unknown variable found: myVariable");
        }

        [Fact]
        public void Returns_Invalid_When_Variable_Is_Empty()
        {
            // Arrange
            var sut = new VariableProcessor([]);

            // Act
            var result = sut.Validate(string.Empty, null);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Variable is required");
        }
    }
}
