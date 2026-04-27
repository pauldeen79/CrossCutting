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

    public class ToTypedBuilder : EmptyEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatable<string> sut = new EmptyEvaluatable<string>();

            // Act
            var actual = sut.ToTypedBuilder();

            // Assert
            actual.ShouldBeOfType<EmptyEvaluatableBuilder<string>>();
        }
    }

    public class GetValue : EmptyEvaluatableTests
    {
        [Fact]
        public void Can_Get_Value_From_Untyped_Empty_Evaluatable()
        {
            // Arrange
            var sut = new EmptyEvaluatable();

            // Act
            var actual = sut.GetValue();

            // Assert
            actual.ShouldBeNull();
        }

        [Fact]
        public void Can_Get_Value_From_Typed_Empty_Evaluatable()
        {
            // Arrange
            var sut = new EmptyEvaluatable<string>();

            // Act
            var actual = sut.GetValue();

            // Assert
            actual.ShouldBeNull();            
        }
    }

    public class BuildTyped : EmptyEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = new EmptyEvaluatableBuilder<bool>();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<EmptyEvaluatable<bool>>();
        }
    }
}