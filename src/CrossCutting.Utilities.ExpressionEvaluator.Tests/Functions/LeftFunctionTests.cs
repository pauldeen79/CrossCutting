namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public class LeftFunctionTests : TestBase<LeftFunction>
{
    public class Evaluate : LeftFunctionTests
    {
        [Fact]
        public void Returns_Ok_When_String_Expression_Is_Long_Enough()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Left").WithMemberType(MemberType.Function).AddArguments("\"hello world\"", "5"), CreateContext("Dummy"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("hello");
        }

        [Fact]
        public void Returns_Invalid_When_String_Expression_Is_Not_Long_Enough()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Left").WithMemberType(MemberType.Function).AddArguments("\"hello world\"", "100000"), CreateContext("Dummy"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Length must refer to a location within the string");
        }
    }
}
