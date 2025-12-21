namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class NotEqualOperatorExpressionTests : TestBase
{
    public class ToBuilder : NotEqualOperatorExpressionTests
    {
        [Fact]
        public void Returns_NotEqualOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "Dummy1 == Dummy2";
            var sut = new NotEqualOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("Dummy1")), Result.Success<IExpression>(new EvaluatableExpression("Dummy2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<NotEqualOperatorEvaluatableBuilder>();
            var notEqualOperatorEvaluatableBuilder = (NotEqualOperatorEvaluatableBuilder)result;
            notEqualOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<EvaluatableExpression>();
            ((EvaluatableExpression)notEqualOperatorEvaluatableBuilder.LeftOperand).SourceExpression.ShouldBe("Dummy1");
            notEqualOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<EvaluatableExpression>();
            ((EvaluatableExpression)notEqualOperatorEvaluatableBuilder.RightOperand).SourceExpression.ShouldBe("Dummy2");
        }
    }

    public class EvaluateAsync : NotEqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "true == true";
            var context = CreateContext(sourceExpression);
            var sut = new NotEqualOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("true")), Result.Success<IExpression>(new EvaluatableExpression("true")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(false);
        }
    }

    public class EvaluateTypedAsync : EqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "true == true";
            var context = CreateContext(sourceExpression);
            var sut = new NotEqualOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("true")), Result.Success<IExpression>(new EvaluatableExpression("true")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeFalse();
        }
    }

    public class ParseAsync : NotEqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "true == true";
            var context = CreateContext(sourceExpression);
            var sut = new NotEqualOperatorExpression(Result.Success<IExpression>(new EvaluatableExpression("true")), Result.Success<IExpression>(new EvaluatableExpression("true")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(NotEqualOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
