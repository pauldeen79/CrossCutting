namespace CrossCutting.Utilities.Parsers.Tests;

public class FunctionParseResultTests
{
    private readonly Mock<IFunctionParseResultEvaluator> _evaluatorMock = new();

    public FunctionParseResultTests()
    {
        _evaluatorMock.Setup(x => x.Evaluate(It.IsAny<FunctionParseResult>()))
                      .Returns<FunctionParseResult>(result => result.FunctionName switch
                        {
                            "MyNestedFunction" => Result<object?>.Success("Evaluated result"),
                            "NumericFunction" => Result<object?>.Success(1),
                            _ => Result<object?>.NotSupported("Only Parsed result function is supported")
                        });
    }

    [Fact]
    public void GetArgumentValue_Returns_Invalid_When_Argument_Is_Not_Present()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentValue(1, "SomeName", _evaluatorMock.Object);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentValue_Returns_Success_When_Argument_Is_Present_And_Literal()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentValue(0, "SomeName", _evaluatorMock.Object);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("some value");
    }

    [Fact]
    public void GetArgumentValue_Returns_Success_When_Argument_Is_Present_And_Function()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("MyNestedFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentValue(0, "SomeName", _evaluatorMock.Object);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Evaluated result");
    }

    [Fact]
    public void GetArgumentStringValue_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentStringValue(1, "SomeName", _evaluatorMock.Object);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentStringValue_Returns_Invalid_When_ArgumentValue_Not_Of_Type_String()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("NumericFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, null)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentStringValue(0, "SomeName", _evaluatorMock.Object);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type string");
    }

    [Fact]
    public void GetArgumentStringValue_Returns_Success_When_ArgumentValue_Of_Type_String()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentStringValue(0, "SomeName", _evaluatorMock.Object);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("some value");
    }
}
