namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class FormattableStringParserTests : IDisposable
{
    private ServiceProvider? _provider;
    private IServiceScope? _scope;
    private IVariable _variable = default!;
    private const string ReplacedValue = "replaced name";

    [Fact]
    public void Parse_Returns_Success_On_Null_Input()
    {
        // Arrange
        var input = default(string);
        var sut = CreateSut();

        // Act
        var result = sut.Parse(input!, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Parse_Returns_Success_On_Empty_Input()
    {
        // Arrange
        var input = string.Empty;
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(input!, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Parse_Throws_On_Null_Settings()
    {
        // Arrange
        var sut = CreateSut();

        // Act & Assert
        sut.Invoking(x => x.Parse("some input", settings: null!))
           .Should().Throw<ArgumentNullException>().WithParameterName("settings");
    }

    [Fact]
    public void Parse_Returns_Success_When_Using_Nested_Open_Signs()
    {
        // Arrange
        var input = "Hello {Name {nested}} you are welcome";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.GetValueOrThrow().Format.Should().Be("Hello {{Name {{nested}} you are welcome");
    }

    [Fact]
    public void Parse_Returns_Invalid_When_Open_Sign_Is_Missing()
    {
        // Arrange
        var input = "}";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Parse_Returns_Invalid_When_Variable_Is_Unknown()
    {
        // Arrange
        var input = "{$unknownVariable}";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Unknown variable found: unknownVariable");
    }

    [Fact]
    public void Parse_Returns_Success_When_Close_Sign_Is_Missing()
    {
        // Arrange
        var input = "{";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.GetValueOrThrow().Format.Should().Be(input + input); //need to duplicate because of FormatException on FormattableString
    }

    [Fact]
    public void Parse_Returns_Result_From_ProcessDelegate_When_Not_Successful()
    {
        // Arrange
        var input = "{Unsupported placeholder}";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(input, settings);

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
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Parse(input, CultureInfo.InvariantCulture, "[value from context]");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be(expectedValue);
    }
    [Fact]
    public void Validate_Returns_Success_On_Null_Input()
    {
        // Arrange
        var input = default(string);
        var sut = CreateSut();

        // Act
        var result = sut.Validate(input!, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Validate_Returns_Success_On_Empty_Input()
    {
        // Arrange
        var input = string.Empty;
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(input!, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Validate_Throws_On_Null_Settings()
    {
        // Arrange
        var sut = CreateSut();

        // Act & Assert
        sut.Invoking(x => x.Validate("some input", settings: null!))
           .Should().Throw<ArgumentNullException>().WithParameterName("settings");
    }

    [Fact]
    public void Validate_Returns_Success_When_Using_Nested_Open_Signs()
    {
        // Arrange
        var input = "Hello {Name {nested}} you are welcome";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Validate_Returns_Invalid_When_Open_Sign_Is_Missing()
    {
        // Arrange
        var input = "}";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Validate_Returns_Invalid_When_Variable_Is_Unknown()
    {
        // Arrange
        var input = "{$unknownVariable}";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Validation failed, see inner results for details");
        result.InnerResults.Single().ErrorMessage.Should().Be("Unknown variable found: unknownVariable");
    }

    [Fact]
    public void Validate_Returns_Success_When_Close_Sign_Is_Missing()
    {
        // Arrange
        var input = "{";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Validate_Returns_Invalid_When_Not_Successful()
    {
        // Arrange
        var input = "{Unsupported placeholder}";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Validation failed, see inner results for details");
        result.InnerResults.Single().ErrorMessage.Should().Be("Unsupported placeholder name: Unsupported placeholder");
    }

    [Theory,
        InlineData("Hello {Name}!"),
        InlineData("Hello {Context}!"),
        InlineData("Hello {{Name}}!"),
        InlineData("Data without accolades"),
        InlineData("public class Bla {{ /* implementation goes here */ }}"),
        InlineData("public class Bla {{ {Name} }}")]
    public void Validate_Returns_Success_On_Valid_Input(string input)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Validate(input, CultureInfo.InvariantCulture, "[value from context]");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
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
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Parse_Works_With_Csharp_Code_Using_One_Character_Placeholder_Markers()
    {
        // Arrange
        var input = "public class Bla {{ /* implementation goes here with {Name} */ }}";
        var settings = new FormattableStringParserSettingsBuilder()
            .WithFormatProvider(CultureInfo.InvariantCulture)
            .WithPlaceholderStart("{")
            .WithPlaceholderEnd("}")
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(input, settings, "[value from context]");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be("public class Bla { /* implementation goes here with replaced name */ }");
    }

    [Fact]
    public void Parse_Works_With_Csharp_Code_Using_Two_Character_Placeholder_Markers()
    {
        // Arrange
        var input = "public class Bla { /* implementation goes here with {{Name}} */ }";
        var settings = new FormattableStringParserSettingsBuilder()
            .WithFormatProvider(CultureInfo.InvariantCulture)
            .WithPlaceholderStart("{{")
            .WithPlaceholderEnd("}}")
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(input, settings, "[value from context]");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be("public class Bla { /* implementation goes here with replaced name */ }");
    }

    [Fact]
    public void Parse_Returns_Error_When_Maximum_Recursion_Level_Is_Reached()
    {
        // Arrange
        const string Input = "Hello {ReplaceWithPlaceholder}!";
        var settings = new FormattableStringParserSettingsBuilder().WithMaximumRecursion(0).Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Maximum of 0 recursions is reached");
    }

    [Fact]
    public void Can_Disable_Escaping_Single_Braces()
    {
        // Arrange
        var input = "public class Bla {{ /* implementation goes here with <Name> */ }}";
        var settings = new FormattableStringParserSettingsBuilder()
            .WithFormatProvider(CultureInfo.InvariantCulture)
            .WithPlaceholderStart("<")
            .WithPlaceholderEnd(">")
            .WithEscapeBraces(false)
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(input, settings, "[value from context]");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be("public class Bla { /* implementation goes here with replaced name */ }");
    }

    [Fact]
    public void Parse_Works_With_ExpressionString_Containing_Function()
    {
        // Arrange
        const string Input = "Hello {MyFunction()}!";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be("Hello function result!");
    }

    [Fact]
    public void Parse_Works_With_ExpressionString_Containing_Nested_Function()
    {
        // Arrange
        const string Input = "Hello {ToUpperCase(MyFunction())}!";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be("Hello FUNCTION RESULT!");
    }

    [Fact]
    public void Validate_Works_With_ExpressionString_Containing_Nested_Function()
    {
        // Arrange
        const string Input = "Hello {ToUpperCase(MyFunction())}!";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Parse_Works_With_ExpressionString_Using_EmptyArgument()
    {
        // Arrange
        const string Input = "Hello {ToUpperCase(null)}!";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be("Hello !");
    }

    [Fact]
    public void Validate_Works_With_ExpressionString_Using_EmptyArgument()
    {
        // Arrange
        const string Input = "Hello {ToUpperCase(null)}!";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Parse_Works_With_ExpressionString()
    {
        // Arrange
        const string Input = "I can add 1 to 2, this results in {1 + 1}";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be("I can add 1 to 2, this results in 2");
    }

    [Fact]
    public void Parse_Works_With_Variable_ExpressionString()
    {
        // Arrange
        const string Input = "I can add 1 to 2, this results in {$variable + $variable}";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be("I can add 1 to 2, this results in 2");
    }

    [Fact]
    public void Parse_Works_With_Placeholder_That_Returns_Another_Placeholder()
    {
        // Arrange
        const string Input = "Hello {ReplaceWithPlaceholder}!";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be($"Hello {ReplacedValue}!");
    }

    [Fact]
    public void Parse_Returns_Invalid_On_Unknown_Placeholder()
    {
        // Arrange
        const string Input = "{Unknown placeholder}";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Parse(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Unknown placeholder in value: Unknown placeholder");
    }

    [Fact]
    public void Validate_Works_With_Placeholder_That_Returns_Another_Placeholder()
    {
        // Arrange
        const string Input = "Hello {ReplaceWithPlaceholder}!";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Validate_Returns_Invalid_On_Unknown_Placeholder()
    {
        // Arrange
        const string Input = "{Unknown placeholder}";
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(Input, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Validation failed, see inner results for details");
        result.InnerResults.Single().ErrorMessage.Should().Be("Unknown placeholder in value: Unknown placeholder");
    }

    [Fact]
    public void Can_Parse_String_And_Defer_Specific_Placeholder()
    {
        // Arrange
        var sut = CreateSut();
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var preparsedResult = sut.Parse("Hello {Name}, you are called {{Name}}", settings).GetValueOrThrow();

        // Act
        var result = sut.Parse(preparsedResult, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be($"Hello {ReplacedValue}, you are called {ReplacedValue}");
        result.Value.ArgumentCount.Should().Be(1);
        result.Value.GetArgument(0).Should().BeEquivalentTo(ReplacedValue);
    }

    [Fact]
    public void Can_Parse_String_And_Defer_Specific_Placeholder_With_Two_Character_Placholder_Markers()
    {
        // Arrange
        var sut = CreateSut();
        var settings = new FormattableStringParserSettingsBuilder()
            .WithPlaceholderStart("{{")
            .WithPlaceholderEnd("}}")
            .Build();
        var preparsedResult = sut.Parse("Hello {{Name}}, you are called {{{{Name}}}}", settings).GetValueOrThrow();

        // Act
        var result = sut.Parse(preparsedResult, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be($"Hello {ReplacedValue}, you are called {ReplacedValue}");
        result.Value.ArgumentCount.Should().Be(1);
        result.Value.GetArgument(0).Should().BeEquivalentTo(ReplacedValue);
    }

    [Fact]
    public void Can_Parse_String_And_Defer_Specific_Placeholder_With_Three_Character_Placholder_Markers()
    {
        // Arrange
        var sut = CreateSut();
        var settings = new FormattableStringParserSettingsBuilder()
            .WithPlaceholderStart("{{{")
            .WithPlaceholderEnd("}}}")
            .Build();
        var preparsedResult = sut.Parse("Hello {{{Name}}}, you are called {{{{{{Name}}}}}}", settings).GetValueOrThrow();

        // Act
        var result = sut.Parse(preparsedResult, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be($"Hello {ReplacedValue}, you are called {ReplacedValue}");
        result.Value.ArgumentCount.Should().Be(1);
        result.Value.GetArgument(0).Should().BeEquivalentTo(ReplacedValue);
    }

    [Fact]
    public void Can_Parse_String_And_Defer_Specific_Placeholder_With_Custom_Two_Character_Placholder_Markers()
    {
        // Arrange
        var sut = CreateSut();
        var settings = new FormattableStringParserSettingsBuilder()
            .WithPlaceholderStart("<%")
            .WithPlaceholderEnd("%>")
            .Build();
        var preparsedResult = sut.Parse("Hello <%Name%>, you are called <%<%Name%>%>", settings).GetValueOrThrow();

        // Act
        var result = sut.Parse(preparsedResult, settings);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be($"Hello {ReplacedValue}, you are called {ReplacedValue}");
        result.Value.ArgumentCount.Should().Be(1);
        result.Value.GetArgument(0).Should().BeEquivalentTo(ReplacedValue);
    }

    [Fact]
    public void Can_Parse_String_With_Double_Placeholder_Signs()
    {
        // Arrange
        var input = "Hello {{Name}}!";
        var sut = CreateSut();
        var settings = new FormattableStringParserSettingsBuilder()
            .WithFormatProvider(CultureInfo.InvariantCulture)
            .WithPlaceholderStart("{{")
            .WithPlaceholderEnd("}}")
            .Build();

        // Act
        var result = sut.Parse(input, settings, "[value from context]");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be($"Hello {ReplacedValue}!");
    }

    [Theory]
    [InlineData("[", "]")]
    [InlineData("<", ">")]
    [InlineData("[[", "]]")]
    [InlineData("<<", ">>")]
    [InlineData("@@", "$$")]
    [InlineData("<%", "%>")]
    [InlineData("{{", "}}")]
    [InlineData("^^", "&&")]
    public void Can_Parse_String_With_Custom_Placeholders(string start, string end)
    {
        // Arrange
        var input = $"Hello {start}Name{end}!";
        var sut = CreateSut();
        var settings = new FormattableStringParserSettingsBuilder()
            .WithFormatProvider(CultureInfo.InvariantCulture)
            .WithPlaceholderStart(start)
            .WithPlaceholderEnd(end)
            .Build();

        // Act
        var result = sut.Parse(input, settings, "[value from context]");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.ToString().Should().Be($"Hello {ReplacedValue}!");
    }

    [Fact]
    public void Can_Implicitly_Convert_ParseStringResult_To_String()
    {
        // Arrange
        var settings = new FormattableStringParserSettingsBuilder().Build();
        var sut = CreateSut();
        var parsedResult = sut.Parse("Hello {Name}!", settings);

        // Act
        string result = parsedResult.GetValueOrThrow();

        // Assert
        result.Should().Be("Hello replaced name!");
    }

    [Fact]
    public void Can_Implicitly_Convert_Null_ParseStringResult_To_String()
    {
        // Arrange
        var parsedResult = default(GenericFormattableString);

        // Act
        string result = parsedResult!;

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void FromString_Creates_New_Instance_From_String_Correclty()
    {
        // Act
        var instance = GenericFormattableString.FromString("hello world");

        // Assert
        instance.Format.Should().Be("{0}");
        instance.ToString().Should().Be("hello world");
    }

    [Fact]
    public void FromString_Creates_New_Instance_From_String_With_Braces_Correclty()
    {
        // Act
        var instance = GenericFormattableString.FromString("hello {world}");

        // Assert
        instance.Format.Should().Be("{0}");
        instance.ToString().Should().Be("hello {world}");
    }

    [Fact]
    public void ToString_Override_Returns_Correct_Result()
    {
        // Arrange
        GenericFormattableString result = "Hello world!";

        // Act
        var stringResult = result.ToString();

        // Assert
        stringResult.Should().Be("Hello world!");
    }

    [Fact]
    public void Implicit_Operator_Returns_Correct_Result()
    {
        // Arrange
        GenericFormattableString result = "Hello world!";

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
        _variable = Substitute.For<IVariable>();

        _variable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(x => x.ArgAt<string>(0) == "variable"
            ? Result.Success<object?>(1)
            : Result.Continue<object?>());

        _variable.Validate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.NoContent<Type>());

        _provider = new ServiceCollection()
            .AddParsers()
            .AddSingleton<IPlaceholder, MyPlaceholderProcessor>()
            .AddSingleton<IFunction, MyFunction>()
            .AddSingleton<IFunction, ToUppercaseFunction>()
            .AddSingleton(_variable)
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
        return _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>();
    }

    private sealed class MyPlaceholderProcessor : IPlaceholder
    {
        public Result<GenericFormattableString> Evaluate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
        {
            return value switch
            {
                "Name" => Result.Success<GenericFormattableString>(ReplacedValue),
                "Context" => Result.Success<GenericFormattableString>(context.ToStringWithDefault()),
                "Unsupported placeholder" => Result.Error<GenericFormattableString>($"Unsupported placeholder name: {value}"),
                "ReplaceWithPlaceholder" => Result.Success<GenericFormattableString>("{Name}"),
                _ => Result.Continue<GenericFormattableString>()
            };
        }

        public Result Validate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
        {
            return value switch
            {
                "Name" => Result.Success(),
                "Context" => Result.Success(),
                "Unsupported placeholder" => Result.Error($"Unsupported placeholder name: {value}"),
                "ReplaceWithPlaceholder" => Result.Success(),
                _ => Result.Continue()
            };
        }
    }

    [FunctionName("MyFunction")]
    private sealed class MyFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Success<object?>("function result");
        }
    }

    [FunctionArgument("expression", typeof(string), false)]
    private sealed class ToUppercaseFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            var valueResult = context.FunctionCall.Arguments.First().Evaluate(context);
            if (!valueResult.IsSuccessful())
            {
                return valueResult;
            }

            return Result.Success<object?>(valueResult.Value.ToStringWithDefault().ToUpperInvariant());
        }
    }
}
