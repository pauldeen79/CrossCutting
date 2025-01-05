namespace CrossCutting.Utilities.Parsers.Tests;

public class FunctionArgumentAttributeTests
{
    [Fact]
    public void Should_Construct_1()
    {
        // Act & Assert
        this.Invoking(_ => new FunctionArgumentAttribute("Name", typeof(string))).Should().NotThrow();
    }

    [Fact]
    public void Should_Construct_2()
    {
        // Act & Assert
        this.Invoking(_ => new FunctionArgumentAttribute("Name", typeof(string), "Description")).Should().NotThrow();
    }

    [Fact]
    public void Should_Construct_3()
    {
        // Act & Assert
        this.Invoking(_ => new FunctionArgumentAttribute("Name", typeof(string), true)).Should().NotThrow();
    }

    [Fact]
    public void Should_Construct_4()
    {
        // Act & Assert
        this.Invoking(_ => new FunctionArgumentAttribute("Name", typeof(string), "Description", true)).Should().NotThrow();
    }
}
