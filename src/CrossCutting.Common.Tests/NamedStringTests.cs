namespace CrossCutting.Common.Tests;

public class NamedStringTests
{
    [Fact]
    public void Can_Replace_Some_Tokens_Using_Format_Function()
    {
        // Arrange
        const string Template = "Hello {Name}, you are {Age} years old. How do you feel?";

        // Act
        var result = NamedString.Format(Template, new { Name = "John Doe", Age = 43 });

        // Assert
        result.Should().Be("Hello John Doe, you are 43 years old. How do you feel?");
    }

    [Fact]
    public void Can_Replace_Tokens_Case_Insensitive_Using_Format_Function()
    {
        // Arrange
        const string Template = "Hello {NAME}, you are {AGE} years old. How do you feel?";

        // Act
        var result = NamedString.Format(Template, new { Name = "John Doe", Age = 43 }, ignoreCase: true);

        // Assert
        result.Should().Be("Hello John Doe, you are 43 years old. How do you feel?");
    }

    [Fact]
    public void By_Default_Format_Function_Is_Case_Sensitive()
    {
        // Arrange
        const string Template = "Hello {NAME}, you are {AGE} years old. How do you feel?";

        // Act
        var result = NamedString.Format(Template, new { Name = "John Doe", Age = 43 });

        // Assert
        result.Should().Be(Template); // no replacements were made
    }
}
