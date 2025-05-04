namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public class MidFunctionTests : TestBase<MidFunction>
{
    public class Evaluate : MidFunctionTests
    {
        [Fact]
        public void Returns_Ok_When_String_Expression_Is_Long_Enough()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Mid").AddArguments("\"hello world\"", "1", "4"), CreateContext("Dummy"), MemberType.Function);
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("ello");
        }

        [Fact]
        public void Returns_Invalid_When_Index_Plus_Value_Is_Too_High()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Mid").AddArguments("\"hello world\"", "0", "100000"), CreateContext("Dummy"), MemberType.Function);
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Index and length must refer to a location within the string");
        }

        [Fact]
        public void Returns_Invalid_When_Index_Is_Too_High()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Mid").AddArguments("\"hello world\"", "100000", "1"), CreateContext("Dummy"), MemberType.Function);
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Index and length must refer to a location within the string");
        }
    }
}
