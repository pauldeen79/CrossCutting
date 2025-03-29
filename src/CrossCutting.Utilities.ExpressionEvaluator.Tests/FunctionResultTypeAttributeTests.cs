namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class FunctionResultTypeAttributeTests
{
    [Fact]
    public void Should_Construct()
    {
        // Act & Assert
        Action a = () => _ = new FunctionResultTypeAttribute(typeof(string));
        a.ShouldNotThrow();
    }

    [Fact]
    public void Should_Fill_Type_Correctly()
    {
        // Act
        var sut = new FunctionResultTypeAttribute(typeof(string));

        // Assert
        sut.Type.ShouldBe(typeof(string));
    }
}
