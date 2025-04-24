namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public class DatePartFunctionTests : TestBase<DatePartFunction>
{
    public class Evaluate : DatePartFunctionTests
    {
        [Fact]
        public void Returns_Success_On_Correct_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            // Note that the ExpressionEvaluator currently doesn't support DateTime values in ISO format, because it will see minus signs and try sub substract these values.
            var functionCall = new FunctionCallBuilder().WithName("DatePart").AddArguments("2025-01-01 01:00:00");
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy"));

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(new DateTime(2025, 1, 1, 1, 0, 0, DateTimeKind.Unspecified).Date);
        }
    }
}
