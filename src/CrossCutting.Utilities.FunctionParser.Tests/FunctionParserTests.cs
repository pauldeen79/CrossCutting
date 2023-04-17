namespace CrossCutting.Utilities.FunctionParser.Tests;

public class FunctionParserTests
{
    [Fact]
    public void Can_Parse_Single_Function_With_Arguments()
    {
        // Arrange
        var input = "MYFUNCTION(a,b,c)";

        // Act
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.Ok);
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
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().HaveCount(2);
        result.Value.Arguments.Should().AllBeOfType<LiteralArgument>();
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().BeEquivalentTo("a,b", "c");
    }

    [Fact]
    public void Can_Parse_Single_Function_Without_Arguments()
    {
        // Arrange
        var input = "MYFUNCTION()";

        // Act
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().BeEmpty();
    }

    [Fact]
    public void Can_Parse_Single_Function_With_Only_Commas_As_Arguments()
    {
        // Arrange
        var input = "MYFUNCTION(,,)";

        // Act
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.Ok);
        result.Value!.FunctionName.Should().Be("MYFUNCTION");
        result.Value.Arguments.Should().HaveCount(3);
        result.Value.Arguments.OfType<LiteralArgument>().Select(x => x.Value).Should().AllBe(string.Empty);
    }

    [Fact]
    public void Can_Parse_Nested_Function()
    {
        // Arrange
        var input = "MYFUNCTION(a,b,MYNESTEDFUNCTION(c,d,e))";

        // Act
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.Ok);
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
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.Ok);
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
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("No function name found");
    }

    [Fact]
    public void Missing_OpenBracket_Returns_NotFound()
    {
        // Arrange
        var input = "MYFUNCTION)";

        // Act
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Missing open bracket");
    }

    [Fact]
    public void Missing_CloseBracket_Returns_NotFound()
    {
        // Arrange
        var input = "MYFUNCTION(";

        // Act
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Missing close bracket");
    }

    [Fact]
    public void Empty_String_Returns_NotFound()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.NotFound);
        result.ErrorMessage.Should().Be("Input cannot be null or empty");
    }

    [Fact]
    public void String_Containing_TemporaryDelimiter_Returns_NotSupported()
    {
        // Arrange
        var input = "This string contains the magic ^^ internal temporary delimiter. Don't ask why, we just don't support it. You're doomed if you try this.";

        // Act
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.NotSupported);
        result.ErrorMessage.Should().Be("Input cannot contain ^^, as this is used internally for formatting");
    }
}
