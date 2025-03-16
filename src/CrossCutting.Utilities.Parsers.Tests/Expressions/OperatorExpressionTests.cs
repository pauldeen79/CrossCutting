namespace CrossCutting.Utilities.Parsers.Tests.Expressions;

public class OperatorExpressionTests
{
    protected static OperatorExpression CreateSut() => new OperatorExpression();

    protected static ExpressionEvaluatorContext CreateContext(string expression, IExpressionEvaluator evaluator)
        => new ExpressionEvaluatorContext(expression, new ExpressionEvaluatorSettingsBuilder(), null, evaluator);

    public class Evaluate : OperatorExpressionTests
    {
        [Theory]
        [InlineData("1 == 2 == 3")]   // Multiple operators
        [InlineData("==")]            // Only an operator
        [InlineData("1 ==")]          // Missing right operand
        [InlineData("== 2")]          // Missing left operand
        [InlineData(" 1 == ")]        // Trailing spaces, missing right operand
        public void Returns_Invalid_On_Invalid_Expression(string expression)
        {
            // Arrange
            var sut = CreateSut();
            var evaluator = Substitute.For<IExpressionEvaluator>();
            evaluator.Evaluate(Arg.Any<string>(), Arg.Any<ExpressionEvaluatorSettings>(), Arg.Any<object?>())
                     .Returns(Result.NoContent<object?>());
            var context = CreateContext(expression, evaluator);

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Invalid_Left_Operand()
        {
            // Arrange
            var sut = CreateSut();
            var evaluator = Substitute.For<IExpressionEvaluator>();
            evaluator.Evaluate(Arg.Any<string>(), Arg.Any<ExpressionEvaluatorSettings>(), Arg.Any<object?>())
                     .Returns(args => args.ArgAt<object?>(0).ToStringWithDefault() == "left" ? Result.Error<object?>("Kaboom") : Result.NoContent<object?>());
            var context = CreateContext("left == right", evaluator);

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Invalid_Right_Operand()
        {
            // Arrange
            var sut = CreateSut();
            var evaluator = Substitute.For<IExpressionEvaluator>();
            evaluator.Evaluate(Arg.Any<string>(), Arg.Any<ExpressionEvaluatorSettings>(), Arg.Any<object?>())
                     .Returns(args => args.ArgAt<object?>(0).ToStringWithDefault() == "right" ? Result.Error<object?>("Kaboom") : Result.NoContent<object?>());
            var context = CreateContext("left == right", evaluator);

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);

        }
    }

    public class Validate : OperatorExpressionTests
    {
        [Theory]
        [InlineData("1 == 2 == 3")]   // Multiple operators
        [InlineData("==")]            // Only an operator
        [InlineData("1 ==")]          // Missing right operand
        [InlineData("== 2")]          // Missing left operand
        [InlineData(" 1 == ")]        // Trailing spaces, missing right operand
        public void Returns_Invalid_On_Invalid_Expression(string expression)
        {
            // Arrange
            var sut = CreateSut();
            var evaluator = Substitute.For<IExpressionEvaluator>();
            evaluator.Validate(Arg.Any<string>(), Arg.Any<ExpressionEvaluatorSettings>(), Arg.Any<object?>())
                     .Returns(Result.NoContent<Type>());
            var context = CreateContext(expression, evaluator);

            // Act
            var result = sut.Validate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Invalid_Left_Operand()
        {
            // Arrange
            var sut = CreateSut();
            var evaluator = Substitute.For<IExpressionEvaluator>();
            evaluator.Validate(Arg.Any<string>(), Arg.Any<ExpressionEvaluatorSettings>(), Arg.Any<object?>())
                     .Returns(args => args.ArgAt<object?>(0).ToStringWithDefault() == "left" ? Result.Error<Type>("Kaboom") : Result.NoContent<Type>());
            var context = CreateContext("left == right", evaluator);

            // Act
            var result = sut.Validate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Invalid_Right_Operand()
        {
            // Arrange
            var sut = CreateSut();
            var evaluator = Substitute.For<IExpressionEvaluator>();
            evaluator.Validate(Arg.Any<string>(), Arg.Any<ExpressionEvaluatorSettings>(), Arg.Any<object?>())
                     .Returns(args => args.ArgAt<object?>(0).ToStringWithDefault() == "right" ? Result.Error<Type>("Kaboom") : Result.NoContent<Type>());
            var context = CreateContext("left == right", evaluator);

            // Act
            var result = sut.Validate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);

        }
    }
}
