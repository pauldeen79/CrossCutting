namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public sealed class IntegrationTests : TestBase, IDisposable
{
    [Fact]
    public void Can_Evaluate_Binary_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "true && true && \"string value\"";

        // Act
        var result = sut.Evaluate(CreateContext(expression));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Evaluate_Comparison_Operator_Expression()
    {
        // Arrange
        var sut = CreateSut();
        var expression = "2 > 1";

        // Act
        var result = sut.Evaluate(CreateContext(expression));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    public IntegrationTests()
    {
        Provider = new ServiceCollection().AddExpressionEvaluator().BuildServiceProvider();
        Scope = Provider.CreateScope();
    }

    private ServiceProvider Provider { get; set; }
    private IServiceScope Scope { get; set; }

    private IExpressionEvaluator CreateSut() => Scope.ServiceProvider.GetRequiredService<IExpressionEvaluator>();

    public void Dispose()
    {
        Scope.Dispose();
        Provider.Dispose();
    }
}
