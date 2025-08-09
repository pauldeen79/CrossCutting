namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public class CrossCuttingTestingTests : TestBase
{
    [Fact]
    public async Task Can_Create_ExpressionEvaluator_And_Evaluate_Expression()
    {
        // Just a small test, to see if we can easily replace the system time with a fixed datetime value, that is injected in the base class of the test.

        // Act
        var now = await Evaluator.EvaluateAsync(CreateContext("DateTime.Now"));

        // Assert
        now.Status.ShouldBe(ResultStatus.Ok);
        now.Value.ShouldBe(CurrentDateTime);
    }
}
