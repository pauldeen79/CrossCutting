namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class StringEndsWithOperatorEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : StringEndsWithOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new StringEndsWithOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<string>("Test"))
                .WithRightOperand(new LiteralEvaluatableBuilder<string>("t"))
                .Build();
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Test".EndsWith('t'));
        }
    }

    public class GetChildEvaluatables : StringEndsWithOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new StringEndsWithOperatorEvaluatableBuilder()
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

    public class ToTypedBuilder : StringEndsWithOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatable<bool> sut = new StringEndsWithOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder("And"))
                .WithRightOperand(new LiteralEvaluatableBuilder("d"))
                .BuildTyped();

            // Act
            var actual = sut.ToTypedBuilder();

            // Assert
            actual.ShouldBeOfType<StringEndsWithOperatorEvaluatableBuilder>();
        }
    }
}