namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class LessOperatorExpressionTests : TestBase
{
    public class ToBuilder : LessOperatorExpressionTests
    {
        [Fact]
        public void Returns_LessOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "Dummy1 < Dummy2";
            var context = CreateContext(sourceExpression);
            var sut = new LessOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "Dummy1")), Result.Success<IExpression>(new OtherExpression(context, "Dummy2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<LessOperatorEvaluatableBuilder>();
            var LessOperatorEvaluatableBuilder = (LessOperatorEvaluatableBuilder)result;
            LessOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)LessOperatorEvaluatableBuilder.LeftOperand).Expression.ShouldBe("Dummy1");
            LessOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)LessOperatorEvaluatableBuilder.RightOperand).Expression.ShouldBe("Dummy2");
        }
    }

    public class EvaluateAsync : LessOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 < 1";
            var context = CreateContext(sourceExpression);
            var sut = new LessOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(false);
        }
    }

    public class EvaluateTypedAsync : LessOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 < 1";
            var context = CreateContext(sourceExpression);
            var sut = new LessOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeFalse();
        }
    }

    public class ParseAsync : LessOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 < 1";
            var context = CreateContext(sourceExpression);
            var sut = new LessOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(LessOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
