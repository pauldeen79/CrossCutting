namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class FunctionParseResultTests : IDisposable
{
    private readonly Mock<IFunctionParseResultEvaluator> _evaluatorMock = new();
    private readonly ServiceProvider _provider;

    public FunctionParseResultTests()
    {
        _evaluatorMock.Setup(x => x.Evaluate(It.IsAny<FunctionParseResult>(), It.IsAny<object?>()))
                      .Returns<FunctionParseResult, object?>((result, _) => result.FunctionName switch
                        {
                            "MyNestedFunction" => Result<object?>.Success("Evaluated result"),
                            "NumericFunction" => Result<object?>.Success(1),
                            "NumericFunctionAsString" => Result<object?>.Success("13"),
                            "LongFunction" => Result<object?>.Success(1L),
                            "LongFunctionAsString" => Result<object?>.Success("13L"),
                            "DecimalFunction" => Result<object?>.Success(1M),
                            "DecimalFunctionAsString" => Result<object?>.Success("13M"),
                            "DateTimeFunctionAsString" => Result<object?>.Success(DateTime.Today.ToString(CultureInfo.InvariantCulture)),
                            "DateTimeFunction" => Result<object?>.Success(DateTime.Today),
                            "BooleanFunction" => Result<object?>.Success(true),
                            "BooleanFunctionAsString" => Result<object?>.Success("true"),
                            "UnknownExpressionString" => Result<object?>.Success("%#$&"),
                            _ => Result<object?>.NotSupported("Only Parsed result function is supported")
                        });
        _provider = new ServiceCollection().AddParsers().BuildServiceProvider();
    }

    [Fact]
    public void GetArgumentValue_Returns_Invalid_When_Argument_Is_Not_Present()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentValue(1, "SomeName", null, _evaluatorMock.Object);

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
        var result = argument.GetArgumentValue(0, "SomeName", null, _evaluatorMock.Object);

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
        var result = argument.GetArgumentValue(0, "SomeName", null, _evaluatorMock.Object);

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
        var result = argument.GetArgumentStringValue(1, "SomeName", null, _evaluatorMock.Object);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentStringValue_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("NumericFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, null)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentStringValue(0, "SomeName", null, _evaluatorMock.Object);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type string");
    }

    [Fact]
    public void GetArgumentStringValue_Returns_Success_When_ArgumentValue_Is_Of_Type_String()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentStringValue(0, "SomeName", null, _evaluatorMock.Object);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("some value");
    }

    [Fact]
    public void GetArgumentInt32Value_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentInt32Value(1, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentInt32Value_Returns_Success_When_ArgumentValue_Is_Of_Type_Int32()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("NumericFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentInt32Value(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(1);
    }

    [Fact]
    public void GetArgumentInt32Value_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("DateTimeFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentInt32Value(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type integer");
    }

    [Fact]
    public void GetArgumentInt32Value_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("UnknownExpressionString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentInt32Value(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type integer");
    }

    [Fact]
    public void GetArgumentInt32Value_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Int32()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("DateTimeFunctionAsString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentInt32Value(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type integer");
    }

    [Fact]
    public void GetArgumentInt32Value_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Int32()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("NumericFunctionAsString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentInt32Value(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13);
    }

    [Fact]
    public void GetArgumentInt64Value_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentInt64Value(1, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentInt64Value_Returns_Success_When_ArgumentValue_Is_Of_Type_Int64()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("LongFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentInt64Value(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(1L);
    }

    [Fact]
    public void GetArgumentInt64Value_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("DateTimeFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentInt64Value(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type long integer");
    }

    [Fact]
    public void GetArgumentInt64Value_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("UnknownExpressionString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentInt64Value(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type long integer");
    }

    [Fact]
    public void GetArgumentInt64Value_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Int64()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("DateTimeFunctionAsString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentInt64Value(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type long integer");
    }

    [Fact]
    public void GetArgumentInt64Value_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Int64()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("LongFunctionAsString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentInt64Value(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13L);
    }

    [Fact]
    public void GetArgumentDecimalValue_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentDecimalValue(1, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentDecimalValue_Returns_Success_When_ArgumentValue_Is_Of_Type_Decimal()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("DecimalFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentDecimalValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(1L);
    }

    [Fact]
    public void GetArgumentDecimalValue_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("DateTimeFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentDecimalValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type decimal");
    }

    [Fact]
    public void GetArgumentDecimalValue_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("UnknownExpressionString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentDecimalValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type decimal");
    }

    [Fact]
    public void GetArgumentDecimalValue_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Decimal()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("DateTimeFunctionAsString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentDecimalValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type decimal");
    }

    [Fact]
    public void GetArgumentDecimalValue_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Decimal()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("DecimalFunctionAsString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentDecimalValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13M);
    }

    [Fact]
    public void GetArgumentBooleanValue_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentBooleanValue(1, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentBooleanValue_Returns_Success_When_ArgumentValue_Is_Of_Type_Boolean()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("BooleanFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentBooleanValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(true);
    }

    [Fact]
    public void GetArgumentBooleanValue_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("DateTimeFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentBooleanValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type boolean");
    }

    [Fact]
    public void GetArgumentBooleanValue_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("UnknownExpressionString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentBooleanValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type boolean");
    }

    [Fact]
    public void GetArgumentBooleanValue_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Boolean()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("DateTimeFunctionAsString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentBooleanValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type boolean");
    }

    [Fact]
    public void GetArgumentBooleanValue_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Boolean()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("BooleanFunctionAsString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentBooleanValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(true);
    }

    [Fact]
    public void GetArgumentDateTimeValue_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new LiteralArgument("some value") }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentDateTimeValue(1, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentDateTimeValue_Returns_Success_When_ArgumentValue_Is_Of_Type_DateTime()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("DateTimeFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentDateTimeValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(DateTime.Today);
    }

    [Fact]
    public void GetArgumentDateTimeValue_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("NumericFunction", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentDateTimeValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type datetime");
    }

    [Fact]
    public void GetArgumentDateTimeValue_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("UnknownExpressionString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentDateTimeValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type datetime");
    }

    [Fact]
    public void GetArgumentDateTimeValue_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_DateTime()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("NumericFunctionAsString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentDateTimeValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type datetime");
    }

    [Fact]
    public void GetArgumentDateTimeValue_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_DateTime()
    {
        // Arrange
        var argument = new FunctionParseResult("Test", new[] { new FunctionArgument(new FunctionParseResult("DateTimeFunctionAsString", Enumerable.Empty<FunctionParseResultArgument>(), CultureInfo.InvariantCulture, default)) }, CultureInfo.InvariantCulture, default);

        // Act
        var result = argument.GetArgumentDateTimeValue(0, "SomeName", null, _evaluatorMock.Object, _provider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(DateTime.Today);
    }

    public void Dispose() => _provider.Dispose();
}
