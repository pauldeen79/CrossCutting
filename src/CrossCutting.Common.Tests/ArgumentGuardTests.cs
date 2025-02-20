namespace CrossCutting.Common.Tests;

public class ArgumentGuardTests
{
    [Fact]
    public void IsNotNull_Returns_Instance_When_Instance_Is_Not_Null()
    {
        // Act
        var result = ArgumentGuard.IsNotNull(this, "instance");

        // Assert
        result.ShouldBeSameAs(this);
    }

    [Fact]
    public void IsNotNull_Throws_When_Instance_Is_Null()
    {
        // Act & Assert
        Action a = () => ArgumentGuard.IsNotNull<string>(default, "the argument name");
        a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("the argument name");
    }
}
