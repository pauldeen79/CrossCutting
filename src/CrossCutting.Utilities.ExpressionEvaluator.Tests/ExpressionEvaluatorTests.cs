namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class ExpressionEvaluatorTests : TestBase
{
    protected ExpressionEvaluator CreateSut() => new ExpressionEvaluator(new 
        OperatorExpressionTokenizer(), new OperatorExpressionParser(), [Expression]);

    public class Evaluate : ExpressionEvaluatorTests
    {
        [Fact]
        public void Returns_Invalid_When_Expression_Is_Null_Or_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(string.Empty, new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Value is required");
        }

        [Fact]
        public void Returns_First_Understood_Result_When_Not_Equal_To_Continue()
        {
            // Arrange
            Expression.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate("expression", new ExpressionEvaluatorSettingsBuilder(), null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("result value");
        }

        [Fact]
        public void Returns_Invalid_When_Expression_Is_Not_Understood()
        {
            // Arrange
            Expression.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Continue<object?>());
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: expression");
        }

        [Fact]
        public void Returns_Invalid_When_Maximum_Recursion_Has_Been_Reached()
        {
            // Arrange
            Expression.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(new ExpressionEvaluatorContext("expression", new ExpressionEvaluatorSettingsBuilder(), null, sut, int.MaxValue));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Maximum recursion level has been reached");
        }
    }

    public class EvaluateTyped : ExpressionEvaluatorTests
    {
        [Fact]
        public void Returns_Invalid_When_Expression_Is_Null_Or_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.EvaluateTyped<string>(string.Empty, new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Value is required");
        }

        [Fact]
        public void Returns_First_Understood_Result_When_Not_Equal_To_Continue()
        {
            // Arrange
            Expression.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = sut.EvaluateTyped<string>("expression", new ExpressionEvaluatorSettingsBuilder(), null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("result value");
        }

        [Fact]
        public void Returns_First_Understood_TypedResult_When_Not_Equal_To_Continue()
        {
            // Arrange
            Expression.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = sut.EvaluateTyped<string>("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("result value");
        }

        [Fact]
        public void Returns_Invalid_When_Expression_Is_Not_Understood()
        {
            // Arrange
            Expression.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Continue<object?>());
            var sut = CreateSut();

            // Act
            var result = sut.EvaluateTyped<string>("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: expression");
        }

        [Fact]
        public void Returns_Invalid_When_Maximum_Recursion_Has_Been_Reached()
        {
            // Arrange
            Expression.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = sut.EvaluateTyped<string>(new ExpressionEvaluatorContext("expression", new ExpressionEvaluatorSettingsBuilder(), null, sut, int.MaxValue));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Maximum recursion level has been reached");
        }
    }

    public class Parse : ExpressionEvaluatorTests
    {
        [Fact]
        public void Returns_Invalid_When_Expression_Is_Null_Or_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Parse(string.Empty, new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Value is required");
        }

        [Fact]
        public void Returns_First_Understood_Result_When_Not_Equal_To_Continue()
        {
            // Arrange
            // Note that this setup simulates the implementation of ComparisonExpression
            Expression.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithSourceExpression("Dummy").WithStatus(ResultStatus.Ok).WithResultType(typeof(bool)));
            var sut = CreateSut();

            // Act
            var result = sut.Parse("expression", new ExpressionEvaluatorSettingsBuilder(), null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
        }

        [Fact]
        public void Returns_Invalid_When_Expression_Is_Not_Understood()
        {
            // Arrange
            Expression.Parse(Arg.Any<ExpressionEvaluatorContext>()).Returns(new ExpressionParseResultBuilder().WithSourceExpression("Dummy").WithStatus(ResultStatus.Continue));
            var sut = CreateSut();

            // Act
            var result = sut.Parse("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: expression");
        }

        [Fact]
        public void Returns_Invalid_When_Maximum_Recursion_Has_Been_Reached()
        {
            // Arrange
            Expression.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = sut.Parse(new ExpressionEvaluatorContext("expression", new ExpressionEvaluatorSettingsBuilder(), null, sut, int.MaxValue));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Maximum recursion level has been reached");
        }
    }
}
