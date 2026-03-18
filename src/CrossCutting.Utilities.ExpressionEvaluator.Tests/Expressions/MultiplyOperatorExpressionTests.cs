namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class MultiplyOperatorExpressionTests : TestBase
{
    public class ToBuilder : MultiplyOperatorExpressionTests
    {
        [Fact]
        public void Returns_MultiplyOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "4 * 2";
            var sut = new MultiplyOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<MultiplyOperatorEvaluatableBuilder>();
            var multiplyOperatorEvaluatableBuilder = (MultiplyOperatorEvaluatableBuilder)result;
            multiplyOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<EvaluatableExpressionBuilder>();
            ((EvaluatableExpressionBuilder)multiplyOperatorEvaluatableBuilder.LeftOperand).SourceExpression.ShouldBe("4");
            multiplyOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<EvaluatableExpressionBuilder>();
            ((EvaluatableExpressionBuilder)multiplyOperatorEvaluatableBuilder.RightOperand).SourceExpression.ShouldBe("2");
        }
    }

    public class EvaluateAsync : MultiplyOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "4 * 2";
            var context = CreateContext(sourceExpression);
            var sut = new MultiplyOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(4 * 2);
        }
    }

    public class ParseAsync : MultiplyOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "4 * 2";
            var context = CreateContext(sourceExpression);
            var sut = new MultiplyOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(MultiplyOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
