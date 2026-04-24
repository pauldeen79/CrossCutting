namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class StringNotStartsWithOperatorEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : StringNotStartsWithOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new StringNotStartsWithOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<string>("Test"))
                .WithRightOperand(new LiteralEvaluatableBuilder<string>("T"))
                .Build();
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(!"Test".StartsWith('T'));
        }
    }

    public class GetChildEvaluatables : StringNotStartsWithOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new StringNotStartsWithOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<string>("Test"))
                .WithRightOperand(new LiteralEvaluatableBuilder<string>("T"))
                .BuildTyped();
            var context = CreateContext("Dummy");

            // Act
            var result = sut.GetChildEvaluatables().ToArray();

            // Assert
            result.Length.ShouldBe(2);
            (await result[0].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe("Test");
            (await result[1].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe("T");
        }
    }
}