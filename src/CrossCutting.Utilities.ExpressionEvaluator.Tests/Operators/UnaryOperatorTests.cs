namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Operands;

public class UnaryOperatorTests : TestBase
{
    protected IOperator Operand { get; }

    public UnaryOperatorTests()
    {
        Operand = Substitute.For<IOperator>();
    }

    public class Evaluate : UnaryOperatorTests
    {
        [Fact]
        public void Returns_Error_From_Expression()
        {
            // Arrange
            var sut = new UnaryOperator(Result.Error<IOperator>("Kaboom"));

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

            var sut = new UnaryOperator(Result.Success(Operand));

            // Act
            var result = sut.Evaluate(CreateContext("!kaboom"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class Parse : UnaryOperatorTests
    {
        [Fact]
        public void Returns_Error_From_Expression()
        {
            // Arrange
            var sut = new UnaryOperator(Result.Error<IOperator>("Kaboom"));

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

            var sut = new UnaryOperator(Result.Success(Operand));

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
