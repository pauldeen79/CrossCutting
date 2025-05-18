namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public class ConvertFunctionTests : TestBase<ConvertFunction>
{
    public class Evaluate : ConvertFunctionTests
    {
        [Fact]
        public async Task Returns_Success_On_Correct_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            var functionCall = new FunctionCallBuilder().WithName("Convert").WithMemberType(MemberType.Function).AddArguments("System.Int32", "1");
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy"));

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task Returns_Error_When_Convert_Is_Not_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var functionCall = new FunctionCallBuilder().WithName("Convert").WithMemberType(MemberType.Function).AddArguments("System.DateTime", "1");
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy"));

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
        }
    }
}
