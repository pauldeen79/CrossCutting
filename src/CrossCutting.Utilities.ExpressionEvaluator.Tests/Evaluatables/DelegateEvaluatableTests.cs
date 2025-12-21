namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class DelegateEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : DelegateEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var context = CreateContext("Dummy");
            var sut = new DelegateEvaluatable<string>(() => "Hello world!");

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello world!");
        }
    }
}