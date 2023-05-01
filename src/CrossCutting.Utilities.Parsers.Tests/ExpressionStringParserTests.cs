namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class ExpressionStringParserTests : IDisposable
{
    private const string ReplacedValue = "replaced name";
    private readonly ServiceProvider _provider;

    public ExpressionStringParserTests()
    {
        _provider = new ServiceCollection()
            .AddParsers()
            .AddSingleton<IPlaceholderProcessor, MyPlaceholderProcessor>()
            .AddSingleton<IFunctionResultParser, MyFunctionResultParser>()
            .BuildServiceProvider();
    }

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
        result.Status.Should().Be(ResultStatus.NotSupported);
        result.ErrorMessage.Should().Be("Unknown expression type found in fragment: error");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Formattable_String_When_Found()
    {
        // Arrange
        var input = "=@\"Hello {Name}!\"";

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
        var input = "=@\"Hello {Kaboom}!\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Unsupported placeholder name: Kaboom");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Literal_String_When_Found()
    {
        // Arrange
        var input = "=\"Hello {Name}!\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Hello {Name}!");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Literal_String_With_Pipe_Sign_When_Found()
    {
        // Arrange
        var input = "=\"Hello | {Name}!\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Hello | {Name}!");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Literal_String_With_Ampersand_When_Found()
    {
        // Arrange
        var input = "=\"Hello & {Name}!\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Hello & {Name}!");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Literal_String_With_Equals_Operator_When_Found_More_Than_Two_Times()
    {
        // Arrange
        var input = "=\"a\" == \"b\" == \"c\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("a\" == \"b\" == \"c");
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
    public void Parse_Returns_Success_Result_From_Function_With_Formattable_String_As_Argument()
    {
        // Arrange
        var input = "=MYFUNCTION2(@\"Hello {Name}!\")";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("result of MYFUNCTION2 function: Hello replaced name!");
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
    public void Parse_Returns_Success_Result_From_Piped_Expression_When_Found()
    {
        // Arrange
        var input = "=\"Hello {Name}!\" | ToUpper(context)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("HELLO {NAME}!");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Concatenated_Expression_When_Found()
    {
        // Arrange
        var input = "=\"Hello \" & \"{Name}!\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Hello {Name}!");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Concatenated_And_Piped_Expression_When_Found()
    {
        // Arrange
        var input = "=\"Hello \" & \"{Name}!\" | ToUpper(context)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("HELLO {NAME}!");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Equals_Operator_When_Found_With_String_Expressions()
    {
        // Arrange
        var input = "=\"Hello\" == \"Hello\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(true);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Equals_Operator_When_Found_With_Non_String_Expressions()
    {
        // Arrange
        var input = "=1 == 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Equals_Operator_When_Found_With_Left_Null()
    {
        // Arrange
        var input = "=null == \"Hello\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Equals_Operator_When_Found_With_Right_Null()
    {
        // Arrange
        var input = "=\"Hello\" == null";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Equals_Operator_When_Found_With_Left_And_Righ_Null()
    {
        // Arrange
        var input = "=null == null";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(true);
    }

    [Fact]
    public void Parse_Returns_Error_Result_From_Equals_Operator_On_Left_Expression_When_Found()
    {
        // Arrange
        var input = "=error() == \"Hello\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
    }

    [Fact]
    public void Parse_Returns_Error_Result_From_Equals_Operator_On_Right_Expression_When_Found()
    {
        // Arrange
        var input = "=\"Hello\" == error()";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_NotEquals_Operator_When_Found_With_String_Expressions()
    {
        // Arrange
        var input = "=\"Hello\" != \"Hello\"";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_Left_Null()
    {
        // Arrange
        var input = "=null > 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_Right_Null()
    {
        // Arrange
        var input = "=2 > null";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_No_Nulls_Same_Type()
    {
        // Arrange
        var input = "=3 > 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(true);
    }

    [Fact]
    public void Parse_Returns_Invalid_Result_From_Greater_Than_Operator_When_Found_With_No_Nulls_Different_Type()
    {
        // Arrange
        var input = "=\"hello\" > 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Object must be of type String.");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_GreaterThanOrEqual_Than_Operator_When_Found_With_Left_Null()
    {
        // Arrange
        var input = "=null >= 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_Right_Null()
    {
        // Arrange
        var input = "=2 >= null";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_No_Nulls_Same_Type()
    {
        // Arrange
        var input = "=3 >= 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(true);
    }

    [Fact]
    public void Parse_Returns_Invalid_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_No_Nulls_Different_Type()
    {
        // Arrange
        var input = "=\"hello\" >= 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Object must be of type String.");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_Left_Null()
    {
        // Arrange
        var input = "=null < 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_Right_Null()
    {
        // Arrange
        var input = "=2 < null";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_No_Nulls_Same_Type()
    {
        // Arrange
        var input = "=3 < 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Invalid_Result_From_Smaller_Than_Operator_When_Found_With_No_Nulls_Different_Type()
    {
        // Arrange
        var input = "=\"hello\" < 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Object must be of type String.");
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_Left_Null()
    {
        // Arrange
        var input = "=null <= 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_Right_Null()
    {
        // Arrange
        var input = "=2 <= null";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_No_Nulls_Same_Type()
    {
        // Arrange
        var input = "=3 <= 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Returns_Invalid_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_No_Nulls_Different_Type()
    {
        // Arrange
        var input = "=\"hello\" <= 2";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Object must be of type String.");
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

    [Fact]
    public void Parse_Returns_NotSupported_When_FunctionParseResult_Could_Not_Be_Understood()
    {
        // Arrange
        using var provider = new ServiceCollection()
            .AddParsers()
            .AddSingleton<IPlaceholderProcessor, MyPlaceholderProcessor>()
            .BuildServiceProvider();
        var input = "=MYFUNCTION()";

        // Act
        var result = provider.GetRequiredService<IExpressionStringParser>().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotSupported);
        result.ErrorMessage.Should().Be("Unknown function found: MYFUNCTION");
    }

    [Fact]
    public void Can_Escape_Equals_Sign_To_Skip_ExpressionString_Parsing()
    {
        // Arrange
        var input = "\'=error()";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("=error()");
    }

    private IExpressionStringParser CreateSut() => _provider.GetRequiredService<IExpressionStringParser>();

    public void Dispose() => _provider.Dispose();

    private sealed class MyPlaceholderProcessor : IPlaceholderProcessor
    {
        public int Order => 10;

        public Result<string> Process(string value, IFormatProvider formatProvider, object? context)
            => value == "Name"
                ? Result<string>.Success(ReplacedValue)
                : Result<string>.Error($"Unsupported placeholder name: {value}");
    }

    private sealed class MyFunctionResultParser : IFunctionResultParser
    {
        public Result<object?> Parse(FunctionParseResult functionParseResult, object? context, IFunctionParseResultEvaluator evaluator)
        {
            if (functionParseResult.FunctionName == "error")
            {
                return Result<object?>.Error("Kaboom");
            }

            if (functionParseResult.FunctionName == "ToUpper")
            {
                return Result<object?>.Success(functionParseResult.Context?.ToString()?.ToUpperInvariant() ?? string.Empty);
            }

            if (functionParseResult.Arguments.Any())
            {
                return Result<object?>.Success($"result of {functionParseResult.FunctionName} function: {string.Join(", ", functionParseResult.Arguments.OfType<LiteralArgument>().Select(x => x.Value))}");
            }

            return Result<object?>.Success($"result of {functionParseResult.FunctionName} function");
        }
    }
}
