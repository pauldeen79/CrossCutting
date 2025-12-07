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
            var sut = new BinaryOrOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "true")), Result.Success<IExpression>(new OtherExpression(context, "false")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<BinaryOrOperatorEvaluatableBuilder>();
            var BinaryOrOperatorEvaluatableBuilder = (BinaryOrOperatorEvaluatableBuilder)result;
            BinaryOrOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)BinaryOrOperatorEvaluatableBuilder.LeftOperand).Expression.ShouldBe("true");
            BinaryOrOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)BinaryOrOperatorEvaluatableBuilder.RightOperand).Expression.ShouldBe("false");
        }
    }

    public class EvaluateAsync : BinaryOrOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "true || false";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryOrOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "true")), Result.Success<IExpression>(new OtherExpression(context, "false")), sourceExpression);

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
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "true || false";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryOrOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "true")), Result.Success<IExpression>(new OtherExpression(context, "false")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeTrue();
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
            var sut = new BinaryOrOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "true")), Result.Success<IExpression>(new OtherExpression(context, "false")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(BinaryOrOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
