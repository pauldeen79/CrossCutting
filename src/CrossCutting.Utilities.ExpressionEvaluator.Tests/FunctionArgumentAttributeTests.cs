namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class FunctionArgumentAttributeTests
{
    [Fact]
    public void Should_Construct_1()
    {
        // Act & Assert
        Action a = () => _ = new MemberArgumentAttribute("Name", typeof(string));
        a.ShouldNotThrow();
    }

    [Fact]
    public void Should_Construct_2()
    {
        // Act & Assert
        Action a = () => _ = new MemberArgumentAttribute("Name", typeof(string), "Description");
        a.ShouldNotThrow();
    }

    [Fact]
    public void Should_Construct_3()
    {
        // Act & Assert
        Action a = () => _ = new MemberArgumentAttribute("Name", typeof(string), true);
        a.ShouldNotThrow();
    }

    [Fact]
    public void Should_Construct_4()
    {
        // Act & Assert
        Action a = () => _ = new MemberArgumentAttribute("Name", typeof(string), "Description", true);
        a.ShouldNotThrow();
    }
}
