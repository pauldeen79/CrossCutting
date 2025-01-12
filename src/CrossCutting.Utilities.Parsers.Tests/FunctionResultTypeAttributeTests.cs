namespace CrossCutting.Utilities.Parsers.Tests;

public class FunctionResultTypeAttributeTests
{
    [Fact]
    public void Should_Construct()
    {
        // Act & Assert
        this.Invoking(_ => new FunctionResultTypeAttribute(typeof(string))).Should().NotThrow();
    }

    [Fact]
    public void Should_Fill_Type_Correctly()
    {
        // Act
        var sut = new FunctionResultTypeAttribute(typeof(string));

        // Assert
        sut.Type.Should().Be<string>();
    }
}
