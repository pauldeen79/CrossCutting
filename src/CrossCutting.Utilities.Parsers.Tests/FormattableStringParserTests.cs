namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class FormattableStringParserTests : IDisposable
{
    private ServiceProvider? _provider;
    private IServiceScope? _scope;
    private const string ReplacedValue = "replaced name";

    [Fact]
    public void Parse_Throws_On_Null_Input()
    {
        // Arrange
        var sut = CreateSut();

        // Act & Assert
        sut.Invoking(x => x.Parse(input: null!, CultureInfo.InvariantCulture))
           .Should().Throw<ArgumentNullException>().WithParameterName("input");
    }

    [Fact]
    public void Parse_Throws_On_Null_FormatProvider()
    {
        // Arrange
        var sut = CreateSut();

        // Act & Assert
        sut.Invoking(x => x.Parse("some input", formatProvider: null!))
           .Should().Throw<ArgumentNullException>().WithParameterName("formatProvider");
    }

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
        result.Value!.ToString().Should().Be(expectedValue);
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
    public void Parse_Works_With_ExpressionString_Containing_Function()
    {
        // Arrange
        const string Input = "Hello {MyFunction()}!";

        // Act
        var result = CreateSut().Parse(Input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be("Hello function result!");
    }

    [Fact]
    public void Parse_Works_With_ExpressionString_Containing_Nested_Function()
    {
        // Arrange
        const string Input = "Hello {ToUpperCase(MyFunction())}!";

        // Act
        var result = CreateSut().Parse(Input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be("Hello FUNCTION RESULT!");
    }

    [Fact]
    public void Parse_Works_With_ExpressionString()
    {
        // Arrange
        const string Input = "I can add 1 to 2, this results in {1 + 1}";

        // Act
        var result = CreateSut().Parse(Input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be("I can add 1 to 2, this results in 2");
    }

    [Fact]
    public void Parse_Works_With_Placeholder_That_Returns_Another_Placeholder()
    {
        // Arrange
        const string Input = "Hello {ReplaceWithPlaceholder}!";
        var sut = CreateSut();

        // Act
        var result = sut.Parse(Input, CultureInfo.InvariantCulture);
        result = sut.Parse(result.GetValueOrThrow().ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture); // have to parse the result, because it contains a new placeholder...

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be($"Hello {ReplacedValue}!");
    }

    [Fact]
    public void Parse_Returns_Invalid_On_Unknown_Placeholder()
    {
        // Arrange
        var input = "{Unknown placeholder}";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Unknown placeholder in value: Unknown placeholder");
    }

    [Fact]
    public void Can_Parse_String_And_Defer_Specific_Placeholder()
    {
        // Arrange
        var sut = CreateSut();
        var preparsedResult = sut.Parse("Hello {Name}, you are called {{Name}}", CultureInfo.InvariantCulture).GetValueOrThrow();

        // Act
        var result = sut.Parse(preparsedResult, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be($"Hello {ReplacedValue}, you are called {ReplacedValue}");
        result.Value.ArgumentCount.Should().Be(1);
        result.Value.GetArgument(0).Should().BeEquivalentTo(ReplacedValue);
    }

    [Fact]
    public void Can_Implicitly_Convert_ParseStringResult_To_String()
    {
        // Arrange
        var sut = CreateSut();
        var parsedResult = sut.Parse("Hello {Name}!", CultureInfo.InvariantCulture);

        // Act
        string result = parsedResult.GetValueOrThrow();

        // Assert
        result.Should().Be("Hello replaced name!");
    }

    [Fact]
    public void Can_Implicitly_Convert_Null_ParseStringResult_To_String()
    {
        // Arrange
        var parsedResult = default(FormattableStringParserResult);

        // Act
        string result = parsedResult!;

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Create_Throws_On_Null_Processors()
    {
        // Act & Assert
        this.Invoking(_ => FormattableStringParser.Create(processors: null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("processors");
    }

    [Fact]
    public void Create_Returns_New_Instance_With_Correct_Configuration()
    {
        // Arrange
        _ = CreateSut();

        // Act
        var result = FormattableStringParser.Create(new MyPlaceholderProcessor());

        // Assert
        result.Should().BeOfType<FormattableStringParser>();
    }

    [Fact]
    public void FromString_Creates_New_Instance_From_String_Correclty()
    {
        // Act
        var instance = FormattableStringParserResult.FromString("hello world");

        // Assert
        instance.Format.Should().Be("{0}");
        instance.ToString().Should().Be("hello world");
    }

    [Fact]
    public void FromString_Creates_New_Instance_From_String_With_Accolades_Correclty()
    {
        // Act
        var instance = FormattableStringParserResult.FromString("hello {world}");

        // Assert
        instance.Format.Should().Be("{0}");
        instance.ToString().Should().Be("hello {world}");
    }

    [Fact]
    public void ToString_Override_Returns_Correct_Result()
    {
        // Arrange
        FormattableStringParserResult result = "Hello world!";

        // Act
        var stringResult = result.ToString();

        // Assert
        stringResult.Should().Be("Hello world!");
    }

    [Fact]
    public void Implicit_Operator_Returns_Correct_Result()
    {
        // Arrange
        FormattableStringParserResult result = "Hello world!";

        // Act
        string stringResult = result;

        // Assert
        stringResult.Should().Be("Hello world!");
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
            .AddSingleton<IFunctionResultParser, ToUpperCaseResultParser>()
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
        return _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>();
    }

    private sealed class MyPlaceholderProcessor : IPlaceholderProcessor
    {
        public int Order => 10;

        public Result<FormattableStringParserResult> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
        {
            return value switch
            {
                "Name" => Result.Success<FormattableStringParserResult>(ReplacedValue),
                "Context" => Result.Success<FormattableStringParserResult>(context.ToStringWithDefault()),
                "Unsupported placeholder" => Result.Error<FormattableStringParserResult>($"Unsupported placeholder name: {value}"),
                "ReplaceWithPlaceholder" => Result.Success<FormattableStringParserResult>("{Name}"),
                _ => Result.Continue<FormattableStringParserResult>()
            };
        }
    }

    private sealed class MyFunctionResultParser : IFunctionResultParser
    {
        public Result<object?> Parse(FunctionParseResult functionParseResult, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
        {
            if (functionParseResult.FunctionName != "MyFunction")
            {
                return Result.Continue<object?>();
            }

            return Result.Success<object?>("function result");
        }
    }

    private sealed class ToUpperCaseResultParser : IFunctionResultParser
    {
        public Result<object?> Parse(FunctionParseResult functionParseResult, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
        {
            if (functionParseResult.FunctionName != "ToUpperCase")
            {
                return Result.Continue<object?>();
            }

            var valueResult = functionParseResult.Arguments.First().GetValueResult(context, evaluator, parser, functionParseResult.FormatProvider);
            if (!valueResult.IsSuccessful())
            {
                return valueResult;
            }

            return Result.Success<object?>(valueResult.Value.ToStringWithDefault().ToUpperInvariant());
        }
    }
}
