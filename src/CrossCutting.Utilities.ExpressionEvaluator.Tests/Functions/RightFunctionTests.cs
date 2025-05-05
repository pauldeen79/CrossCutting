namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public class RightFunctionTests : TestBase<RightFunction>
{
    public class Evaluate : RightFunctionTests
    {
        [Fact]
        public void Returns_Ok_When_String_Expression_Is_Long_Enough()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Right").WithMemberType(MemberType.Function).AddArguments("\"hello world\"", "5"), CreateContext("Dummy"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("world");
        }

        [Fact]
        public void Returns_Invalid_When_String_Expression_Is_Not_Long_Enough()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Right").WithMemberType(MemberType.Function).AddArguments("\"hello world\"", "100000"), CreateContext("Dummy"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Length must refer to a location within the string");
        }
    }
}
