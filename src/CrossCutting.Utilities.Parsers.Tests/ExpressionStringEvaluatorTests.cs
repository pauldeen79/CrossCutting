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
            .AddSingleton<IFunction, ToUpperFunction>()
            .AddSingleton<IFunction, ErrorFunction>()
            .AddSingleton<IFunction, MyFunction>()
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
    }

    protected static ExpressionStringEvaluatorSettings CreateSettings()
        => new ExpressionStringEvaluatorSettingsBuilder();

    public class Evaluate : ExpressionStringEvaluatorTests
    {
        [Fact]
        public void Returns_Success_With_Input_Value_On_Empty_String()
        {
            // Arrange
            var input = string.Empty;

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(input);
        }

        [Fact]
        public void Returns_Invalid_On_Null_String()
        {
            // Arrange
            var input = default(string);

            // Act
            var result = CreateSut().Evaluate(input!, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Expression string is required");
        }

        [Fact]
        public void Returns_Success_When_Input_Only_Contains_Equals_Sign()
        {
            // Arrange
            var input = "=";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(input);
        }

        [Fact]
        public void Returns_Success_With_Input_Value_On_String_That_Does_Not_Start_With_Equals_Sign()
        {
            // Arrange
            var input = "string that does not begin with =";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(input);
        }

        [Fact]
        public void Returns_Success_Result_From_Mathematic_Expression_When_Found()
        {
            // Arrange
            var input = "=1+1";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(2);
        }

        [Fact]
        public void Returns_Success_Result_From_Variable_Expression_When_Found()
        {
            // Arrange
            var input = "=$myvariable";
            _variable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(x => x.ArgAt<string>(0) == "myvariable" ? Result.Success<object?>("MyVariableValue") : Result.Continue<object?>());

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("MyVariableValue");
        }

        [Fact]
        public void Returns_Failure_From_Mathemetic_Expression_When_Found()
        {
            // Arrange
            var input = "=1+error";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: error");
        }

        [Fact]
        public void Returns_Success_Result_From_Formattable_String_When_Found_And_FormattableStringProcessor_Is_Not_Null()
        {
            // Arrange
            var input = "=@\"Hello {Name}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings(), _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello replaced name!");
        }

        [Fact]
        public void Returns_Formattable_String_Unprocessed_When_Found_And_FormattableStringProcessor_Is_Null()
        {
            // Arrange
            var input = "=@\"Hello {Name}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello {Name}!");
        }

        [Fact]
        public void Returns_Failure_Result_From_Formattable_String_When_Found()
        {
            // Arrange
            var input = "=@\"Hello {Kaboom}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings(), _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Unsupported placeholder name: Kaboom");
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_When_Found()
        {
            // Arrange
            var input = "=\"Hello {Name}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello {Name}!");
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_With_Pipe_Sign_When_Found()
        {
            // Arrange
            var input = "=\"Hello | {Name}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello | {Name}!");
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_With_Ampersand_When_Found()
        {
            // Arrange
            var input = "=\"Hello & {Name}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello & {Name}!");
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_With_Equals_Operator_When_Found_More_Than_Two_Times()
        {
            // Arrange
            var input = "=\"a\" == \"b\" == \"c\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("a\" == \"b\" == \"c");
        }

        [Fact]
        public void Returns_Success_Result_From_Function_When_Found()
        {
            // Arrange
            var input = "=MYFUNCTION()";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(ReplacedValue);
        }

        [Fact]
        public void Returns_Success_Result_From_Function_With_Formattable_String_As_Argument()
        {
            // Arrange
            var input = "=toupper(@\"Hello {Name}!\")";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings(), _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("HELLO REPLACED NAME!");
        }

        [Fact]
        public void Returns_Failure_Result_From_Function_When_Found()
        {
            // Arrange
            var input = "=error()";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Success_With_Input_String_When_No_Mathematic_Expression_Or_Formattable_String_Or_Function_Was_Found()
        {
            // Arrange
            var input = "some string that does not start with = sign";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(input);
        }

        [Fact]
        public void Returns_Success_With_Input_String_When_String_Starts_With_Equals_Sign_But_No_Other_Expression_Was_Found_After_This()
        {
            // Arrange
            var input = "=\"some string that starts with = sign but does not contain any formattable string, function or mathematical expression\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(input[2..^1]);
        }

        [Fact]
        public void Returns_Success_Result_From_Piped_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello {Name}!\" | ToUpper(context)";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("HELLO {NAME}!");
        }

        [Fact]
        public void Returns_Success_Result_From_Concatenated_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello \" & \"Name!\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello Name!");
        }

        [Fact]
        public void Returns_Success_Result_From_Concatenated_Expression_Containing_Formattable_String_When_Found()
        {
            // Arrange
            var input = "=\"Hello \" & @\"{Name}!\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings(), _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Hello replaced name!");
        }

        [Fact]
        public void Returns_Success_Result_From_Concatenated_And_Piped_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello \" & \"{Name}!\" | ToUpper(context)";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("HELLO {NAME}!");
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_String_Expressions()
        {
            // Arrange
            var input = "=\"Hello\" == \"Hello\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Non_String_Expressions()
        {
            // Arrange
            var input = "=1 == 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null == \"Hello\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=\"Hello\" == null";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Left_And_Righ_Null()
        {
            // Arrange
            var input = "=null == null";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Error_Result_From_Equals_Operator_On_Left_Expression_When_Found()
        {
            // Arrange
            var input = "=error() == \"Hello\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
        }

        [Fact]
        public void Returns_Error_Result_From_Equals_Operator_On_Right_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello\" == error()";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
        }

        [Fact]
        public void Returns_Success_Result_From_NotEquals_Operator_When_Found_With_String_Expressions()
        {
            // Arrange
            var input = "=\"Hello\" != \"Hello\"";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null > 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 > null";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 > 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Invalid_Result_From_Greater_Than_Operator_When_Found_With_No_Nulls_Different_Type()
        {
            // Arrange
            var input = "=\"hello\" > 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Object must be of type String.");
        }

        [Fact]
        public void Returns_Success_Result_From_GreaterThanOrEqual_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null >= 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 >= null";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 >= 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Invalid_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_No_Nulls_Different_Type()
        {
            // Arrange
            var input = "=\"hello\" >= 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Object must be of type String.");
        }

        [Fact]
        public void Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null < 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 < null";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 < 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Invalid_Result_From_Smaller_Than_Operator_When_Found_With_No_Nulls_Different_Type()
        {
            // Arrange
            var input = "=\"hello\" < 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Object must be of type String.");
        }

        [Fact]
        public void Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null <= 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 <= null";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 <= 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Invalid_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_No_Nulls_Different_Type()
        {
            // Arrange
            var input = "=\"hello\" <= 2";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Object must be of type String.");
        }

        [Fact]
        public void Returns_Success_Result_From_CastExpression_When_Found()
        {
            // Arrange
            var input = $"=cast 13 as {typeof(short).FullName}";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe((short)13);
        }

        [Fact]
        public void Returns_Success_Result_From_CastExpression_When_Found_With_Function()
        {
            // Arrange
            var input = $"=cast ToUpper(\"value\") as {typeof(string).FullName}";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("VALUE");
        }

        [Fact]
        public void Returns_Invalid_Result_From_CastExpression_Unknown_Type()
        {
            // Arrange
            var input = "=cast 13 as unknowntype";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown type: unknowntype");
        }

        [Fact]
        public void Returns_Success_Result_From_TypeOfOfExpression_When_Found()
        {
            // Arrange
            var input = $"=typeof({typeof(short).FullName})";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(short));
        }

        [Fact]
        public void Returns_Invalid_Result_From_TypeOfExpression_Unknown_Type()
        {
            // Arrange
            var input = "=typeof(unknowntype)";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown type: unknowntype");
        }

        [Fact]
        public void Returns_Invalid_Result_From_CastExpression_Expression_Evaluation_Failed()
        {
            // Arrange
            var input = $"=cast Error() as {typeof(string).FullName}";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Invalid_When_FunctionParser_Returns_Invalid()
        {
            // Arrange
            var input = "=somefunction(\uE002)";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Input cannot contain \uE002, as this is used internally for formatting");
        }

        [Fact]
        public void Returns_Failure_From_FunctionParser_When_FunctionName_Is_Missing()
        {
            // Arrange
            var input = "=()";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.ErrorMessage.ShouldBe("No function name found");
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
            var result = scope.ServiceProvider.GetRequiredService<IExpressionStringEvaluator>().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown function: MYFUNCTION");
        }

        [Fact]
        public void Can_Escape_Equals_Sign_To_Skip_ExpressionString_Parsing()
        {
            // Arrange
            var input = "\'=error()";

            // Act
            var result = CreateSut().Evaluate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("=error()");
        }

        [Theory,
            InlineData("=ToUpper(\"  space  \")"),
            InlineData("=ToUpper(@\"  space  \")")]
        public void Function_With_String_Argument_Preserves_WhiteSpace(string input)
        {
            // Arrange
            using var provider = new ServiceCollection()
                .AddParsers()
                .AddSingleton<IFunction, ToUpperFunction>()
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();

            var sut = scope.ServiceProvider.GetRequiredService<IExpressionStringEvaluator>();

            // Act
            var result = sut.Evaluate(input, CreateSettings(), scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo("  SPACE  ");
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
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Invalid_On_Null_String()
        {
            // Arrange
            var input = default(string);

            // Act
            var result = CreateSut().Validate(input!, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Expression string is required");
        }

        [Fact]
        public void Returns_Success_When_Input_Only_Contains_Equals_Sign()
        {
            // Arrange
            var input = "=";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_With_Input_Value_On_String_That_Does_Not_Start_With_Equals_Sign()
        {
            // Arrange
            var input = "string that does not begin with =";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(string));
        }

        [Fact]
        public void Returns_Success_Result_From_Mathematic_Expression_When_Found()
        {
            // Arrange
            var input = "=1+1";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Variable_Expression_When_Found()
        {
            // Arrange
            var input = "=$myvariable";
            _variable.Validate(Arg.Any<string>(), Arg.Any<object?>()).Returns(x => x.ArgAt<string>(0) == "myvariable" ? Result.Success(typeof(string)) : Result.Continue<Type>());

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Failure_From_Mathemetic_Expression_When_Found()
        {
            // Arrange
            var input = "=1+error";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: error");
        }

        [Fact]
        public void Returns_Success_Result_From_Formattable_String_When_Found_And_FormattableStringProcessor_Is_Not_Null()
        {
            // Arrange
            var input = "=@\"Hello {Name}!\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings(), _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Formattable_String_Unprocessed_When_Found_And_FormattableStringProcessor_Is_Null()
        {
            // Arrange
            var input = "=@\"Hello {Name}!\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Failure_Result_From_Formattable_String_When_Found()
        {
            // Arrange
            var input = "=@\"Hello {Kaboom}!\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings(), _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Validation failed, see inner results for details");
            result.InnerResults.Single().ErrorMessage.ShouldBe("Unsupported placeholder name: Kaboom");
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_When_Found()
        {
            // Arrange
            var input = "=\"Hello {Name}!\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_With_Pipe_Sign_When_Found()
        {
            // Arrange
            var input = "=\"Hello | {Name}!\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_With_Ampersand_When_Found()
        {
            // Arrange
            var input = "=\"Hello & {Name}!\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Literal_String_With_Equals_Operator_When_Found_More_Than_Two_Times()
        {
            // Arrange
            var input = "=\"a\" == \"b\" == \"c\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Function_When_Found()
        {
            // Arrange
            var input = "=MYFUNCTION()";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Function_With_Formattable_String_As_Argument()
        {
            // Arrange
            var input = "=TOUPPER(@\"Hello {Name}!\")";

            // Act
            var result = CreateSut().Validate(input, CreateSettings(), _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_With_Input_String_When_No_Mathematic_Expression_Or_Formattable_String_Or_Function_Was_Found()
        {
            // Arrange
            var input = "some string that does not start with = sign";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_With_Input_String_When_String_Starts_With_Equals_Sign_But_No_Other_Expression_Was_Found_After_This()
        {
            // Arrange
            var input = "=\"some string that starts with = sign but does not contain any formattable string, function or mathematical expression\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Piped_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello {Name}!\" | ToUpper(\"bla\")";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
        }

        [Fact]
        public void Returns_Success_Result_From_Concatenated_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello \" & \"Name!\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
        }

        [Fact]
        public void Returns_Success_Result_From_Concatenated_Expression_Containing_Formattable_String_When_Found()
        {
            // Arrange
            var input = "=\"Hello \" & @\"{Name}!\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings(), _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
        }

        [Fact]
        public void Returns_Success_Result_From_Concatenated_And_Piped_Expression_When_Found()
        {
            // Arrange
            var input = "=\"Hello \" & \"{Name}!\" | ToUpper(\"bla\")";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_String_Expressions()
        {
            // Arrange
            var input = "=\"Hello\" == \"Hello\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Non_String_Expressions()
        {
            // Arrange
            var input = "=1 == 2";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null == \"Hello\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=\"Hello\" == null";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Equals_Operator_When_Found_With_Left_And_Righ_Null()
        {
            // Arrange
            var input = "=null == null";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_NotEquals_Operator_When_Found_With_String_Expressions()
        {
            // Arrange
            var input = "=\"Hello\" != \"Hello\"";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null > 2";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 > null";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Greater_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 > 2";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_GreaterThanOrEqual_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null >= 2";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 >= null";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_GreaterOrEqual_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 >= 2";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null < 2";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 < null";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_Smaller_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 < 2";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_Left_Null()
        {
            // Arrange
            var input = "=null <= 2";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_Right_Null()
        {
            // Arrange
            var input = "=2 <= null";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_Result_From_SmallerOrEqual_Than_Operator_When_Found_With_No_Nulls_Same_Type()
        {
            // Arrange
            var input = "=3 <= 2";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Invalid_When_FunctionParser_Returns_Invalid()
        {
            // Arrange
            var input = "=somefunction(\uE002)";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Input cannot contain \uE002, as this is used internally for formatting");
        }

        [Fact]
        public void Returns_Failure_From_FunctionParser_When_FunctionName_Is_Missing()
        {
            // Arrange
            var input = "=()";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.ErrorMessage.ShouldBe("No function name found");
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
            var result = scope.ServiceProvider.GetRequiredService<IExpressionStringEvaluator>().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown function: MYFUNCTION");
        }

        [Fact]
        public void Returns_Success_Result_From_CastExpression_When_Found()
        {
            // Arrange
            var input = $"=cast 13 as {typeof(short).FullName}";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
            result.Value.ShouldBeNull();
        }

        [Fact]
        public void Returns_Success_Result_From_CastExpression_When_Found_With_Function()
        {
            // Arrange
            var input = $"=cast Expression() as {typeof(string).FullName}";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
            result.Value.ShouldBeNull();
        }

        [Fact]
        public void Returns_Invalid_Result_From_CastExpression_Unknown_Type()
        {
            // Arrange
            var input = "=cast 13 as unknowntype";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown type: unknowntype");
        }

        [Fact]
        public void Returns_Success_Result_From_TypeOfExpression_When_Found()
        {
            // Arrange
            var input = $"=typeof({typeof(short).FullName})";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
            result.Value.ShouldBeNull();
        }

        [Fact]
        public void Returns_Invalid_Result_From_TypeOfExpression_Unknown_Type()
        {
            // Arrange
            var input = "=typeof(unknowntype)";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown type: unknowntype");
        }

        [Fact]
        public void Can_Escape_Equals_Sign_To_Skip_ExpressionString_Parsing()
        {
            // Arrange
            var input = "\'=error()";

            // Act
            var result = CreateSut().Validate(input, CreateSettings());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }

    private IExpressionStringEvaluator CreateSut() => _scope.ServiceProvider.GetRequiredService<IExpressionStringEvaluator>();

    private sealed class MyPlaceholderProcessor : IPlaceholder
    {
        public Result<GenericFormattableString> Evaluate(string value, PlaceholderSettings settings, object? context, IFormattableStringParser formattableStringParser)
            => value == "Name"
                ? Result.Success(new GenericFormattableString(ReplacedValue))
                : Result.Error<GenericFormattableString>($"Unsupported placeholder name: {value}");

        public Result Validate(string value, PlaceholderSettings settings, object? context, IFormattableStringParser formattableStringParser)
            => value == "Name"
                ? Result.Success()
                : Result.Error($"Unsupported placeholder name: {value}");
    }

    private sealed class ErrorFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Error<object?>("Kaboom");
        }
    }

    [FunctionArgument("Expression", typeof(string))]
    private sealed class ToUpperFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Success<object?>(
                context.Context?.ToString()?.ToUpperInvariant()
                ?? context.FunctionCall.Arguments.FirstOrDefault()?.Evaluate(context).Value?.ToString()?.ToUpperInvariant()
                ?? string.Empty);
        }
    }

    [FunctionName("MyFunction")]
    private sealed class MyFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Success<object?>(ReplacedValue);
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
