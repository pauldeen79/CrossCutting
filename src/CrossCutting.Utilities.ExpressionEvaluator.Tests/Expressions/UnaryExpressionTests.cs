namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class UnaryExpressionTests : TestBase
{
    protected IExpression Operand { get; }

    public UnaryExpressionTests()
    {
        Operand = Substitute.For<IExpression>();
    }

    public class Evaluate : UnaryExpressionTests
    {
        [Fact]
        public void Returns_Error_From_Expression()
        {
            // Arrange
            var sut = new UnaryExpression(Result.Error<IExpression>("Kaboom"));

            // Act
            var result = sut.Evaluate(CreateContext("kaboom"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Error_From_Expression_Parsing()
        {
            // Arrange
            Operand
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Error<object?>("Kaboom"));

            var sut = new UnaryExpression(Result.Success(Operand));

            // Act
            var result = sut.Evaluate(CreateContext("!kaboom"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class Parse : UnaryExpressionTests
    {
        [Fact]
        public void Returns_Error_From_Expression()
        {
            // Arrange
            var sut = new UnaryExpression(Result.Error<IExpression>("Kaboom"));

            // Act
            var result = sut.Parse(CreateContext("kaboom"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(1);
            result.PartResults.First().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Error_From_Expression_Parsing()
        {
            // Arrange
            Operand
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithErrorMessage("Kaboom").WithStatus(ResultStatus.Error));

            var sut = new UnaryExpression(Result.Success(Operand));

            // Act
            var result = sut.Parse(CreateContext("!kaboom"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(1);
            result.PartResults.First().ErrorMessage.ShouldBe("Kaboom");
        }
    }
}
