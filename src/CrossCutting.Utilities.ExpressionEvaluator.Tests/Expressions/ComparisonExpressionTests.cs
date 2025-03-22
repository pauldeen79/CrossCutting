namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class ComparisonExpressionTests : TestBase<ComparisonExpression>
{
    protected IExpressionEvaluator Evaluator { get; }

    protected ComparisonExpressionTests()
    {
        Evaluator = Substitute.For<IExpressionEvaluator>();
        Evaluator
            .Evaluate(Arg.Any<string>(), Arg.Any<ExpressionEvaluatorSettings>(), Arg.Any<object?>())
            .Returns(Result.Success<object?>("some result"));
    }

    protected ExpressionEvaluatorContext CreateContext(string expression)
        => new ExpressionEvaluatorContext(expression, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(CultureInfo.InvariantCulture), null, Evaluator);

    public class Evaluate : ComparisonExpressionTests
    {
        [Fact]
        public void Returns_Continue_When_Expression_Does_Not_Contain_Any_Comparison_Characters()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "Some expression not containing comparison characters like equals";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Continue_When_Expression_Contains_Comparison_Character_But_Is_Not_Complete()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "A ==";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Invalid_On_Wrong_Number_Of_Expression_Parts()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "MyField == \"some value\" AND MyField.Length >";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Comparison expression has invalid number of parts");
        }

        [Fact]
        public void Returns_Invalid_On_Malformed_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "MyField AND \"some value\" AND B == C";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Comparison expression is malformed");
        }

        [Fact]
        public void Returns_Success_On_Valid_Single_Part_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "MyField == \"some value\"";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_On_Valid_Multiple_Part_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "MyField == \"some value\" AND MyField.Length > 1";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_On_Valid_Expression_With_Brackets()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "(MyField == \"some value\" AND MyField.Length > 1) OR MyField == \"some other value\"";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
