using CrossCutting.Common.Extensions;

namespace CrossCutting.Utilities.Parsers.Tests;

public class ExpressionStringEvaluatorTests : IDisposable
{
    private const string ReplacedValue = "replaced name";
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;
    private readonly IVariable _variable;
    private bool disposedValue;

    public ExpressionStringEvaluatorTests()
    {
        _variable = Substitute.For<IVariable>();
        _provider = new ServiceCollection()
            .AddParsers()
            .AddSingleton(_variable)
            .AddSingleton<IPlaceholder, MyPlaceholderProcessor>()
            .AddSingleton<IFunction, MyFunction>()
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
    }

    public class Parse : ExpressionStringEvaluatorTests
    {
        [Fact]
        public void Returns_Success_With_Input_Value_On_Empty_String()
        {
            // Arrange
            var input = string.Empty;

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(input);
        }

        [Fact]
        public void Returns_Invali_On_Null_String()
        {
            // Arrange
            var input = default(string);

            // Act
            var result = CreateSut().Evaluate(input!, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Input is required");
        }

        [Fact]
        public void Returns_Success_When_Input_Only_Contains_Equals_Sign()
        {
            // Arrange
            var input = "=";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(input);
        }

        [Fact]
        public void Returns_Success_With_Input_Value_On_String_That_Does_Not_Start_With_Equals_Sign()
        {
            // Arrange
            var input = "string that does not begin with =";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(input);
        }

        [Fact]
        public void Returns_Success_Result_From_Mathematic_Expression_When_Found()
        {
            // Arrange
            var input = "=1+1";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(2);
        }

        [Fact]
        public void Returns_Success_Result_From_Variable_Expression_When_Found()
        {
            // Arrange
            var input = "=$myvariable";
            _variable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(x => x.ArgAt<string>(0) == "myvariable" ? Result.Success<object?>("MyVariableValue") : Result.Continue<object?>());

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("MyVariableValue");
        }

        [Fact]
        public void Returns_Failure_From_Mathemetic_Expression_When_Found()
        {
            // Arrange
            var input = "=1+error";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.NotSupported);
            result.ErrorMessage.Should().Be("Unknown expression type found in fragment: error");
        }

        [Fact]
        public void Returns_Success_Result_From_Formattable_String_When_Found_And_FormattableStringProcessor_Is_Not_Null()
        {
            // Arrange
            var input = "=@\"Hello {Name}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture, _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("Hello replaced name!");
        }

        [Fact]
        public void Returns_Formattable_String_Unprocessed_When_Found_And_FormattableStringProcessor_Is_Null()
        {
            // Arrange
            var input = "=@\"Hello {Name}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("Hello {Name}!");
        }

        [Fact]
        public void Returns_Failure_Result_From_Formattable_String_When_Found()
        {
            // Arrange
            var input = "=@\"Hello {Kaboom}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture, _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Unsupported placeholder name: Kaboom");
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_When_Found()
        {
            // Arrange
            var input = "=\"Hello {Name}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("Hello {Name}!");
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_With_Pipe_Sign_When_Found()
        {
            // Arrange
            var input = "=\"Hello | {Name}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("Hello | {Name}!");
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_With_Ampersand_When_Found()
        {
            // Arrange
            var input = "=\"Hello & {Name}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("Hello & {Name}!");
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_With_Equals_Operator_When_Found_More_Than_Two_Times()
        {
            // Arrange
            var input = "=\"a\" == \"b\" == \"c\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("a\" == \"b\" == \"c");
        }

        [Fact]
        public void Returns_Success_Result_From_Function_When_Found()
        {
            // Arrange
            var input = "=MYFUNCTION()";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("result of MYFUNCTION function");
        }

        [Fact]
        public void Returns_Success_Result_From_Function_With_Formattable_String_As_Argument()
        {
            // Arrange
            var input = "=MYFUNCTION2(@\"Hello {Name}!\")";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture, _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("result of MYFUNCTION2 function: Hello replaced name!");
        }

        [Fact]
        public void Returns_Failure_Result_From_Function_When_Found()
        {
            // Arrange
            var input = "=error()";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Returns_Success_With_Input_String_When_No_Mathematic_Expression_Or_Formattable_String_Or_Function_Was_Found()
        {
            // Arrange
            var input = "some string that does not start with = sign";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(input);
        }

        [Fact]
        public void Returns_Success_With_Input_String_When_String_Starts_With_Equals_Sign_But_No_Other_Expression_Was_Found_After_This()
        {
            // Arrange
            var input = "=\"some string that starts with = sign but does not contain any formattable string, function or mathematical expression\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be(input[2..^1]);
        }

        [Fact]
        public void Returns_Success_Result_From_Piped_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello {Name}!\" | ToUpper(context)";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("HELLO {NAME}!");
        }

        [Fact]
        public void Returns_Success_Result_From_Concatenated_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello \" & \"Name!\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("Hello Name!");
        }

        [Fact]
        public void Returns_Success_Result_From_Concatenated_Expression_Containing_Formattable_String_When_Found()
        {
            // Arrange
            var input = "=\"Hello \" & @\"{Name}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture, _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("Hello replaced name!");
        }

        [Fact]
        public void Returns_Success_Result_From_Concatenated_And_Piped_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello \" & \"{Name}!\" | ToUpper(context)";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("HELLO {NAME}!");
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_String_Expressions()
        {
            // Arrange
            var input = "=\"Hello\" == \"Hello\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Non_String_Expressions()
        {
            // Arrange
            var input = "=1 == 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null == \"Hello\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=\"Hello\" == null";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Left_And_Righ_Null()
        {
            // Arrange
            var input = "=null == null";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Error_Result_From_Equals_Operator_On_Left_Expression_When_Found()
        {
            // Arrange
            var input = "=error() == \"Hello\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
        }

        [Fact]
        public void Returns_Error_Result_From_Equals_Operator_On_Right_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello\" == error()";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
        }

        [Fact]
        public void Returns_Success_Result_From_NotEquals_Operator_When_Found_With_String_Expressions()
        {
            // Arrange
            var input = "=\"Hello\" != \"Hello\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null > 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 > null";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 > 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Invalid_Result_From_Greater_Than_Operator_When_Found_With_No_Nulls_Different_Type()
        {
            // Arrange
            var input = "=\"hello\" > 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Object must be of type String.");
        }

        [Fact]
        public void Returns_Success_Result_From_GreaterThanOrEqual_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null >= 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 >= null";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 >= 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Invalid_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_No_Nulls_Different_Type()
        {
            // Arrange
            var input = "=\"hello\" >= 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Object must be of type String.");
        }

        [Fact]
        public void Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null < 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 < null";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 < 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Invalid_Result_From_Smaller_Than_Operator_When_Found_With_No_Nulls_Different_Type()
        {
            // Arrange
            var input = "=\"hello\" < 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Object must be of type String.");
        }

        [Fact]
        public void Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null <= 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 <= null";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 <= 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Invalid_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_No_Nulls_Different_Type()
        {
            // Arrange
            var input = "=\"hello\" <= 2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Object must be of type String.");
        }

        [Fact]
        public void Returns_Invalid_When_FunctionParser_Returns_Invalid()
        {
            // Arrange
            var input = "=somefunction(\uE002)";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Input cannot contain \uE002, as this is used internally for formatting");
        }

        [Fact]
        public void Returns_Failure_From_FunctionParser_When_FunctionName_Is_Missing()
        {
            // Arrange
            var input = "=()";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.NotFound);
            result.ErrorMessage.Should().Be("No function name found");
        }

        [Fact]
        public void Returns_NotSupported_When_FunctionParseResult_Could_Not_Be_Understood()
        {
            // Arrange
            using var provider = new ServiceCollection()
                .AddParsers()
                .AddSingleton<IPlaceholder, MyPlaceholderProcessor>()
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();
            var input = "=MYFUNCTION()";

            // Act
            var result = scope.ServiceProvider.GetRequiredService<IExpressionStringEvaluator>().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.NotSupported);
            result.ErrorMessage.Should().Be("Unknown function: MYFUNCTION");
        }

        [Fact]
        public void Can_Escape_Equals_Sign_To_Skip_ExpressionString_Parsing()
        {
            // Arrange
            var input = "\'=error()";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("=error()");
        }

        [Theory,
            InlineData("=ToUpperCase(\"  space  \")"),
            InlineData("=ToUpperCase(@\"  space  \")")]
        public void Function_With_String_Argument_Preserves_WhiteSpace(string input)
        {
            // Arrange
            var functionResultParserMock = Substitute.For<IFunction>();
            functionResultParserMock.Evaluate(Arg.Any<FunctionCall>(), Arg.Any<IFunctionEvaluator>(), Arg.Any<IExpressionEvaluator>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                .Returns(x =>
                {
                    if (x.ArgAt<FunctionCall>(0).Name != "ToUpperCase")
                    {
                        return Result.Continue<object?>();
                    }

                    return Result.Success<object?>(x.ArgAt<FunctionCall>(0).GetArgumentStringValueResult(0, "expression", x.ArgAt<IFormatProvider>(3), x.ArgAt<object?>(4), x.ArgAt<IFunctionEvaluator>(1), x.ArgAt<IExpressionEvaluator>(2)).GetValueOrThrow().ToUpperInvariant());
                });
            using var provider = new ServiceCollection()
                .AddParsers()
                .AddSingleton<IPlaceholder, MyPlaceholderProcessor>()
                .AddSingleton(functionResultParserMock)
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();

            var parser = scope.ServiceProvider.GetRequiredService<IExpressionStringEvaluator>();

            // Act
            var result = parser.Evaluate(input, CultureInfo.InvariantCulture, scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo("  SPACE  ");
        }
    }

    public class Validate : ExpressionStringEvaluatorTests
    {
        [Fact]
        public void Returns_Success_With_Input_Value_On_Empty_String()
        {
            // Arrange
            var input = string.Empty;

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Invali_On_Null_String()
        {
            // Arrange
            var input = default(string);

            // Act
            var result = CreateSut().Validate(input!, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Input is required");
        }

        [Fact]
        public void Returns_Success_When_Input_Only_Contains_Equals_Sign()
        {
            // Arrange
            var input = "=";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_With_Input_Value_On_String_That_Does_Not_Start_With_Equals_Sign()
        {
            // Arrange
            var input = "string that does not begin with =";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Mathematic_Expression_When_Found()
        {
            // Arrange
            var input = "=1+1";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Variable_Expression_When_Found()
        {
            // Arrange
            var input = "=$myvariable";
            _variable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(x => x.ArgAt<string>(0) == "myvariable" ? Result.Success<object?>("MyVariableValue") : Result.Continue<object?>());

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Failure_From_Mathemetic_Expression_When_Found()
        {
            // Arrange
            var input = "=1+error";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.NotSupported);
            result.ErrorMessage.Should().Be("Unknown expression type found in fragment: error");
        }

        [Fact]
        public void Returns_Success_Result_From_Formattable_String_When_Found_And_FormattableStringProcessor_Is_Not_Null()
        {
            // Arrange
            var input = "=@\"Hello {Name}!\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture, _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Formattable_String_Unprocessed_When_Found_And_FormattableStringProcessor_Is_Null()
        {
            // Arrange
            var input = "=@\"Hello {Name}!\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Failure_Result_From_Formattable_String_When_Found()
        {
            // Arrange
            var input = "=@\"Hello {Kaboom}!\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture, _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Validation failed, see inner results for details");
            result.InnerResults.Single().ErrorMessage.Should().Be("Unsupported placeholder name: Kaboom");
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_When_Found()
        {
            // Arrange
            var input = "=\"Hello {Name}!\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_With_Pipe_Sign_When_Found()
        {
            // Arrange
            var input = "=\"Hello | {Name}!\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_With_Ampersand_When_Found()
        {
            // Arrange
            var input = "=\"Hello & {Name}!\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_With_Equals_Operator_When_Found_More_Than_Two_Times()
        {
            // Arrange
            var input = "=\"a\" == \"b\" == \"c\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Function_When_Found()
        {
            // Arrange
            var input = "=MYFUNCTION()";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Function_With_Formattable_String_As_Argument()
        {
            // Arrange
            var input = "=MYFUNCTION2(@\"Hello {Name}!\")";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture, _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_With_Input_String_When_No_Mathematic_Expression_Or_Formattable_String_Or_Function_Was_Found()
        {
            // Arrange
            var input = "some string that does not start with = sign";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_With_Input_String_When_String_Starts_With_Equals_Sign_But_No_Other_Expression_Was_Found_After_This()
        {
            // Arrange
            var input = "=\"some string that starts with = sign but does not contain any formattable string, function or mathematical expression\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Piped_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello {Name}!\" | ToUpper(context)";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Concatenated_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello \" & \"Name!\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Concatenated_Expression_Containing_Formattable_String_When_Found()
        {
            // Arrange
            var input = "=\"Hello \" & @\"{Name}!\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture, _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Concatenated_And_Piped_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello \" & \"{Name}!\" | ToUpper(context)";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_String_Expressions()
        {
            // Arrange
            var input = "=\"Hello\" == \"Hello\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Non_String_Expressions()
        {
            // Arrange
            var input = "=1 == 2";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null == \"Hello\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=\"Hello\" == null";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Left_And_Righ_Null()
        {
            // Arrange
            var input = "=null == null";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_NotEquals_Operator_When_Found_With_String_Expressions()
        {
            // Arrange
            var input = "=\"Hello\" != \"Hello\"";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null > 2";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 > null";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 > 2";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_GreaterThanOrEqual_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null >= 2";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 >= null";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 >= 2";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null < 2";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 < null";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 < 2";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null <= 2";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 <= null";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 <= 2";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Invalid_When_FunctionParser_Returns_Invalid()
        {
            // Arrange
            var input = "=somefunction(\uE002)";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Input cannot contain \uE002, as this is used internally for formatting");
        }

        [Fact]
        public void Returns_Failure_From_FunctionParser_When_FunctionName_Is_Missing()
        {
            // Arrange
            var input = "=()";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.NotFound);
            result.ErrorMessage.Should().Be("No function name found");
        }

        [Fact]
        public void Returns_Invalid_When_FunctionParseResult_Could_Not_Be_Understood()
        {
            // Arrange
            using var provider = new ServiceCollection()
                .AddParsers()
                .AddSingleton<IPlaceholder, MyPlaceholderProcessor>()
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();
            var input = "=MYFUNCTION()";

            // Act
            var result = scope.ServiceProvider.GetRequiredService<IExpressionStringEvaluator>().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("Unknown function: MYFUNCTION");
        }

        [Fact]
        public void Can_Escape_Equals_Sign_To_Skip_ExpressionString_Parsing()
        {
            // Arrange
            var input = "\'=error()";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Theory,
            InlineData("=ToUpperCase(\"  space  \")"),
            InlineData("=ToUpperCase(@\"  space  \")")]
        public void Function_With_String_Argument_Preserves_WhiteSpace(string input)
        {
            // Arrange
            var functionMock = Substitute.For<IFunction>();
            //Validate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
            functionMock.Validate(Arg.Any<FunctionCall>(), Arg.Any<IFunctionEvaluator>(), Arg.Any<IExpressionEvaluator>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                .Returns(x =>
                {
                    if (x.ArgAt<FunctionCall>(0).Name != "ToUpperCase")
                    {
                        return Result.Continue<object?>();
                    }

                    return Result.Success<object?>(x.ArgAt<FunctionCall>(0).GetArgumentStringValueResult(0, "expression", x.ArgAt<IFormatProvider>(3), x.ArgAt<object?>(4), x.ArgAt<IFunctionEvaluator>(1), x.ArgAt<IExpressionEvaluator>(2)).GetValueOrThrow().ToUpperInvariant());
                });
            using var provider = new ServiceCollection()
                .AddParsers()
                .AddSingleton<IPlaceholder, MyPlaceholderProcessor>()
                .AddSingleton(functionMock)
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();

            var parser = scope.ServiceProvider.GetRequiredService<IExpressionStringEvaluator>();

            // Act
            var result = parser.Validate(input, CultureInfo.InvariantCulture, scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }
    }

    private IExpressionStringEvaluator CreateSut() => _scope.ServiceProvider.GetRequiredService<IExpressionStringEvaluator>();

    private sealed class MyPlaceholderProcessor : IPlaceholder
    {
        public int Order => 10;

        public Result<GenericFormattableString> Evaluate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
            => value == "Name"
                ? Result.Success(new GenericFormattableString(ReplacedValue))
                : Result.Error<GenericFormattableString>($"Unsupported placeholder name: {value}");

        public Result Validate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
            => value == "Name"
                ? Result.Success()
                : Result.Error($"Unsupported placeholder name: {value}");
    }

    private sealed class MyFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        {
            if (functionCall.Name == "error")
            {
                return Result.Error<object?>("Kaboom");
            }

            if (functionCall.Name == "ToUpper")
            {
                return Result.Success<object?>(context?.ToString()?.ToUpperInvariant() ?? string.Empty);
            }

            if (functionCall.Arguments.Count > 0)
            {
                return Result.Success<object?>($"result of {functionCall.Name} function: {string.Join(", ", functionCall.Arguments.OfType<LiteralArgument>().Select(x => x.GetValueResult(context, functionEvaluator, expressionEvaluator, formatProvider).GetValueOrThrow()))}");
            }

            return Result.Success<object?>($"result of {functionCall.Name} function");
        }

        public Result Validate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        {
            if (!functionCall.Name.In("error", "ToUpper", "MYFUNCTION", "MYFUNCTION2"))
            {
                return Result.Continue();
            }

            // Aparently, this function does not care about the given arguments
            return Result.Success();
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _scope.Dispose();
                _provider.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
