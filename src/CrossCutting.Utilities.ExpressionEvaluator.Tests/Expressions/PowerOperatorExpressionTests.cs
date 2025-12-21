namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class PowerOperatorExpressionTests : TestBase
{
    public class ToBuilder : PowerOperatorExpressionTests
    {
        [Fact]
        public void Returns_PowerOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "4 ^ 2";
            var sut = new PowerOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<PowerOperatorEvaluatableBuilder>();
            var equalOperatorEvaluatableBuilder = (PowerOperatorEvaluatableBuilder)result;
            equalOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<EvaluatableExpression>();
            ((EvaluatableExpression)equalOperatorEvaluatableBuilder.LeftOperand).SourceExpression.ShouldBe("4");
            equalOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<EvaluatableExpression>();
            ((EvaluatableExpression)equalOperatorEvaluatableBuilder.RightOperand).SourceExpression.ShouldBe("2");
        }
    }

    public class EvaluateAsync : PowerOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "4 ^ 2";
            var context = CreateContext(sourceExpression);
            var sut = new PowerOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(Math.Pow(4, 2));
        }
    }

    public class ParseAsync : PowerOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "4 ^ 2";
            var context = CreateContext(sourceExpression);
            var sut = new PowerOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(PowerOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
