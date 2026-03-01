namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class MultiplyOperatorEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : MultiplyOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new MultiplyOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(8))
                .WithRightOperand(new LiteralEvaluatableBuilder(2))
                .Build();
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(8 * 2);
        }
    }

    public class GetChildEvaluatables : MultiplyOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new MultiplyOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(8))
                .WithRightOperand(new LiteralEvaluatableBuilder(2))
                .BuildTyped();
            var context = CreateContext("Dummy");

            // Act
            var result = sut.GetChildEvaluatables().ToArray();

            // Assert
            result.Length.ShouldBe(2);
            (await result[0].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe(8);
            (await result[1].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe(2);
        }
    }
}