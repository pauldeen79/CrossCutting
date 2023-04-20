namespace CrossCutting.Utilities.Parsers.Tests;

public class FormattableStringParserTests
{
    private const string ReplacedValue = "replaced name";
    [Fact]
    public void Parse_Returns_NotSupported_When_Using_Nested_Open_Signs()
    {
        // Arrange
        var input = "Hello {Name {nested}} you are welcome";

        // Act
        var result = FormattableStringParser.Parse(input, ProcessPlaceholder);

        // Assert
        result.Status.Should().Be(ResultStatus.NotSupported);
    }

    [Fact]
    public void Parse_Returns_Invalid_When_Open_Sign_Is_Missing()
    {
        // Arrange
        var input = "}";

        // Act
        var result = FormattableStringParser.Parse(input, ProcessPlaceholder);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Parse_Returns_Invalid_When_Close_Sign_Is_Missing()
    {
        // Arrange
        var input = "{";

        // Act
        var result = FormattableStringParser.Parse(input, ProcessPlaceholder);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Parse_Returns_Result_From_ProcessDelegate_When_Not_Successful()
    {
        // Arrange
        var input = "{Unsupported placeholder}";

        // Act
        var result = FormattableStringParser.Parse(input, ProcessPlaceholder);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Unsupported placeholder name: Unsupported placeholder");
    }

    [Theory,
        InlineData("Hello {Name}!", $"Hello {ReplacedValue}!"),
        InlineData("Hello {{Name}}!", "Hello {{Name}}!")]
    public void Parse_Returns_Success_On_Valid_Input(string input, string expectedValue)
    {
        // Act
        var result = FormattableStringParser.Parse(input, ProcessPlaceholder);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(expectedValue);
    }

    private Result<string> ProcessPlaceholder(string arg)
    {
        if (arg == "Name")
        {
            return Result<string>.Success(ReplacedValue);
        }

        return Result<string>.Error($"Unsupported placeholder name: {arg}");
    }
}
