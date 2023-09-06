namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class FunctionParseResultTests : IDisposable
{
    private readonly Mock<IFunctionParseResultEvaluator> _evaluatorMock = new();
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;

    public FunctionParseResultTests()
    {
        _evaluatorMock.Setup(x => x.Evaluate(It.IsAny<FunctionParseResult>(), It.IsAny<IExpressionParser>(), It.IsAny<object?>()))
                      .Returns<FunctionParseResult, IExpressionParser, object?>((result, _, _) => result.FunctionName switch
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
        _provider = new ServiceCollection().AddParsers().BuildServiceProvider(true);
        _scope = _provider.CreateScope();
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Invalid_When_Argument_Is_Not_Present()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithLiteralArgument();

        // Act
        var result = argument.GetArgumentValueResult(1, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Literal()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithLiteralArgument();

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("some value");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Literal_And_Ignores_DefaultValue()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithLiteralArgument();

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>(), "ignored");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("some value");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Function()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("MyNestedFunction");

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Evaluated result");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_With_DefaultValue_When_Argument_Is_Not_Present_But_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithoutArguments();

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>(), "some value");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("some value");
    }

    [Fact]
    public void GetArgumentStringValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithLiteralArgument();

        // Act
        var result = argument.GetArgumentStringValueResult(1, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentStringValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("NumericFunction");

        // Act
        var result = argument.GetArgumentStringValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type string");
    }

    [Fact]
    public void GetArgumentStringValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithLiteralArgument();

        // Act
        var result = argument.GetArgumentStringValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("some value");
    }

    [Fact]
    public void GetArgumentStringValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithoutArguments();

        // Act
        var result = argument.GetArgumentStringValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>(), "default value");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("default value");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithLiteralArgument();

        // Act
        var result = argument.GetArgumentInt32ValueResult(1, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Int32()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("NumericFunction");

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(1);
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("DateTimeFunction");

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type integer");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("UnknownExpressionString");

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type integer");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Int32()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("DateTimeFunctionAsString");

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type integer");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Int32()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("NumericFunctionAsString");

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13);
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithoutArguments();

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>(), 13);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13);
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithLiteralArgument();

        // Act
        var result = argument.GetArgumentInt64ValueResult(1, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Int64()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("LongFunction");

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(1L);
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("DateTimeFunction");

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type long integer");
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("UnknownExpressionString");

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type long integer");
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Int64()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("DateTimeFunctionAsString");

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type long integer");
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Int64()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("LongFunctionAsString");

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13L);
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithoutArguments();

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>(), 13L);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13L);
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithLiteralArgument();

        // Act
        var result = argument.GetArgumentDecimalValueResult(1, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Decimal()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("DecimalFunction");

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(1L);
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("DateTimeFunction");

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type decimal");
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("UnknownExpressionString");

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type decimal");
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Decimal()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("DateTimeFunctionAsString");

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type decimal");
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Decimal()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("DecimalFunctionAsString");

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13M);
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithoutArguments();

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>(), 13.5M);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13.5M);
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithLiteralArgument();

        // Act
        var result = argument.GetArgumentBooleanValueResult(1, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Boolean()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("BooleanFunction");

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(true);
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("DateTimeFunction");

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type boolean");
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("UnknownExpressionString");

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type boolean");
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Boolean()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("DateTimeFunctionAsString");

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type boolean");
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Boolean()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("BooleanFunctionAsString");

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(true);
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithoutArguments();

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>(), true);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeTrue();
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithLiteralArgument();

        // Act
        var result = argument.GetArgumentDateTimeValueResult(1, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_DateTime()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("DateTimeFunction");

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(DateTime.Today);
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("NumericFunction");

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type datetime");
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("UnknownExpressionString");

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type datetime");
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_DateTime()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("NumericFunctionAsString");

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type datetime");
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_DateTime()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithFunctionArgument("DateTimeFunctionAsString");

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(DateTime.Today);
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionParseResultWithoutArguments();
        var dt = DateTime.Now;

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", null, _evaluatorMock.Object, _scope.ServiceProvider.GetRequiredService<IExpressionParser>(), dt);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(dt);
    }

    public void Dispose()
    {
        _scope.Dispose();
        _provider.Dispose();
    }
    
    private static FunctionParseResult CreateFunctionParseResultWithoutArguments()
        => new FunctionParseResultBuilder()
            .WithFunctionName("Test")
            .Build();

    private static FunctionParseResult CreateFunctionParseResultWithLiteralArgument()
        => new FunctionParseResultBuilder()
            .WithFunctionName("Test")
            .AddArguments(new LiteralArgumentBuilder().WithValue("some value"))
            .Build();

    private static FunctionParseResult CreateFunctionParseResultWithFunctionArgument(string functionName)
        => new FunctionParseResultBuilder()
            .WithFunctionName("Test")
            .AddArguments(new FunctionArgumentBuilder().WithFunction(new FunctionParseResultBuilder().WithFunctionName(functionName)))
            .Build();
}
