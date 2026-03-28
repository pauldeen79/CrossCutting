namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class LiteralResultEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : LiteralResultEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var context = CreateContext("Dummy");
            var sut = new LiteralResultEvaluatable<string>(Result.Success("Hello world!"));

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello world!");
        }
    }
}