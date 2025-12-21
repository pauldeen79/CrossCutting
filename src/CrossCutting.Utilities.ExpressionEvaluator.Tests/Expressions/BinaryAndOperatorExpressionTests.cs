namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class BinaryAndOperatorExpressionTests : TestBase
{
    public class ToBuilder : BinaryAndOperatorExpressionTests
    {
        [Fact]
        public void Returns_Builder_Correctly_On_Successful_Expressions()
        {
            // Arrange
            var sourceExpression = "true && true";
            var sut = new BinaryAndOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("true")), Result.Success<IExpression>(new EvaluatableExpression("true")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<BinaryAndOperatorEvaluatableBuilder>();
            var binaryAndOperatorEvaluatableBuilder = (BinaryAndOperatorEvaluatableBuilder)result;
            binaryAndOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<EvaluatableExpression>();
            ((EvaluatableExpression)binaryAndOperatorEvaluatableBuilder.LeftOperand).SourceExpression.ShouldBe("true");
            binaryAndOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<EvaluatableExpression>();
            ((EvaluatableExpression)binaryAndOperatorEvaluatableBuilder.RightOperand).SourceExpression.ShouldBe("true");
        }

        [Fact]
        public void Returns_Builder_Correctly_On_Error_Expressions()
        {
            // Arrange
            var sourceExpression = "x && y";
            var sut = new BinaryAndOperatorExpression(Result.Error<IExpression>("x is unknown"), Result.Error<IExpression>("y is unknown"), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<BinaryAndOperatorEvaluatableBuilder>();
            var binaryAndOperatorEvaluatableBuilder = (BinaryAndOperatorEvaluatableBuilder)result;
            binaryAndOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<LiteralResultEvaluatable>();
            ((LiteralResultEvaluatable)binaryAndOperatorEvaluatableBuilder.LeftOperand).Value.Status.ShouldBe(ResultStatus.Error);
            ((LiteralResultEvaluatable)binaryAndOperatorEvaluatableBuilder.LeftOperand).Value.ErrorMessage.ShouldBe("x is unknown");
            binaryAndOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<LiteralResultEvaluatable>();
            ((LiteralResultEvaluatable)binaryAndOperatorEvaluatableBuilder.RightOperand).Value.Status.ShouldBe(ResultStatus.Error);
            ((LiteralResultEvaluatable)binaryAndOperatorEvaluatableBuilder.RightOperand).Value.ErrorMessage.ShouldBe("y is unknown");
        }
    }

    public class EvaluateAsync : BinaryAndOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result_On_Valid_Operands()
        {
            // Arrange
            var sourceExpression = "true && true";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryAndOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("true")), Result.Success<IExpression>(new EvaluatableExpression("true")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }
    }

    public class EvaluateTypedAsync : BinaryAndOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result_On_Valid_Operands()
        {
            // Arrange
            var sourceExpression = "true && false";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryAndOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("true")), Result.Success<IExpression>(new EvaluatableExpression("false")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeFalse();
        }

        [Fact]
        public async Task Returns_Correct_Result_On_Invalid_Left_Operand()
        {
            // Arrange
            var sourceExpression = "1 && true";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryAndOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("1")), Result.Success<IExpression>(new EvaluatableExpression("true")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Left operand should be of type boolean");
        }

        [Fact]
        public async Task Returns_Correct_Result_On_Invalid_Right_Operand()
        {
            // Arrange
            var sourceExpression = "true && 1";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryAndOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("true")), Result.Success<IExpression>(new EvaluatableExpression("1")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Right operand should be of type boolean");
        }
    }

    public class ParseAsync : BinaryAndOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "true && true";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryAndOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("true")), Result.Success<IExpression>(new EvaluatableExpression("true")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(BinaryAndOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
