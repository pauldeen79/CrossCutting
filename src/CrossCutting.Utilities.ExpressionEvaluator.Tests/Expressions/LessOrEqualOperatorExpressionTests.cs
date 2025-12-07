namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class LessOrEqualOperatorExpressionTests : TestBase
{
    public class ToBuilder : LessOrEqualOperatorExpressionTests
    {
        [Fact]
        public void Returns_LessOrEqualOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "Dummy1 <= Dummy2";
            var context = CreateContext(sourceExpression);
            var sut = new LessOrEqualOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "Dummy1")), Result.Success<IExpression>(new OtherExpression(context, "Dummy2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<LessOrEqualOperatorEvaluatableBuilder>();
            var LessOrEqualOperatorEvaluatableBuilder = (LessOrEqualOperatorEvaluatableBuilder)result;
            LessOrEqualOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)LessOrEqualOperatorEvaluatableBuilder.LeftOperand).Expression.ShouldBe("Dummy1");
            LessOrEqualOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)LessOrEqualOperatorEvaluatableBuilder.RightOperand).Expression.ShouldBe("Dummy2");
        }
    }

    public class EvaluateAsync : LessOrEqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 <= 1";
            var context = CreateContext(sourceExpression);
            var sut = new LessOrEqualOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(false);
        }
    }

    public class EvaluateTypedAsync : LessOrEqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 <= 1";
            var context = CreateContext(sourceExpression);
            var sut = new LessOrEqualOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeFalse();
        }
    }

    public class ParseAsync : LessOrEqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 <= 1";
            var context = CreateContext(sourceExpression);
            var sut = new LessOrEqualOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(LessOrEqualOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
