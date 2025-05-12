namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public class MidFunctionTests : TestBase<MidFunction>
{
    public class Evaluate : MidFunctionTests
    {
        [Fact]
        public async Task Returns_Ok_When_String_Expression_Is_Long_Enough()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Mid").WithMemberType(MemberType.Function).AddArguments("\"hello world\"", "1", "4"), CreateContext("Dummy"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("ello");
        }

        [Fact]
        public async Task Returns_Invalid_When_Index_Plus_Value_Is_Too_High()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Mid").WithMemberType(MemberType.Function).AddArguments("\"hello world\"", "0", "100000"), CreateContext("Dummy"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Index and length must refer to a location within the string");
        }

        [Fact]
        public async Task Returns_Invalid_When_Index_Is_Too_High()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Mid").WithMemberType(MemberType.Function).AddArguments("\"hello world\"", "100000", "1"), CreateContext("Dummy"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Index and length must refer to a location within the string");
        }
    }
}
