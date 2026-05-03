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

    public class GetValue : LiteralResultEvaluatableTests
    {
        [Fact]
        public void Can_Get_Value_From_Untyped_LiteralResult_Evaluatable()
        {
            // Arrange
            var sut = new LiteralResultEvaluatable("some value");

            // Act
            var actual = sut.GetValue();

            // Assert
            actual.GetValue().ShouldBe("some value");
        }

        [Fact]
        public void Can_Get_Value_From_Typed_LiteralResult_Evaluatable()
        {
            // Arrange
            var sut = new LiteralResultEvaluatable<string>("some value");

            // Act
            var actual = sut.GetValue();

            // Assert
            actual.GetValue().ShouldBe("some value");            
        }
    }

    public class ToTypedBuilder : LiteralResultEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatable<string> sut = new LiteralResultEvaluatable<string>("Hello world!");

            // Act
            var actual = sut.ToTypedBuilder();

            // Assert
            actual.ShouldBeOfType<LiteralResultEvaluatableBuilder<string>>();
        }
    }

    public class BuildTyped : LiteralResultEvaluatableTests
    {
        [Fact]
        public async Task Gives_Correct_Result()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = new LiteralResultEvaluatableBuilder<bool>()
                .WithValue(true);

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<LiteralResultEvaluatable<bool>>();
        }
    }
}