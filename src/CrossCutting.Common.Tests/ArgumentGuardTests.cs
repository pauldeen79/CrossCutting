namespace CrossCutting.Common.Tests;

public class ArgumentGuardTests
{
    [Fact]
    public void IsNotNull_Returns_Instance_When_Instance_Is_Not_Null()
    {
        // Act
        var result = ArgumentGuard.IsNotNull(this, "instance");

        // Assert
        result.Should().BeSameAs(this);
    }

    [Fact]
    public void IsNotNull_Throws_When_Instance_Is_Null()
    {
        // Act & Assert
        this.Invoking(_ => ArgumentGuard.IsNotNull<string>(default, "the argument name"))
            .Should().Throw<ArgumentNullException>().WithParameterName("the argument name");
    }
}
