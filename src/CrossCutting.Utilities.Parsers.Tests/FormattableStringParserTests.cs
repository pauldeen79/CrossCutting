namespace CrossCutting.Utilities.Parsers.Tests;

public class FormattableStringParserTests
{
    private const string ReplacedValue = "replaced name";

    [Fact]
    public void Parse_Returns_Invalid_When_Using_Nested_Open_Signs()
    {
        // Arrange
        var input = "Hello {Name {nested}} you are welcome";

        // Act
        var result = CreateSut().Parse(input);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Parse_Returns_Invalid_When_Open_Sign_Is_Missing()
    {
        // Arrange
        var input = "}";

        // Act
        var result = CreateSut().Parse(input);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Parse_Returns_Invalid_When_Close_Sign_Is_Missing()
    {
        // Arrange
        var input = "{";

        // Act
        var result = CreateSut().Parse(input);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Parse_Returns_Result_From_ProcessDelegate_When_Not_Successful()
    {
        // Arrange
        var input = "{Unsupported placeholder}";

        // Act
        var result = CreateSut().Parse(input);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Unsupported placeholder name: Unsupported placeholder");
    }

    [Theory,
        InlineData("Hello {Name}!", $"Hello {ReplacedValue}!"),
        InlineData("Hello {{Name}}!", "Hello {{Name}}!"),
        InlineData("Data without accolades", "Data without accolades")]
    public void Parse_Returns_Success_On_Valid_Input(string input, string expectedValue)
    {
        // Act
        var result = CreateSut().Parse(input);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(expectedValue);
    }

    private FormattableStringParser CreateSut() => new(new MyPlaceholderProcessor());

    private sealed class MyPlaceholderProcessor : IPlaceholderProcessor
    {
        public Result<string> Process(string value)
            => value == "Name"
                ? Result<string>.Success(ReplacedValue)
                : Result<string>.Error($"Unsupported placeholder name: {value}");
    }
}
