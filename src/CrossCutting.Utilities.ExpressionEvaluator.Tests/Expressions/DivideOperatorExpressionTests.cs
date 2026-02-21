namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class DivideOperatorExpressionTests : TestBase
{
    public class ToBuilder : DivideOperatorExpressionTests
    {
        [Fact]
        public void Returns_DivideOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "4 / 2";
            var sut = new DivideOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<DivideOperatorEvaluatableBuilder>();
            var divideOperatorEvaluatableBuilder = (DivideOperatorEvaluatableBuilder)result;
            divideOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<EvaluatableExpressionBuilder>();
            ((EvaluatableExpressionBuilder)divideOperatorEvaluatableBuilder.LeftOperand).SourceExpression.ShouldBe("4");
            divideOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<EvaluatableExpressionBuilder>();
            ((EvaluatableExpressionBuilder)divideOperatorEvaluatableBuilder.RightOperand).SourceExpression.ShouldBe("2");
        }
    }

    public class EvaluateAsync : DivideOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "4 / 2";
            var context = CreateContext(sourceExpression);
            var sut = new DivideOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(4 / 2);
        }
    }

    public class ParseAsync : DivideOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "4 / 2";
            var context = CreateContext(sourceExpression);
            var sut = new DivideOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(DivideOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
