namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class ExpressionEvaluatorTests : TestBase
{
    protected ExpressionEvaluator CreateSut() => new ExpressionEvaluator([Expression]);

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
            Expression.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("result value");
        }

        [Fact]
        public void Returns_Invalid_When_Expression_Is_Not_Understood()
        {
            // Arrange
            Expression.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Continue<object?>());
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: expression");
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
            Expression.Evaluate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = sut.EvaluateTyped<string>("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("result value");
        }

        [Fact]
        public void Returns_First_Understood_TypedResult_When_Not_Equal_To_Continue()
        {
            // Arrange
            var typedExpression = Substitute.For<IExpression<string>>();
            typedExpression.EvaluateTyped(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success<string>("result value"));
            var sut = new ExpressionEvaluator([typedExpression]);

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
            var typedExpression = Substitute.For<IExpression<string>>();
            typedExpression.EvaluateTyped(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Continue<string>());
            var sut = new ExpressionEvaluator([typedExpression]);

            // Act
            var result = sut.EvaluateTyped<string>("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: expression");
        }
    }

    public class Validate : ExpressionEvaluatorTests
    {
        [Fact]
        public void Returns_Invalid_When_Expression_Is_Null_Or_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Validate(string.Empty, new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Value is required");
        }

        [Fact]
        public void Returns_First_Understood_Result_When_Not_Equal_To_Continue()
        {
            // Arrange
            // Note that this setup simulates the implementation of ComparisonExpression
            Expression.Validate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Success(typeof(bool)));
            var sut = CreateSut();

            // Act
            var result = sut.Validate("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(bool));
        }

        [Fact]
        public void Returns_Invalid_When_Expression_Is_Not_Understood()
        {
            // Arrange
            Expression.Validate(Arg.Any<ExpressionEvaluatorContext>()).Returns(Result.Continue<Type>());
            var sut = CreateSut();

            // Act
            var result = sut.Validate("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: expression");
        }
    }
}
