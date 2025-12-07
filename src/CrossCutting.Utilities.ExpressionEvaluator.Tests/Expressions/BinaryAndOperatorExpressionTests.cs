namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class BinaryAndOperatorExpressionTests : TestBase
{
    public class ToBuilder : BinaryAndOperatorExpressionTests
    {
        [Fact]
        public void Returns_BinaryAndOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "true && true";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryAndOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "true")), Result.Success<IExpression>(new OtherExpression(context, "true")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<BinaryAndOperatorEvaluatableBuilder>();
            var BinaryAndOperatorEvaluatableBuilder = (BinaryAndOperatorEvaluatableBuilder)result;
            BinaryAndOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)BinaryAndOperatorEvaluatableBuilder.LeftOperand).Expression.ShouldBe("true");
            BinaryAndOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)BinaryAndOperatorEvaluatableBuilder.RightOperand).Expression.ShouldBe("true");
        }
    }

    public class EvaluateAsync : BinaryAndOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "true && true";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryAndOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "true")), Result.Success<IExpression>(new OtherExpression(context, "true")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }
    }

    public class EvaluateTypedAsync : BinaryAndOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "true && true";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryAndOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "true")), Result.Success<IExpression>(new OtherExpression(context, "true")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeTrue();
        }
    }

    public class ParseAsync : BinaryAndOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "true && true";
            var context = CreateContext(sourceExpression);
            var sut = new BinaryAndOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "true")), Result.Success<IExpression>(new OtherExpression(context, "true")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(BinaryAndOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
