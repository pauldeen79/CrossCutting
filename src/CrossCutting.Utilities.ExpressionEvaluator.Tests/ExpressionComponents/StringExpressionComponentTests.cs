namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class StringExpressionComponentTests : TestBase<StringExpressionComponent>
{
    public class EvaluateAsync : StringExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Success_On_String_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("\"my string value\"");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("my string value");
        }

        [Fact]
        public async Task Returns_Continue_On_Non_String_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("null");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }
    }

    public class ParseAsync : StringExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Success_On_String_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("\"my string value\"");

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(string));
            result.ExpressionComponentType.ShouldBe(typeof(StringExpressionComponent));
        }

        [Fact]
        public async Task Returns_Continue_On_Non_String_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("null");

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            result.ExpressionComponentType.ShouldBe(typeof(StringExpressionComponent));
        }
    }
}
