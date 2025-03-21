namespace CrossCutting.Utilities.Parsers.Tests;

public class VariableEvaluatorTests
{
    public class Evaluate : VariableEvaluatorTests
    {
        [Fact]
        public void Returns_Result_When_Variable_Is_Not_Unknown()
        {
            // Arrange
            var myVariable = Substitute.For<IVariable>();
            myVariable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.Success<object?>("result value"));
            var sut = new VariableEvaluator([myVariable]);

            // Act
            var result = sut.Evaluate("myVariable", null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("result value");
        }

        [Fact]
        public void Returns_Invalid_When_Variable_Is_Unknown()
        {
            // Arrange
            var sut = new VariableEvaluator([]);

            // Act
            var result = sut.Evaluate("myVariable", null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown variable found: myVariable");
        }

        [Fact]
        public void Returns_Invalid_When_Variable_Is_Empty()
        {
            // Arrange
            var sut = new VariableEvaluator([]);

            // Act
            var result = sut.Evaluate(string.Empty, null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Variable is required");
        }
    }

    public class Validate : VariableEvaluatorTests
    {
        [Fact]
        public void Returns_Result_When_Variable_Is_Not_Unknown()
        {
            // Arrange
            var myVariable = Substitute.For<IVariable>();
            myVariable.Validate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.Success(typeof(string)));
            var sut = new VariableEvaluator([myVariable]);

            // Act
            var result = sut.Validate("myVariable", null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Invalid_When_Variable_Is_Unknown()
        {
            // Arrange
            var sut = new VariableEvaluator([]);

            // Act
            var result = sut.Validate("myVariable", null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown variable found: myVariable");
        }

        [Fact]
        public void Returns_Invalid_When_Variable_Is_Empty()
        {
            // Arrange
            var sut = new VariableEvaluator([]);

            // Act
            var result = sut.Validate(string.Empty, null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Variable is required");
        }
    }
}
