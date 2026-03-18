namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class AddOperatorExpressionTests : TestBase
{
    public class ToBuilder : AddOperatorExpressionTests
    {
        [Fact]
        public void Returns_AddOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "4 + 2";
            var sut = new AddOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<AddOperatorEvaluatableBuilder>();
            var addOperatorEvaluatableBuilder = (AddOperatorEvaluatableBuilder)result;
            addOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<EvaluatableExpressionBuilder>();
            ((EvaluatableExpressionBuilder)addOperatorEvaluatableBuilder.LeftOperand).SourceExpression.ShouldBe("4");
            addOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<EvaluatableExpressionBuilder>();
            ((EvaluatableExpressionBuilder)addOperatorEvaluatableBuilder.RightOperand).SourceExpression.ShouldBe("2");
        }
    }

    public class EvaluateAsync : AddOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "4 + 2";
            var context = CreateContext(sourceExpression);
            var sut = new AddOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(4 + 2);
        }
    }

    public class ParseAsync : AddOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "4 + 2";
            var context = CreateContext(sourceExpression);
            var sut = new AddOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(AddOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
