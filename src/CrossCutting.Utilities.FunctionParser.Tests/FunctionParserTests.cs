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
    public void Missing_Function_Name_Returns_Invalid()
    {
        // Arrange
        var input = "()";

        // Act
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("No function name found");
    }

    [Fact]
    public void Missing_OpenBracket_Returns_Invalid()
    {
        // Arrange
        var input = "MYFUNCTION)";

        // Act
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not find open bracket");
    }

    [Fact]
    public void Missing_CloseBracket_Returns_Invalid()
    {
        // Arrange
        var input = "MYFUNCTION(";

        // Act
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not find close bracket");
    }

    [Fact]
    public void Empty_String_Returns_Invalid()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = FunctionParser.Parse(input);

        // Assert
        result.Status.Should().Be(Common.Results.ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Input cannot be null or empty");
    }
}
