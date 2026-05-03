namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class BinaryAndOperatorEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : BinaryAndOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new BinaryAndOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<bool>(true))
                .WithRightOperand(new LiteralEvaluatableBuilder<bool>(false))
                .Build();
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true && false);
        }
    }

    public class GetChildEvaluatables : BinaryAndOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new BinaryAndOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<bool>(true))
                .WithRightOperand(new LiteralEvaluatableBuilder<bool>(false))
                .BuildTyped();
            var context = CreateContext("Dummy");

            // Act
            var result = sut.GetChildEvaluatables().ToArray();

            // Assert
            result.Length.ShouldBe(2);
            (await result[0].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe(true);
            (await result[1].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe(false);
        }
    }

    public class ToTypedBuilder : BinaryAndOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatable<bool> sut = new BinaryAndOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<bool>(true))
                .WithRightOperand(new LiteralEvaluatableBuilder<bool>(false))
                .BuildTyped();

            // Act
            var actual = sut.ToTypedBuilder();

            // Assert
            actual.ShouldBeOfType<BinaryAndOperatorEvaluatableBuilder>();
        }
    }

    public class BuildTyped : BinaryAndOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = new BinaryAndOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder<bool>(true))
                .WithRightOperand(new LiteralEvaluatableBuilder<bool>(false));

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<BinaryAndOperatorEvaluatable>();
        }
    }
}