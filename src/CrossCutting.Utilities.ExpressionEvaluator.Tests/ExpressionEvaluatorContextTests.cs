namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class ExpressionEvaluatorContextTests : TestBase
{
    public class Constructor : ExpressionEvaluatorContextTests
    {
        [Fact]
        public void Replaces_Null_Expression_With_StringEmpty()
        {
            // Arrange
            var expression = default(string);

            // Act
            var sut =  CreateContext(expression!);

            // Assert
            sut.Expression.ShouldBeEmpty();
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
