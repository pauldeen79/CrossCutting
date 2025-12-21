namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class BinaryOrOperatorExpressionTests : TestBase
{
    public class ToBuilder : BinaryOrOperatorExpressionTests
    {
        [Fact]
        public void Returns_BinaryOrOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "true || false";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryOrOperatorExpression(Result.Success<IExpression>(new OtherExpression("true")), Result.Success<IExpression>(new OtherExpression("false")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<BinaryOrOperatorEvaluatableBuilder>();
            var BinaryOrOperatorEvaluatableBuilder = (BinaryOrOperatorEvaluatableBuilder)result;
            BinaryOrOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)BinaryOrOperatorEvaluatableBuilder.LeftOperand).SourceExpression.ShouldBe("true");
            BinaryOrOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)BinaryOrOperatorEvaluatableBuilder.RightOperand).SourceExpression.ShouldBe("false");
        }
    }

    public class EvaluateAsync : BinaryOrOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result_On_Valid_Operands()
        {
            // Arrange
            var sourceExpression = "true || false";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryOrOperatorExpression(Result.Success<IExpression>(new OtherExpression("true")), Result.Success<IExpression>(new OtherExpression("false")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }
    }

    public class EvaluateTypedAsync : BinaryOrOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result_On_Valid_Operands()
        {
            // Arrange
            var sourceExpression = "true || false";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryOrOperatorExpression(Result.Success<IExpression>(new OtherExpression("true")), Result.Success<IExpression>(new OtherExpression("false")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeTrue();
        }

        [Fact]
        public async Task Returns_Correct_Result_On_Invalid_Left_Operand()
        {
            // Arrange
            var sourceExpression = "1 || true";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryOrOperatorExpression(Result.Success<IExpression>(new OtherExpression("1")), Result.Success<IExpression>(new OtherExpression("true")), sourceExpression);

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
            var sourceExpression = "true || 1";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryOrOperatorExpression(Result.Success<IExpression>(new OtherExpression("true")), Result.Success<IExpression>(new OtherExpression("1")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Right operand should be of type boolean");
        }
    }

    public class ParseAsync : BinaryOrOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "true || false";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryOrOperatorExpression(Result.Success<IExpression>(new OtherExpression("true")), Result.Success<IExpression>(new OtherExpression("false")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(BinaryOrOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
