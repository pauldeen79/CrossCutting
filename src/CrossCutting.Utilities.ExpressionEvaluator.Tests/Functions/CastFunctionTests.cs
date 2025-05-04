namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public class CastFunctionTests : TestBase<CastFunction>
{
    public class Evaluate : CastFunctionTests
    {
        [Fact]
        public void Returns_Success_On_Correct_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            var functionCall = new FunctionCallBuilder().WithName("Cast").AddArguments("System.Int32", "1");
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy"), MemberType.Function);

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<int>();
        }

        [Fact]
        public void Returns_Error_When_Cast_Is_Not_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var functionCall = new FunctionCallBuilder().WithName("Cast").AddArguments("System.Int64", "1");
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy"), MemberType.Function);

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
        }
    }
}
