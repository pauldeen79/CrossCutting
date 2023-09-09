namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class FormattableStringParserTests : IDisposable
{
    private ServiceProvider? _provider;
    private IServiceScope? _scope;
    private const string ReplacedValue = "replaced name";

    [Fact]
    public void Parse_Returns_Invalid_When_Using_Nested_Open_Signs()
    {
        // Arrange
        var input = "Hello {Name {nested}} you are welcome";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Parse_Returns_Invalid_When_Open_Sign_Is_Missing()
    {
        // Arrange
        var input = "}";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Parse_Returns_Invalid_When_Close_Sign_Is_Missing()
    {
        // Arrange
        var input = "{";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Parse_Returns_Result_From_ProcessDelegate_When_Not_Successful()
    {
        // Arrange
        var input = "{Unsupported placeholder}";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Unsupported placeholder name: Unsupported placeholder");
    }

    [Theory,
        InlineData("Hello {Name}!", $"Hello {ReplacedValue}!"),
        InlineData("Hello {Context}!", $"Hello [value from context]!"),
        InlineData("Hello {{Name}}!", "Hello {Name}!"),
        InlineData("Data without accolades", "Data without accolades"),
        InlineData("public class Bla {{ /* implementation goes here */ }}", "public class Bla { /* implementation goes here */ }"),
        InlineData("public class Bla {{ {Name} }}", $"public class Bla {{ {ReplacedValue} }}")]
    public void Parse_Returns_Success_On_Valid_Input(string input, string expectedValue)
    {
        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture, "[value from context]");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void Parse_Works_With_MultiLine_Template()
    {
        // Arrange
        const string Input = @"        [Fact]
        public void {Name}()
        {{
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.{Context}();

            // Assert
            //TODO
        }}";

        // Act
        var result = CreateSut().Parse(Input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Parse_Works_With_ExpressionString()
    {
        // Arrange
        const string Input = "Hello {MyFunction()}!";

        // Act
        var result = CreateSut().Parse(Input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Hello function result!");
    }

    [Fact]
    public void Parse_Returns_NotSupported_On_Unknown_Placeholder()
    {
        // Arrange
        var input = "{Unknown placeholder}";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotSupported);
        result.ErrorMessage.Should().Be("Unknown placeholder in value: Unknown placeholder");
    }

    public void Dispose()
    {
        _scope?.Dispose();
        _provider?.Dispose();
    }

    private IFormattableStringParser CreateSut()
    {
        _provider = new ServiceCollection()
            .AddParsers()
            .AddSingleton<IPlaceholderProcessor, MyPlaceholderProcessor>()
            .AddSingleton<IFunctionResultParser, MyFunctionResultParser>()
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
        return _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>();
    }

    private sealed class MyPlaceholderProcessor : IPlaceholderProcessor
    {
        public int Order => 10;

        public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
        {
            if (value == "Name")
            {
                return Result<string>.Success(ReplacedValue);
            }

            if (value == "Context")
            {
                return Result<string>.Success(context?.ToString() ?? string.Empty);
            }

            if (value == "Unsupported placeholder")
            {
                return Result<string>.Error($"Unsupported placeholder name: {value}");
            }

            return Result<string>.Continue();
        }
    }

    private sealed class MyFunctionResultParser : IFunctionResultParser
    {
        public Result<object?> Parse(FunctionParseResult functionParseResult, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
        {
            if (functionParseResult.FunctionName != "MyFunction")
            {
                return Result<object?>.Continue();
            }

            return Result<object?>.Success("function result");
        }
    }
}
