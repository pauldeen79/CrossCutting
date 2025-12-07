namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class SmallerOperatorExpressionTests : TestBase
{
    public class ToBuilder : SmallerOperatorExpressionTests
    {
        [Fact]
        public void Returns_SmallerOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "Dummy1 < Dummy2";
            var context = CreateContext(sourceExpression);
            var sut = new SmallerOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "Dummy1")), Result.Success<IExpression>(new OtherExpression(context, "Dummy2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<SmallerOperatorEvaluatableBuilder>();
            var SmallerOperatorEvaluatableBuilder = (SmallerOperatorEvaluatableBuilder)result;
            SmallerOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)SmallerOperatorEvaluatableBuilder.LeftOperand).Expression.ShouldBe("Dummy1");
            SmallerOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)SmallerOperatorEvaluatableBuilder.RightOperand).Expression.ShouldBe("Dummy2");
        }
    }

    public class EvaluateAsync : SmallerOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 < 1";
            var context = CreateContext(sourceExpression);
            var sut = new SmallerOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(false);
        }
    }

    public class EvaluateTypedAsync : SmallerOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 < 1";
            var context = CreateContext(sourceExpression);
            var sut = new SmallerOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeFalse();
        }
    }

    public class ParseAsync : SmallerOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 < 1";
            var context = CreateContext(sourceExpression);
            var sut = new SmallerOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(SmallerOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
