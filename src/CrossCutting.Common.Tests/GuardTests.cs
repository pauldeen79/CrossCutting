namespace CrossCutting.Common.Tests;

public class GuardTests
{
    [Fact]
    public void AgainstNull_ValidInput_NoException()
    {
        //Arrange
        object value = new object();
        string argumentName = "test";

        //Act
        Action action = () => Guard.AgainstNull(value, argumentName);

        //Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void AgainstNull_NullInput_ArgumentNullException()
    {
        //Arrange
        object? value = null;
        string argumentName = "test";

        //Act
        Action action = () => Guard.AgainstNull(value, argumentName);

        //Assert
        action.Should().Throw<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{argumentName}')");
    }

    [Fact]
    public void AgainstNull_Generic_ValidInput_ValidOutput()
    {
        //Arrange
        object? value = new object();
        string argumentName = "test";

        //Act
        var output = Guard.AgainstNull<object?>(value, argumentName);

        //Assert
        output.Should().Be(value);
    }

    [Fact]
    public void AgainstNull_Generic_NullInput_ArgumentNullException()
    {
        //Arrange
        object? value = null;
        string argumentName = "test";

        //Act
        Action action = () => Guard.AgainstNull<object?>(value, argumentName);

        //Assert
        action.Should().Throw<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{argumentName}')");
    }
}
