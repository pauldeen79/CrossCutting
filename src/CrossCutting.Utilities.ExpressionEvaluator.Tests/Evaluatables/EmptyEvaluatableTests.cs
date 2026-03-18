namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class EmptyEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : EmptyEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result_Typed()
        {
            // Arrange
            var context = CreateContext("Dummy");
            var sut = new EmptyEvaluatable<string>();

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
            result.Value.ShouldBeNull();
        }

        [Fact]
        public async Task Gives_Correct_Result_Untyped()
        {
            // Arrange
            var context = CreateContext("Dummy");
            var sut = new EmptyEvaluatable();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
            result.Value.ShouldBeNull();
        }
    }
}