namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public class CoalesceFunctionTests : TestBase<CoalesceFunction>
{
    public class Evaluate : CoalesceFunctionTests
    {
        [Fact]
        public void Returns_NoContent_When_All_Expressions_Are_Null()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Coalesce").WithMemberType(MemberType.Function).AddArguments("null", "null", "null"), CreateContext("Dummy"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
            result.Value.ShouldBeNull();
        }

        [Fact]
        public void Returns_Non_Succesful_Result_From_Expressions_When_Present()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Coalesce").WithMemberType(MemberType.Function).AddArguments("null", "null", "error"), CreateContext("Dummy"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Succesful_Result_From_Expressions_When_Value_Is_Not_Null()
        {
            // Arrange
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Coalesce").WithMemberType(MemberType.Function).AddArguments("null", "null", "1"), CreateContext("Dummy"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1);
        }
    }
}
