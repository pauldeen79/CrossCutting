namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class GreaterOrEqualOperatorExpressionTests : TestBase
{
    public class ToBuilder : GreaterOrEqualOperatorExpressionTests
    {
        [Fact]
        public void Returns_GreaterOrEqualOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "Dummy1 > Dummy2";
            var context = CreateContext(sourceExpression);
            var sut = new GreaterOrEqualOperatorExpression(Result.Success<IExpression>(new OtherExpression("Dummy1")), Result.Success<IExpression>(new OtherExpression("Dummy2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<GreaterOrEqualOperatorEvaluatableBuilder>();
            var GreaterOrEqualOperatorEvaluatableBuilder = (GreaterOrEqualOperatorEvaluatableBuilder)result;
            GreaterOrEqualOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)GreaterOrEqualOperatorEvaluatableBuilder.LeftOperand).SourceExpression.ShouldBe("Dummy1");
            GreaterOrEqualOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)GreaterOrEqualOperatorEvaluatableBuilder.RightOperand).SourceExpression.ShouldBe("Dummy2");
        }
    }

    public class EvaluateAsync : GreaterOrEqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 >= 1";
            var context = CreateContext(sourceExpression);
            var sut = new GreaterOrEqualOperatorExpression(Result.Success<IExpression>(new OtherExpression("2")), Result.Success<IExpression>(new OtherExpression("1")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }
    }

    public class EvaluateTypedAsync : GreaterOrEqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 >= 1";
            var context = CreateContext(sourceExpression);
            var sut = new GreaterOrEqualOperatorExpression(Result.Success<IExpression>(new OtherExpression("2")), Result.Success<IExpression>(new OtherExpression("1")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeTrue();
        }
    }

    public class ParseAsync : GreaterOrEqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "2 >= 1";
            var context = CreateContext(sourceExpression);
            var sut = new GreaterOrEqualOperatorExpression(Result.Success<IExpression>(new OtherExpression("2")), Result.Success<IExpression>(new OtherExpression("1")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(GreaterOrEqualOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
