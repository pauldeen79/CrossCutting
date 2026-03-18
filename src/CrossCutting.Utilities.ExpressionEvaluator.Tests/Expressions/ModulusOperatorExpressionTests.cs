namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class ModulusOperatorExpressionTests : TestBase
{
    public class ToBuilder : ModulusOperatorExpressionTests
    {
        [Fact]
        public void Returns_ModulusOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "4 % 2";
            var sut = new ModulusOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<ModulusOperatorEvaluatableBuilder>();
            var modulusOperatorEvaluatableBuilder = (ModulusOperatorEvaluatableBuilder)result;
            modulusOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<EvaluatableExpressionBuilder>();
            ((EvaluatableExpressionBuilder)modulusOperatorEvaluatableBuilder.LeftOperand).SourceExpression.ShouldBe("4");
            modulusOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<EvaluatableExpressionBuilder>();
            ((EvaluatableExpressionBuilder)modulusOperatorEvaluatableBuilder.RightOperand).SourceExpression.ShouldBe("2");
        }
    }

    public class EvaluateAsync : ModulusOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "4 % 2";
            var context = CreateContext(sourceExpression);
            var sut = new ModulusOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(4 % 2);
        }
    }

    public class ParseAsync : ModulusOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "4 % 2";
            var context = CreateContext(sourceExpression);
            var sut = new ModulusOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("4")), Result.Success<IExpression>(new EvaluatableExpression("2")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(ModulusOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
