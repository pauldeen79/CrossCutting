namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class EqualOperatorEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : EqualOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(1))
                .WithRightOperand(new LiteralEvaluatableBuilder(2))
                .Build();
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 == 2);
        }
    }

    public class GetChildEvaluatables : EqualOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(1))
                .WithRightOperand(new LiteralEvaluatableBuilder(2))
                .BuildTyped();
            var context = CreateContext("Dummy");

            // Act
            var result = sut.GetChildEvaluatables().ToArray();

            // Assert
            result.Length.ShouldBe(2);
            (await result[0].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe(1);
            (await result[1].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe(2);
        }
    }

    public class ToTypedBuilder : EqualOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatable<bool> sut = new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(true))
                .WithRightOperand(new LiteralEvaluatableBuilder(false))
                .BuildTyped();

            // Act
            var actual = sut.ToTypedBuilder();

            // Assert
            actual.ShouldBeOfType<EqualOperatorEvaluatableBuilder>();
        }
    }
}