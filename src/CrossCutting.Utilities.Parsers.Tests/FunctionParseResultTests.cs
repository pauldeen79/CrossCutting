namespace CrossCutting.Utilities.Parsers.Tests;

public class FunctionParseResultTests
{
    [Fact]
    public void GetArgument_Returns_Invalid_When_Argument_Is_Not_Present()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgument(1, "SomeName");

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgument_Returns_Success_When_Argument_Is_Present()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgument(0, "SomeName");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeSameAs(argument.Arguments[0]);
    }
}
