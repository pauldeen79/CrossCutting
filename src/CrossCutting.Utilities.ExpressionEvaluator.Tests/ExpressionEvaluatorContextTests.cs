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
        public void Trims_The_Expression()
        {
            // Arrange
            var expression = "    SomeExpression()    ";

            // Act
            var sut = CreateContext(expression);

            // Assert
            sut.Expression.ShouldBe("SomeExpression()");
        }

        [Fact]
        public void Sets_ParentContext_Correctly()
        {
            // Arrange
            var expression = "child expression";

            // Act
            var sut = CreateContext(expression, currentRecursionLevel: 2, parentContext: new ExpressionEvaluatorContext("parent expression", new ExpressionEvaluatorSettingsBuilder(), null, Evaluator, currentRecursionLevel: 1));

            // Assert
            sut.CurrentRecursionLevel.ShouldBe(2);
            sut.Expression.ShouldBe("child expression");

            sut.ParentContext.ShouldNotBeNull();
            sut.ParentContext.Expression.ShouldBe("parent expression");
            sut.ParentContext.CurrentRecursionLevel.ShouldBe(1);
        }
    }
}
