namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class EqualOperatorExpressionTests : TestBase
{
    public class ToBuilder : EqualOperatorExpressionTests
    {
        [Fact]
        public void Returns_EqualOperatorEvaluatableBuilder_Correctly()
        {
            // Arrange
            var sourceExpression = "Dummy1 == Dummy2";
            var context = CreateContext(sourceExpression);
            var sut = new EqualOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "Dummy1")), Result.Success<IExpression>(new OtherExpression(context, "Dummy2")), sourceExpression);

            // Act
            var result = sut.ToBuilder();

            // Assert
            result.ShouldBeOfType<EqualOperatorEvaluatableBuilder>();
            var equalOperatorEvaluatableBuilder = (EqualOperatorEvaluatableBuilder)result;
            equalOperatorEvaluatableBuilder.LeftOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)equalOperatorEvaluatableBuilder.LeftOperand).SourceExpression.ShouldBe("Dummy1");
            equalOperatorEvaluatableBuilder.RightOperand.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)equalOperatorEvaluatableBuilder.RightOperand).SourceExpression.ShouldBe("Dummy2");
        }
    }

    public class EvaluateAsync : EqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "true == true";
            var context = CreateContext(sourceExpression);
            var sut = new EqualOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "true")), Result.Success<IExpression>(new OtherExpression(context, "true")), sourceExpression);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
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
            var sut = new EqualOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "true")), Result.Success<IExpression>(new OtherExpression(context, "true")), sourceExpression);

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeTrue();
        }
    }

    public class ParseAsync : EqualOperatorExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var sourceExpression = "true == true";
            var context = CreateContext(sourceExpression);
            var sut = new EqualOperatorExpression(Result.Success<IExpression>(new OtherExpression(context, "true")), Result.Success<IExpression>(new OtherExpression(context, "true")), sourceExpression);

            // Act
            var result = await sut.ParseAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(EqualOperatorExpression));
            result.ResultType.ShouldBe(typeof(bool));
        }
    }
}
