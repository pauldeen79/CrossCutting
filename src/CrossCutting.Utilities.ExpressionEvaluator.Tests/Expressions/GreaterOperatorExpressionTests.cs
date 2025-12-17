namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class GreaterOperatorExpressionTests : TestBase
{
    public class ToBuilder : GreaterOperatorExpressionTests
    {
        [Fact]
        public void Returns_GreaterOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "Dummy1 > Dummy2";
            var context = CreateContext(sourceExpression);
            var sut = new GreaterOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "Dummy1")), Result.Success<IExpression>(new OtherExpression(context, "Dummy2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<GreaterOperatorEvaluatableBuilder>();
            var GreaterOperatorEvaluatableBuilder = (GreaterOperatorEvaluatableBuilder)result;
            GreaterOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)GreaterOperatorEvaluatableBuilder.LeftOperand).SourceExpression.ShouldBe("Dummy1");
            GreaterOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)GreaterOperatorEvaluatableBuilder.RightOperand).SourceExpression.ShouldBe("Dummy2");
        }
    }

    public class EvaluateAsync : GreaterOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 > 1";
            var context = CreateContext(sourceExpression);
            var sut = new GreaterOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }
    }

    public class EvaluateTypedAsync : GreaterOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 > 1";
            var context = CreateContext(sourceExpression);
            var sut = new GreaterOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeTrue();
        }
    }

    public class ParseAsync : GreaterOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 > 1";
            var context = CreateContext(sourceExpression);
            var sut = new GreaterOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "2")), Result.Success<IExpression>(new OtherExpression(context, "1")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(GreaterOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
