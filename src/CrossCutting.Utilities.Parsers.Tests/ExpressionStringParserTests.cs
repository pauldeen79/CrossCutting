namespace CrossCutting.Utilities.Parsers.Tests;

public class ExpressionStringParserTests
{
    private const string ReplacedValue = "replaced name";

    [Fact]
    public void Parse_Returns_Success_With_Input_Value_On_Empty_String()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(input);
    }

    [Fact]
    public void Parse_Returns_Success_When_Input_Only_Contains_Equals_Sign()
    {
        // Arrange
        var input = "=";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(input);
    }

    [Fact]
    public void Parse_Returns_Success_With_Input_Value_On_String_That_Does_Not_Start_With_Equals_Sign()
    {
        // Arrange
        var input = "string that does not begin with =";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(input);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Mathematic_Expression_When_Found()
    {
        // Arrange
        var input = "=1+1";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(2);
    }

    [Fact]
    public void Parse_Returns_Failure_From_Mathemetic_Expression_When_Found()
    {
        // Arrange
        var input = "=1+error";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Unknown expression type found in fragment: error");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Formattable_String_When_Found()
    {
        // Arrange
        var input = "=\"Hello {Name}!\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Hello replaced name!");
    }

    [Fact]
    public void Parse_Returns_Failure_Result_From_Formattable_String_When_Found()
    {
        // Arrange
        var input = "=\"Hello {Kaboom}!\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Unsupported placeholder name: Kaboom");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Function_When_Found()
    {
        // Arrange
        var input = "=MYFUNCTION()";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("result of MYFUNCTION function");
    }

    [Fact]
    public void Parse_Returns_Failure_Result_From_Function_When_Found()
    {
        // Arrange
        var input = "=error()";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom");
    }

    [Fact]
    public void Parse_Returns_Success_With_Input_String_When_No_Mathematic_Expression_Or_Formattable_String_Or_Function_Was_Found()
    {
        // Arrange
        var input = "some string that does not start with = sign";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(input);
    }

    [Fact]
    public void Parse_Returns_Success_With_Input_String_When_String_Starts_With_Equals_Sign_But_No_Other_Expressoin_Was_Found_After_This()
    {
        // Arrange
        var input = "=\"some string that starts with = sign but does not contain any formattable string, function or mathematical expression\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(input.Substring(2, input.Length - 3));
    }

    [Fact]
    public void Parse_Returns_NotSupported_When_FunctionParser_Returns_NotSupported()
    {
        // Arrange
        var input = "=somefunction(^^)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotSupported);
        result.ErrorMessage.Should().Be("Input cannot contain ^^, as this is used internally for formatting");
    }

    [Fact]
    public void Parse_Returns_Failure_From_FunctionParser_When_FunctionName_Is_Missing()
    {
        // Arrange
        var input = "=()";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("No function name found");
    }

    private ExpressionStringParser CreateSut() => new(
        new MyFunctionResultParser(),
        new IExpressionStringParserProcessor[]
        {
            new EmptyExpressionProcessor(),
            new LiteralExpressionProcessor(),
            new OnlyEqualsExpressionProcessor(),
            new FormattableStringExpressionProcessor(new FormattableStringParser(new MyPlaceholderProcessor())),
            new MathematicExpressionProcessor(new MathematicExpressionParser(new DefaultExpressionParser()))
        }
    );

    private sealed class MyPlaceholderProcessor : IPlaceholderProcessor
    {
        public Result<string> Process(string value)
            => value == "Name"
                ? Result<string>.Success(ReplacedValue)
                : Result<string>.Error($"Unsupported placeholder name: {value}");
    }

    private sealed class MyFunctionResultParser : IFunctionResultParser
    {
        public Result<object> Parse(FunctionParseResult functionParseResult)
            => functionParseResult.FunctionName == "error"
                ? Result<object>.Error("Kaboom")
                : Result<object>.Success($"result of {functionParseResult.FunctionName} function");
    }
}
