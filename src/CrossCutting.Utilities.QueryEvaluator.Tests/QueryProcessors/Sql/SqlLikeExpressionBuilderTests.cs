namespace CrossCutting.Utilities.QueryEvaluator.Tests.QueryProcessors.Sql;

public class SqlLikeExpressionBuilderTests
{
    public class DefaultConstructor : SqlLikeExpressionBuilderTests
    {
        [Fact]
        public void Constructs_Correctly()
        {
            // Act
            var sut = new SqlLikeExpressionBuilder();

            // Assert
            sut.FormatString.ShouldBe("{0}");
            sut.SourceExpression.ShouldBeNull();
        }
    }

    public class FullArgumentsConstructor : SqlLikeExpressionBuilderTests
    {
        [Fact]
        public void Constructs_Correctly()
        {
            // Arrange
            var expression = new ContextExpression();
            var formatString = "some {0} custom format string";

            // Act
            var sut = new SqlLikeExpressionBuilder(expression, formatString);

            // Assert
            sut.FormatString.ShouldBe(formatString);
            sut.SourceExpression.ShouldBeSameAs(expression);
        }
    }

    public class Build : SqlLikeExpressionBuilderTests
    {
        [Fact]
        public void Creates_Expression_Correctly()
        {
            // Arrange
            var expression = new ContextExpression();
            var formatString = "some {0} custom format string";
            var sut = new SqlLikeExpressionBuilder(expression, formatString);

            // Act
            var result = sut.Build();

            // Assert
            result.ShouldBeOfType<SqlLikeExpression>();
            ((SqlLikeExpression)result).FormatString.ShouldBeSameAs(formatString);
            ((SqlLikeExpression)result).SourceExpression.ShouldBeSameAs(expression);
        }
    }

    public class WithSourceExpression : SqlLikeExpressionBuilderTests
    {
        [Fact]
        public void Sets_SourceExpression_Correctly()
        {
            // Arrange
            var expression = new ContextExpression();
            var sut = new SqlLikeExpressionBuilder();

            // Act
            var result = sut.WithSourceExpression(expression);

            // Assert
            result.ShouldBeSameAs(sut);
            result.SourceExpression.ShouldBe(expression);
        }
    }

    public class WithFormatString : SqlLikeExpressionBuilderTests
    {
        [Fact]
        public void Sets_FormatString_Correctly()
        {
            // Arrange
            var formatString = "some {0} custom format string";
            var sut = new SqlLikeExpressionBuilder();

            // Act
            var result = sut.WithFormatString(formatString);

            // Assert
            result.ShouldBeSameAs(sut);
            result.FormatString.ShouldBe(formatString);
        }
    }
}
