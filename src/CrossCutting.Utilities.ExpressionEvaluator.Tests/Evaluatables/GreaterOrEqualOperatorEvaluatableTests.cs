namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class GreaterOrEqualOperatorEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : GreaterOrEqualOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new GreaterOrEqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(1))
                .WithRightOperand(new LiteralEvaluatableBuilder(2))
                .Build();
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1 >= 2);
        }
    }

    public class GetChildEvaluatables : GreaterOrEqualOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new GreaterOrEqualOperatorEvaluatableBuilder()
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

    public class ToTypedBuilder : GreaterOrEqualOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatable<bool> sut = new GreaterOrEqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder(true))
                .WithRightOperand(new LiteralEvaluatableBuilder(false))
                .BuildTyped();

            // Act
            var actual = sut.ToTypedBuilder();

            // Assert
            actual.ShouldBeOfType<GreaterOrEqualOperatorEvaluatableBuilder>();
        }
    }
}