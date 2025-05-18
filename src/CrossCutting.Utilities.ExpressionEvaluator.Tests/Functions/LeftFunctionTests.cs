namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public class LeftFunctionTests : TestBase<LeftFunction>
{
    public class EvaluateAsync : LeftFunctionTests
    {
        [Fact]
        public async Task Returns_Ok_When_String_Expression_Is_Long_Enough()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Left").WithMemberType(MemberType.Function).AddArguments("\"hello world\"", "5"), CreateContext("Dummy"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("hello");
        }

        [Fact]
        public async Task Returns_Invalid_When_String_Expression_Is_Not_Long_Enough()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Left").WithMemberType(MemberType.Function).AddArguments("\"hello world\"", "100000"), CreateContext("Dummy"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Length must refer to a location within the string");
        }
    }
}
