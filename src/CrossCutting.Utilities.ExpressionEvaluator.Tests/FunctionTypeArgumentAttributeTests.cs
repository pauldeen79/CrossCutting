namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class FunctionTypeArgumentAttributeTests
{
    [Fact]
    public void Should_Construct_1()
    {
        // Act & Assert
        Action a = () => _ = new MemberTypeArgumentAttribute("Name");
        a.ShouldNotThrow();
    }

    [Fact]
    public void Should_Construct_2()
    {
        // Act & Assert
        Action a = () => _ = new MemberTypeArgumentAttribute("Name", "Description");
        a.ShouldNotThrow();
    }
}
