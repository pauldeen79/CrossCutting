namespace CrossCutting.Utilities.Parsers.Tests;

public class ResultAttributeTests
{
    [Fact]
    public void Should_Construct_1()
    {
        // Act & Assert
        this.Invoking(_ => new FunctionResultAttribute(ResultStatus.Ok, typeof(string), "value", "description")).Should().NotThrow();
    }

    [Fact]
    public void Should_Construct_2()
    {
        // Act & Assert
        this.Invoking(_ => new FunctionResultAttribute(ResultStatus.Ok, "description")).Should().NotThrow();
    }

    [Fact]
    public void Should_Construct_3()
    {
        // Act & Assert
        this.Invoking(_ => new FunctionResultAttribute(ResultStatus.Ok)).Should().NotThrow();
    }
}
