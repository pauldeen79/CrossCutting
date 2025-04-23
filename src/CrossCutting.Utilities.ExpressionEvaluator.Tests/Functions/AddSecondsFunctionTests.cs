namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public class AddSecondsFunctionTests : TestBase<AddSecondsFunction>
{
    public class Evaluate : AddSecondsFunctionTests
    {
        [Fact]
        public void Returns_Success_On_Correct_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            var functionCall = new FunctionCallBuilder().WithName("AddSeconds").AddArguments("2025-01-01", "1");
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy"));

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddSeconds(1));
        }
    }
}
