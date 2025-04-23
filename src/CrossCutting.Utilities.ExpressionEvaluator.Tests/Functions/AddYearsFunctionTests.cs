namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public class AddYearsFunctionTests : TestBase<AddYearsFunction>
{
    public class Evaluate : AddYearsFunctionTests
    {
        [Fact]
        public void Returns_Success_On_Correct_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            var functionCall = new FunctionCallBuilder().WithName("AddYears").AddArguments("2025-01-01", "1");
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy"));

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddYears(1));
        }
    }
}
