namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class ComparisonExpressionTests : TestBase<ComparisonExpression>
{
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
            var expression = "context == \"some value\" AND context.Length >";

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
            var expression = "context AND \"some value\" AND B == C";

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
            var expression = "context == \"some value\"";

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
            var expression = "context == \"some value\" AND context.Length > 1";

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
            var expression = "(context >= 1 AND context <= 5) OR context == \"some other value\"";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
