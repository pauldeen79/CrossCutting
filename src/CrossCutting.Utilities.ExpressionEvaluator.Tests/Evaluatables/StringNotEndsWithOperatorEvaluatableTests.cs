namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class StringNotEndsWithOperatorEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : StringNotEndsWithOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new StringNotEndsWithOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<string>("Test"))
                .WithRightOperand(new LiteralEvaluatableBuilder<string>("t"))
                .Build();
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(!"Test".EndsWith('t'));
        }
    }

    public class GetChildEvaluatables : StringNotEndsWithOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new StringNotEndsWithOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<string>("Test"))
                .WithRightOperand(new LiteralEvaluatableBuilder<string>("t"))
                .BuildTyped();
            var context = CreateContext("Dummy");

            // Act
            var result = sut.GetChildEvaluatables().ToArray();

            // Assert
            result.Length.ShouldBe(2);
            (await result[0].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe("Test");
            (await result[1].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe("t");
        }
    }
}