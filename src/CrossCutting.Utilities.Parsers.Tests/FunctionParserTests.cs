namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class FunctionParserTests : IDisposable
{
    private const string ReplacedValue = "replaced name";
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;

    public FunctionParserTests()
    {
        _provider = new ServiceCollection()
            .AddParsers()
            .AddSingleton<IPlaceholder, MyPlaceholderProcessor>()
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
    }

    [Fact]
    public void Can_Parse_Single_Function_With_Arguments()
    {
        // Arrange
        var input = "MYFUNCTION(a,b,c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value.Arguments.Count.ShouldBe(3);
        result.Value.Arguments.ShouldAllBe(x => x is ExpressionArgument);
        result.Value.Arguments.OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ToArray().ShouldBeEquivalentTo(new[] { "a", "b", "c" });
    }

    [Fact]
    public void Can_Parse_Single_Function_With_Quoted_Arguments()
    {
        // Arrange
        var input = "MYFUNCTION(\"a,b\",c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value.Arguments.Count.ShouldBe(2);
        result.Value.Arguments.ShouldAllBe(x => x is ExpressionArgument);
        result.Value.Arguments.OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ToArray().ShouldBeEquivalentTo(new[] { "a,b", "c" });
    }

    [Fact]
    public void Can_Parse_Single_Function_With_Quoted_Arguments_That_Are_Surrounded_With_Spaces()
    {
        // Arrange
        var input = "MYFUNCTION(\"  a,b  \",c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value.Arguments.Count.ShouldBe(2);
        result.Value.Arguments.ShouldAllBe(x => x is ExpressionArgument);
        result.Value.Arguments.OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ToArray().ShouldBeEquivalentTo(new[] { "  a,b  ", "c" });
    }

    [Fact]
    public void Can_Parse_Single_Function_With_FormattableString_Argument()
    {
        // Arrange
        var input = "MYFUNCTION(@\"Hello, {Name}!\",b,c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture, _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value.Arguments.Count.ShouldBe(3);
        result.Value.Arguments.ShouldAllBe(x => x is ExpressionArgument);
        result.Value.Arguments.OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ToArray().ShouldBeEquivalentTo(new[] { "Hello, replaced name!", "b", "c" });
    }

    [Fact]
    public void Can_Parse_Single_Function_With_FormattableString_Argument_That_Are_Surrounded_With_Spaces()
    {
        // Arrange
        var input = "MYFUNCTION(@\"  Hello, {Name}!  \",b,c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture, _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value.Arguments.Count.ShouldBe(3);
        result.Value.Arguments.ShouldAllBe(x => x is ExpressionArgument);
        result.Value.Arguments.OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ToArray().ShouldBeEquivalentTo(new[] { "  Hello, replaced name!  ", "b", "c" });
    }

    [Fact]
    public void Can_Parse_Single_Function_Without_Arguments()
    {
        // Arrange
        var input = "MYFUNCTION()";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value.Arguments.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Parse_Single_Function_With_Only_Commas_As_Arguments()
    {
        // Arrange
        var input = "MYFUNCTION(,,)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value.Arguments.Count.ShouldBe(3);
        result.Value.Arguments.OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ShouldAllBe(x => x == string.Empty);
    }

    [Fact]
    public void Can_Parse_Single_Function_With_Round_Brackets_In_Arguments()
    {
        // Arrange
        var input = "MYFUNCTION(\"FN1(a)\",\"FN2(b)\",\"FN3(c)\")";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value.Arguments.Count.ShouldBe(3);
        result.Value.Arguments.ShouldAllBe(x => x is ExpressionArgument);
        result.Value.Arguments.OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ToArray().ShouldBeEquivalentTo(new[] { "FN1(a)", "FN2(b)", "FN3(c)" });
    }

    [Fact]
    public void Can_Parse_Nested_Function()
    {
        // Arrange
        var input = "MYFUNCTION(a,b,MYNESTEDFUNCTION(c,d,e))";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value!.Arguments.Count.ShouldBe(3);
        result.Value.Arguments.OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ToArray().ShouldBeEquivalentTo(new[] { "a", "b" });
        result.Value.Arguments.OfType<FunctionArgument>().Select(x => x.Function.Name).ToArray().ShouldBeEquivalentTo(new[] { "MYNESTEDFUNCTION" });
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ToArray().ShouldBeEquivalentTo(new[] { "c", "d", "e" });
    }

    [Fact]
    public void Can_Parse_Double_Nested_Function()
    {
        // Arrange
        var input = "MYFUNCTION(a,b,MYNESTEDFUNCTION(SUB1(c),SUB1(d),SUB1(SUB2(e))))";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value!.Arguments.Count.ShouldBe(3);
        result.Value.Arguments.OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ToArray().ShouldBeEquivalentTo(new[] { "a", "b" });
        result.Value.Arguments.OfType<FunctionArgument>().Select(x => x.Function.Name).ToArray().ShouldBeEquivalentTo(new[] { "MYNESTEDFUNCTION" });
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).Count().ShouldBe(3);
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).ShouldAllBe(x => x is FunctionArgument);
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().Select(x => x.Function.Name).ToArray().ShouldBeEquivalentTo(new[] { "SUB1", "SUB1", "SUB1" });
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).Select(x => x.GetType().Name).ToArray().ShouldBeEquivalentTo(new[] { nameof(ExpressionArgument), nameof(ExpressionArgument), nameof(FunctionArgument) });
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().First().Function.Name.ShouldBe("SUB2");
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().First().Function.Arguments.Select(x => x.GetType().Name).ToArray().ShouldBeEquivalentTo(new[] { nameof(ExpressionArgument) });
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().First().Function.Arguments.OfType<ExpressionArgument>().First().Value.ShouldBe("e");
    }

    [Fact]
    public void Missing_Function_Name_Returns_NotFound()
    {
        // Arrange
        var input = "()";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("No function name found");
    }

    [Fact]
    public void Not_Ending_With_Close_Bracket_Returns_NotFound()
    {
        // Arrange
        var input = "SOMEFUNCTION() with a suffix";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Input has additional characters after last close bracket");
    }

    [Fact]
    public void Error_In_FunctionName_Determination_Returns_NotFound()
    {
        // Arrange
        using var provider = new ServiceCollection().AddSingleton<IFunctionParser, FunctionParser>().BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<IFunctionParser>();
        var input = "MYFUNCTION(some argument)";

        // Act
        var result = sut.Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("No function name found");
    }

    [Fact]
    public void Missing_OpenBracket_Returns_NotFound()
    {
        // Arrange
        var input = "MYFUNCTION)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Missing open bracket");
    }

    [Fact]
    public void Missing_CloseBracket_Returns_NotFound()
    {
        // Arrange
        var input = "MYFUNCTION(";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Missing close bracket");
    }

    [Fact]
    public void Empty_String_Returns_NotFound()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("No function found");
    }

    [Fact]
    public void Non_Function_String_Returns_NotFound()
    {
        // Arrange
        var input = "some string that is not a function";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.ErrorMessage.ShouldBe("Missing close bracket");
    }

    [Fact]
    public void String_Containing_TemporaryDelimiter_Returns_NotSupported()
    {
        // Arrange
        var input = "This string contains the magic \uE002 internal temporary delimiter. Don't ask why, we just don't support it. You're doomed if you try this.";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotSupported);
        result.ErrorMessage.ShouldBe("Input cannot contain \uE002, as this is used internally for formatting");
    }

    [Fact]
    public void Parse_Returns_Error_When_ArgumentProcessor_Returns_Error()
    {
        // Arrange
        using var provider = new ServiceCollection().AddParsers().AddSingleton<IFunctionParserArgumentProcessor, ErrorArgumentProcessor>().BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<IFunctionParser>();
        var input = "MYFUNCTION(some argument)";

        // Act
        var result = sut.Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Kaboom");
    }

    [Fact]
    public void Parse_Returns_Error_When_NameProcessor_Returns_Error()
    {
        // Arrange
        using var provider = new ServiceCollection()
            .AddSingleton<IFunctionParserNameProcessor, ErrorNameProcessor>() // important to add this before the default parser name processor because of the order...
            .AddParsers()
            .BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<IFunctionParser>();
        var input = "MYFUNCTION(some argument)";

        // Act
        var result = sut.Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Kaboom");
    }

    [Fact]
    public void Parse_With_Spaces_In_FunctionName_Strips_Spaces()
    {
        // Arrange
        var input = " MYFUNCTION (a,b,c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value.Arguments.Count.ShouldBe(3);
        result.Value.Arguments.ShouldAllBe(x => x is ExpressionArgument);
        result.Value.Arguments.OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ToArray().ShouldBeEquivalentTo(new[] { "a", "b", "c" });
    }

    [Fact]
    public void Parse_With_Spaces_In_Arguments_Strips_Spaces()
    {
        // Arrange
        var input = "MYFUNCTION(a , b , c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value.Arguments.Count.ShouldBe(3);
        result.Value.Arguments.ShouldAllBe(x => x is ExpressionArgument);
        result.Value.Arguments.OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ToArray().ShouldBeEquivalentTo(new[] { "a", "b", "c" });
    }

    [Fact]
    public void Parse_With_Spaces_In_Quoted_Arguments_Does_Not_Strip_Spaces()
    {
        // Arrange
        var input = "MYFUNCTION(\"a \",\" b \",\" c\")";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value!.Name.ShouldBe("MYFUNCTION");
        result.Value.Arguments.Count.ShouldBe(3);
        result.Value.Arguments.ShouldAllBe(x => x is ExpressionArgument);
        result.Value.Arguments.OfType<ExpressionArgument>().Select(x => x.Value.ToStringWithDefault()).ToArray().ShouldBeEquivalentTo(new[] { "a ", " b ", " c" });
    }

    public void Dispose()
    {
        _scope.Dispose();
        _provider.Dispose();
    }

    private IFunctionParser CreateSut() => _scope.ServiceProvider.GetRequiredService<IFunctionParser>();

    private sealed class ErrorArgumentProcessor : IFunctionParserArgumentProcessor
    {
        public Result<IFunctionCallArgument> Process(string argument, IReadOnlyCollection<FunctionCall> functionCalls, FunctionParserSettings settings, object? context)
            => Result.Error<IFunctionCallArgument>("Kaboom");
    }

    private sealed class ErrorNameProcessor : IFunctionParserNameProcessor
    {
        public Result<string> Process(string input) => Result.Error<string>("Kaboom");
    }

    private sealed class MyPlaceholderProcessor : IPlaceholder
    {
        public Result<GenericFormattableString> Evaluate(string value, PlaceholderSettings settings, object? context, IFormattableStringParser formattableStringParser)
            => value == "Name"
                ? Result.Success<GenericFormattableString>(ReplacedValue)
                : Result.Error<GenericFormattableString>($"Unsupported placeholder name: {value}");

        public Result Validate(string value, PlaceholderSettings settings, object? context, IFormattableStringParser formattableStringParser)
            => value == "Name"
                ? Result.Success()
                : Result.Error($"Unsupported placeholder name: {value}");
    }
}
