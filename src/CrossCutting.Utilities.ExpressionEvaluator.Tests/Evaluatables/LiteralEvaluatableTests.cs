namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Evaluatables;

public class LiteralEvaluatableTests : TestBase
{
    public class EvaluateTypedAsync : LiteralEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            var context = CreateContext("Dummy");
            var sut = new LiteralEvaluatable<string>("Hello world!");

            // Act
            var result = await sut.EvaluateTypedAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello world!");
        }
    }

    public class GetValue : LiteralEvaluatableTests
    {
        [Fact]
        public void Can_Get_Value_From_Untyped_Literal_Evaluatable()
        {
            // Arrange
            var sut = new LiteralEvaluatable("some value");

            // Act
            var actual = sut.GetValue();

            // Assert
            actual.ShouldBe("some value");
        }

        [Fact]
        public void Can_Get_Value_From_Typed_Literal_Evaluatable()
        {
            // Arrange
            var sut = new LiteralEvaluatable<string>("some value");

            // Act
            var actual = sut.GetValue();

            // Assert
            actual.ShouldBe("some value");            
        }
    }

    public class ToTypedBuilder : LiteralEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatable<string> sut = new LiteralEvaluatable<string>("Hello world!");

            // Act
            var actual = sut.ToTypedBuilder();

            // Assert
            actual.ShouldBeOfType<LiteralEvaluatableBuilder<string>>();
        }
    }

    public class BuildTyped : LiteralEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = new LiteralEvaluatableBuilder<bool>()
                .WithValue(true);

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<LiteralEvaluatable<bool>>();
        }
    }
}