﻿namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class FunctionParserTests : IDisposable
{
    private const string ReplacedValue = "replaced name";
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;

    public FunctionParserTests()
    {
        _provider = new ServiceCollection()
            .AddParsers()
            .AddSingleton<IPlaceholderProcessor, MyPlaceholderProcessor>()
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
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().HaveCount(3);
        result.Value.Arguments.Should().AllBeOfType<LiteralArgument>();
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("a", "b", "c");
    }

    [Fact]
    public void Can_Parse_Single_Function_With_Quoted_Arguments()
    {
        // Arrange
        var input = "MYFUNCTION(\"a,b\",c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().HaveCount(2);
        result.Value.Arguments.Should().AllBeOfType<LiteralArgument>();
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("a,b", "c");
    }

    [Fact]
    public void Can_Parse_Single_Function_With_Quoted_Arguments_That_Are_Surrounded_With_Spaces()
    {
        // Arrange
        var input = "MYFUNCTION(\"  a,b  \",c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().HaveCount(2);
        result.Value.Arguments.Should().AllBeOfType<LiteralArgument>();
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("  a,b  ", "c");
    }

    [Fact]
    public void Can_Parse_Single_Function_With_FormattableString_Argument()
    {
        // Arrange
        var input = "MYFUNCTION(@\"Hello, {Name}!\",b,c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture, _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().HaveCount(3);
        result.Value.Arguments.Should().AllBeOfType<LiteralArgument>();
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("Hello, replaced name!", "b", "c");
    }

    [Fact]
    public void Can_Parse_Single_Function_With_FormattableString_Argument_That_Are_Surrounded_With_Spaces()
    {
        // Arrange
        var input = "MYFUNCTION(@\"  Hello, {Name}!  \",b,c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture, _scope.ServiceProvider.GetRequiredService<IFormattableStringParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().HaveCount(3);
        result.Value.Arguments.Should().AllBeOfType<LiteralArgument>();
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("  Hello, replaced name!  ", "b", "c");
    }

    [Fact]
    public void Can_Parse_Single_Function_Without_Arguments()
    {
        // Arrange
        var input = "MYFUNCTION()";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().BeEmpty();
    }

    [Fact]
    public void Can_Parse_Single_Function_With_Only_Commas_As_Arguments()
    {
        // Arrange
        var input = "MYFUNCTION(,,)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().HaveCount(3);
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().AllBe(string.Empty);
    }

    [Fact]
    public void Can_Parse_Single_Function_With_Round_Brackets_In_Arguments()
    {
        // Arrange
        var input = "MYFUNCTION(\"FN1(a)\",\"FN2(b)\",\"FN3(c)\")";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().HaveCount(3);
        result.Value.Arguments.Should().AllBeOfType<LiteralArgument>();
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("FN1(a)", "FN2(b)", "FN3(c)");
    }

    [Fact]
    public void Can_Parse_Nested_Function()
    {
        // Arrange
        var input = "MYFUNCTION(a,b,MYNESTEDFUNCTION(c,d,e))";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value!.Arguments.Should().HaveCount(3);
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("a", "b");
        result.Value.Arguments.OfType<FunctionArgument>().Select(x => x.Function.FunctionName).Should().BeEquivalentTo("MYNESTEDFUNCTION");
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("c", "d", "e");
    }

    [Fact]
    public void Can_Parse_Double_Nested_Function()
    {
        // Arrange
        var input = "MYFUNCTION(a,b,MYNESTEDFUNCTION(SUB1(c),SUB1(d),SUB1(SUB2(e))))";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value!.Arguments.Should().HaveCount(3);
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("a", "b");
        result.Value.Arguments.OfType<FunctionArgument>().Select(x => x.Function.FunctionName).Should().BeEquivalentTo("MYNESTEDFUNCTION");
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).Should().HaveCount(3);
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).Should().AllBeOfType<FunctionArgument>();
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().Select(x => x.Function.FunctionName).Should().BeEquivalentTo("SUB1", "SUB1", "SUB1");
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).Select(x => x.GetType().Name).Should().BeEquivalentTo(nameof(LiteralArgument), nameof(LiteralArgument), nameof(FunctionArgument));
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().First().Function.FunctionName.Should().Be("SUB2");
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().First().Function.Arguments.Select(x => x.GetType().Name).Should().BeEquivalentTo(nameof(LiteralArgument));
        result.Value.Arguments.OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().SelectMany(x => x.Function.Arguments).OfType<FunctionArgument>().First().Function.Arguments.OfType<LiteralArgument>().First().Value.Should().Be("e");
    }

    [Fact]
    public void Missing_Function_Name_Returns_NotFound()
    {
        // Arrange
        var input = "()";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("No function name found");
    }

    [Fact]
    public void Not_Ending_With_Close_Bracket_Returns_NotFound()
    {
        // Arrange
        var input = "SOMEFUNCTION() with a suffix";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Input has additional characters after last close bracket");
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
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("No function name found");
    }

    [Fact]
    public void Missing_OpenBracket_Returns_NotFound()
    {
        // Arrange
        var input = "MYFUNCTION)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Missing open bracket");
    }

    [Fact]
    public void Missing_CloseBracket_Returns_NotFound()
    {
        // Arrange
        var input = "MYFUNCTION(";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Missing close bracket");
    }

    [Fact]
    public void Empty_String_Returns_NotFound()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Input cannot be null or empty");
    }

    [Fact]
    public void Non_Function_String_Returns_NotFound()
    {
        // Arrange
        var input = "some string that is not a function";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Missing close bracket");
    }

    [Fact]
    public void String_Containing_TemporaryDelimiter_Returns_NotSupported()
    {
        // Arrange
        var input = "This string contains the magic \uE002 internal temporary delimiter. Don't ask why, we just don't support it. You're doomed if you try this.";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.NotSupported);
        result.ErrorMessage.Should().Be("Input cannot contain \uE002, as this is used internally for formatting");
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
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom");
    }

    [Fact]
    public void Parse_Returns_Error_When_NameProcessor_Returns_Error()
    {
        // Arrange
        using var provider = new ServiceCollection().AddParsers().AddSingleton<IFunctionParserNameProcessor, ErrorNameProcessor>().BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<IFunctionParser>();
        var input = "MYFUNCTION(some argument)";

        // Act
        var result = sut.Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom");
    }

    [Fact]
    public void Parse_With_Spaces_In_FunctionName_Strips_Spaces()
    {
        // Arrange
        var input = " MYFUNCTION (a,b,c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().HaveCount(3);
        result.Value.Arguments.Should().AllBeOfType<LiteralArgument>();
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("a", "b", "c");
    }

    [Fact]
    public void Parse_With_Spaces_In_Arguments_Strips_Spaces()
    {
        // Arrange
        var input = "MYFUNCTION(a , b , c)";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().HaveCount(3);
        result.Value.Arguments.Should().AllBeOfType<LiteralArgument>();
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("a", "b", "c");
    }

    [Fact]
    public void Parse_With_Spaces_In_Quoted_Arguments_Does_Not_Strip_Spaces()
    {
        // Arrange
        var input = "MYFUNCTION(\"a \",\" b \",\" c\")";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().HaveCount(3);
        result.Value.Arguments.Should().AllBeOfType<LiteralArgument>();
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("a ", " b ", " c");
    }

    public void Dispose()
    {
        _scope.Dispose();
        _provider.Dispose();
    }

    private IFunctionParser CreateSut() => _scope.ServiceProvider.GetRequiredService<IFunctionParser>();

    private sealed class ErrorArgumentProcessor : IFunctionParserArgumentProcessor
    {
        public int Order => 1;

        public Result<FunctionParseResultArgument> Process(string stringArgument, IReadOnlyCollection<FunctionParseResult> results, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser)
            => Result.Error<FunctionParseResultArgument>("Kaboom");
    }

    private sealed class ErrorNameProcessor : IFunctionParserNameProcessor
    {
        public int Order => 1;

        public Result<string> Process(string input) => Result.Error<string>("Kaboom");
    }

    private sealed class MyPlaceholderProcessor : IPlaceholderProcessor
    {
        public int Order => 10;

        public Result<FormattableStringParserResult> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
            => value == "Name"
                ? Result.Success<FormattableStringParserResult>(ReplacedValue)
                : Result.Error<FormattableStringParserResult>($"Unsupported placeholder name: {value}");
    }
}
