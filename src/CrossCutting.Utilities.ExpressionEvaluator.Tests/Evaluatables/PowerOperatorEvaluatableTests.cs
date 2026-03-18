namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class PowerOperatorEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : PowerOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new PowerOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(2))
                .WithRightOperand(new LiteralEvaluatableBuilder(4))
                .Build();
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(Math.Pow(2, 4));
        }
    }

    public class GetChildEvaluatables : PowerOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new PowerOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(2))
                .WithRightOperand(new LiteralEvaluatableBuilder(4))
                .BuildTyped();
            var context = CreateContext("Dummy");

            // Act
            var result = sut.GetChildEvaluatables().ToArray();

            // Assert
            result.Length.ShouldBe(2);
            (await result[0].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe(2);
            (await result[1].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe(4);
        }
    }
}