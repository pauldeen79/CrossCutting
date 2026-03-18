namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class UnaryNegateOperatorEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : UnaryNegateOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new UnaryNegateOperatorEvaluatableBuilder()
                .WithOperand(new LiteralEvaluatableBuilder(true))
                .Build();
            var context = CreateContext("Dummy");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(!true);
        }
    }
    
    public class GetChildEvaluatables : UnaryNegateOperatorEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var sut = new UnaryNegateOperatorEvaluatableBuilder()
                .WithOperand(new LiteralEvaluatableBuilder(true))
                .BuildTyped();
            var context = CreateContext("Dummy");

            // Act
            var result = sut.GetChildEvaluatables().ToArray();

            // Assert
            result.Length.ShouldBe(1);
            (await result[0].EvaluateAsync(context, CancellationToken.None)).GetValue().ShouldBe(true);
        }
    }
}