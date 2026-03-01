namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class NullOperatorEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : NullOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new NullOperatorEvaluatableBuilder()
                .WithOperand(new LiteralEvaluatableBuilder(1))
                .Build();
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe((object)1 is null);
        }
    }

    public class GetChildEvaluatables : NullOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new NullOperatorEvaluatableBuilder()
                .WithOperand(new LiteralEvaluatableBuilder(1))
                .BuildTyped();
            var context = CreateContext("Dummy");

            // Act
            var result = sut.GetChildEvaluatables().ToArray();

            // Assert
            result.Length.ShouldBe(1);
            (await result[0].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe(1);
        }
    }
}