namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class ModulusOperatorEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : ModulusOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new ModulusOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(4))
                .WithRightOperand(new LiteralEvaluatableBuilder(2))
                .Build();
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(4 % 2);
        }
    }

    public class GetChildEvaluatables : ModulusOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new ModulusOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(4))
                .WithRightOperand(new LiteralEvaluatableBuilder(2))
                .BuildTyped();
            var context = CreateContext("Dummy");

            // Act
            var result = sut.GetChildEvaluatables().ToArray();

            // Assert
            result.Length.ShouldBe(2);
            (await result[0].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe(4);
            (await result[1].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe(2);
        }
    }
}