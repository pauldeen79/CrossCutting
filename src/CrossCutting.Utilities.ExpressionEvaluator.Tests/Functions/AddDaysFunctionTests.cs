namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public class AddDaysFunctionTests : TestBase<AddDaysFunction>
{
    public class Evaluate : AddDaysFunctionTests
    {
        [Fact]
        public void Returns_Success_On_Correct_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            var functionCall = new FunctionCallBuilder().WithName("AddDays").AddArguments("2025-01-01", "1");
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy"));

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddDays(1));
        }
    }
}
