namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class ExpressionEvaluatorContextTests : TestBase
{
    public class Constructor : ExpressionEvaluatorContextTests
    {
        [Fact]
        public void Throws_On_Null_Expression()
        {
            // Arrange
            Action a = () => CreateContext(expression: null!);

            // Assert
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("expression");
        }

        [Fact]
        public void Fills_QuoteMap_When_Quotes_Are_Present()
        {
            // Arrange
            var expression = "\"Some \" + \"quoted \" + \"expression\"";

            // Act
            var sut = CreateContext(expression);

            // Assert
            sut.QuoteMap.Count().ShouldBe(3);
        }

        [Fact]
        public void Leaves_QuoteMap_Empty_When_Quotes_Are_Not_Present()
        {
            // Arrange
            var expression = "SomeUnquotedExpression()";

            // Act
            var sut = CreateContext(expression);

            // Assert
            sut.QuoteMap.Count().ShouldBe(0);
        }

        [Fact]
        public void Trims_The_Expression()
        {
            // Arrange
            var expression = "    SomeExpression()    ";

            // Act
            var sut = CreateContext(expression);

            // Assert
            sut.Expression.ShouldBe("SomeExpression()");
        }
    }
}
