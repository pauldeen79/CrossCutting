namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class DelegateResultEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : DelegateResultEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var context = CreateContext("Dummy");
            var sut = new DelegateResultEvaluatable<string>(() => Result.Success("Hello world!"));

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello world!");
        }
    }

    public class ToTypedBuilder : DelegateResultEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatable<string> sut = new DelegateResultEvaluatable<string>(() => "Hello world!");

            // Act
            var actual = sut.ToTypedBuilder();

            // Assert
            actual.ShouldBeOfType<DelegateResultEvaluatableBuilder<string>>();
        }
    }

    public class BuildTyped : DelegateResultEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = new DelegateResultEvaluatableBuilder<bool>()
                .WithValue(() => Result.Success(true));

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<DelegateResultEvaluatable<bool>>();
        }
    }
}