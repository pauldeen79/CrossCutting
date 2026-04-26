namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class NotNullOperatorEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : NotNullOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new NotNullOperatorEvaluatableBuilder()
                .WithOperand(new LiteralEvaluatableBuilder(1))
                .Build();
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe((object)1 is not null);
        }
    }

    public class GetChildEvaluatables : NotNullOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new NotNullOperatorEvaluatableBuilder()
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

    public class ToTypedBuilder : NotNullOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatable<bool> sut = new NotNullOperatorEvaluatableBuilder()
                .WithOperand(new LiteralEvaluatableBuilder(true))
                .BuildTyped();

            // Act
            var actual = sut.ToTypedBuilder();

            // Assert
            actual.ShouldBeOfType<NotNullOperatorEvaluatableBuilder>();
        }
    }
}