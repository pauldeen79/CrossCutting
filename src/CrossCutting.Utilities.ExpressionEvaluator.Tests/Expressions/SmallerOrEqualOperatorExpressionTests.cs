namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class SmallerOrEqualOperatorExpressionTests : TestBase
{
    public class ToBuilder : SmallerOrEqualOperatorExpressionTests
    {
        [Fact]
        public void Returns_SmallerOrEqualOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "Dummy1 <= Dummy2";
            var context = CreateContext(sourceExpression);
            var sut = new SmallerOrEqualOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "Dummy1")), Result.Success<IExpression>(new OtherExpression(context, "Dummy2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<SmallerOrEqualOperatorEvaluatableBuilder>();
            var SmallerOrEqualOperatorEvaluatableBuilder = (SmallerOrEqualOperatorEvaluatableBuilder)result;
            SmallerOrEqualOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)SmallerOrEqualOperatorEvaluatableBuilder.LeftOperand).Expression.ShouldBe("Dummy1");
            SmallerOrEqualOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)SmallerOrEqualOperatorEvaluatableBuilder.RightOperand).Expression.ShouldBe("Dummy2");
        }
    }

    public class EvaluateAsync : SmallerOrEqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 <= 1";
            var context = CreateContext(sourceExpression);
            var sut = new SmallerOrEqualOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(false);
        }
    }

    public class EvaluateTypedAsync : SmallerOrEqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 <= 1";
            var context = CreateContext(sourceExpression);
            var sut = new SmallerOrEqualOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeFalse();
        }
    }

    public class ParseAsync : SmallerOrEqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 <= 1";
            var context = CreateContext(sourceExpression);
            var sut = new SmallerOrEqualOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(SmallerOrEqualOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
