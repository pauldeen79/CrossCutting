namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class UnaryExpressionTests : TestBase
{
    protected IExpression Operand => ClassFactories.GetOrCreate<IExpression>(ClassFactory);

    public class EvaluateAsync : UnaryExpressionTests
    {
        [Fact]
        public async Task Returns_Error_From_Expression()
        {
            // Arrange
            var context = CreateContext("kaboom");
            var sut = new UnaryExpression(context.Expression, Result.Error<IExpression>("Kaboom"));

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Error_From_Expression_Parsing()
        {
            // Arrange
            var context = CreateContext("!kaboom");
            Operand
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Error<object?>("Kaboom"));

            var sut = new UnaryExpression(context.Expression, Result.Success(Operand));

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Wraps_Exception_Into_Error_Result()
        {
            // Arrange
            var exceptionExpression = Substitute.For<IExpression>();
            exceptionExpression
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Throws<InvalidOperationException>();
            var context = CreateContext("error");
            var sut = new UnaryExpression(context.Expression, Result.Success(exceptionExpression));

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
        }
    }

    public class ParseAsync : UnaryExpressionTests
    {
        [Fact]
        public async Task Returns_Error_From_Expression()
        {
            // Arrange
            var context = CreateContext("kaboom");
            var sut = new UnaryExpression(context.Expression, Result.Error<IExpression>("Kaboom"));

            // Act
            var result = await sut.ParseAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(1);
            result.PartResults.First().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Error_From_Expression_Parsing()
        {
            // Arrange
            var context = CreateContext("!kaboom");
            Operand
                .ParseAsync(Arg.Any<CancellationToken>())
                .Returns(new ExpressionParseResultBuilder().WithErrorMessage("Kaboom").WithStatus(ResultStatus.Error));

            var sut = new UnaryExpression(context.Expression, Result.Success(Operand));

            // Act
            var result = await sut.ParseAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(1);
            result.PartResults.First().ErrorMessage.ShouldBe("Kaboom");
        }
    }
}
