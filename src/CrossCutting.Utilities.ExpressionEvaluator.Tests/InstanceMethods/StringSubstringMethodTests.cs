namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.InstanceMethods;

public class StringSubstringMethodTests : TestBase<StringSubstringMethod>
{
    public class Evaluate : StringSubstringMethodTests
    {
        [Fact]
        public async Task Returns_Ok_On_Valid_Arguments_Without_Length()
        {
            // Arrange
            var sut = CreateSut();
            var state = CreateDotExpressionComponentState("\"Hello world\".Substring(6)", "Hello world", "Substring(6)");
            var context = new FunctionCallContext(state);

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello world".Substring(6));
        }

        [Fact]
        public async Task Returns_Ok_On_Valid_Arguments_With_Length()
        {
            // Arrange
            var sut = CreateSut();
            var state = CreateDotExpressionComponentState("\"Hello world\".Substring(0, 5)", "Hello world", "Substring(0, 5)");
            var context = new FunctionCallContext(state);

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello world".Substring(0, 5));
        }

        [Fact]
        public async Task Returns_Invalid_On_Invalid_Arguments_Without_Length()
        {
            // Arrange
            var sut = CreateSut();
            var state = CreateDotExpressionComponentState("\"Hello world\".Substring(600)", "Hello world", "Substring(600)");
            var context = new FunctionCallContext(state);

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Index must refer to a location within the string");
        }

        [Fact]
        public async Task Returns_Invalid_On_Invalid_Arguments_With_Length()
        {
            // Arrange
            var sut = CreateSut();
            var state = CreateDotExpressionComponentState("\"Hello world\".Substring(0, 500)", "Hello world", "Substring(0, 500)");
            var context = new FunctionCallContext(state);

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Index and length must refer to a location within the string");
        }
    }
}
