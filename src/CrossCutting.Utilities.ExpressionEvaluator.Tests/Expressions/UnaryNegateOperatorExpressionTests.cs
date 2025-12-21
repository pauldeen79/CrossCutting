namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class UnaryNegateOperatorExpressionTests : TestBase
{
    protected IExpression Operand => ClassFactories.GetOrCreate<IExpression>(ClassFactory);

    public class EvaluateAsync : UnaryNegateOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Error_From_Expression()
        {
            // Arrange
            var context = CreateContext("kaboom");
            var sut = new UnaryNegateOperatorExpression(Result.Error<IExpression>("Kaboom"), context.Expression);

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

            var sut = new UnaryNegateOperatorExpression(Result.Success(Operand), context.Expression);

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
            var sut = new UnaryNegateOperatorExpression(Result.Success(exceptionExpression), context.Expression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
        }
    }

    public class EvaluateTypedAsync : UnaryNegateOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Error_From_Expression()
        {
            // Arrange
            var context = CreateContext("kaboom");
            var sut = new UnaryNegateOperatorExpression(Result.Error<IExpression>("Kaboom"), context.Expression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Success_From_Expression()
        {
            // Arrange
            var context = CreateContext("true");
            var sut = new UnaryNegateOperatorExpression(Result.Success<IExpression>(new OtherExpression("true")), context.Expression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeFalse();
        }
        
        [Fact]
        public async Task Returns_Error_From_Expression_Parsing()
        {
            // Arrange
            var context = CreateContext("!kaboom");
            Operand
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Error<object?>("Kaboom"));

            var sut = new UnaryNegateOperatorExpression(Result.Success(Operand), context.Expression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

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
            var sut = new UnaryNegateOperatorExpression(Result.Success(exceptionExpression), context.Expression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
        }
    }

    public class ParseAsync : UnaryNegateOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Error_From_Expression()
        {
            // Arrange
            var context = CreateContext("kaboom");
            var sut = new UnaryNegateOperatorExpression(Result.Error<IExpression>("Kaboom"), context.Expression);

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(1);
            result.PartResults.First().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Success_From_Expression()
        {
            // Arrange
            var context = CreateContext("!true");
            var sut = new UnaryNegateOperatorExpression(Result.Success<IExpression>(new OtherExpression("!true")), context.Expression);

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(UnaryNegateOperatorExpression));
            result.PartResults.Count.ShouldBe(1);
            result.PartResults.First().Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Error_From_Expression_Parsing()
        {
            // Arrange
            var context = CreateContext("!kaboom");
            Operand
                .ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(new ExpressionParseResultBuilder().WithErrorMessage("Kaboom").WithStatus(ResultStatus.Error));

            var sut = new UnaryNegateOperatorExpression(Result.Success(Operand), context.Expression);

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(1);
            result.PartResults.First().ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class ToBuilder : UnaryNegateOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_AddOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var context = CreateContext("kaboom");
            var sut = new UnaryNegateOperatorExpression(Result.Error<IExpression>("Kaboom"), context.Expression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<UnaryNegateOperatorEvaluatableBuilder>();
            var equalOperatorEvaluatableBuilder = (UnaryNegateOperatorEvaluatableBuilder)result;
            equalOperatorEvaluatableBuilder.Operand.ShouldNotBeNull();
            var evaluateResult = await equalOperatorEvaluatableBuilder.Operand.EvaluateAsync(context, CancellationToken.None);
            evaluateResult.Status.ShouldBe(ResultStatus.Error);
            evaluateResult.ErrorMessage.ShouldBe("Kaboom");
        }
    }
}
