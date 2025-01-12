namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class FunctionCallTests : IDisposable
{
    private readonly IFunctionEvaluator _functionEvaluatorMock = Substitute.For<IFunctionEvaluator>();
    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;

    public FunctionCallTests()
    {
        _functionEvaluatorMock
            //<FunctionParseResult, IExpressionParser, object?>((result, _, _)
            .Evaluate(Arg.Any<FunctionCall>(), Arg.Any<IExpressionEvaluator>(), Arg.Any<object?>())
            .Returns(x => x.ArgAt<FunctionCall>(0).Name switch
            {
                "MyNestedFunction" => Result.Success<object?>("Evaluated result"),
                "NumericFunction" => Result.Success<object?>(1),
                "NumericFunctionAsString" => Result.Success<object?>("13"),
                "LongFunction" => Result.Success<object?>(1L),
                "LongFunctionAsString" => Result.Success<object?>("13L"),
                "DecimalFunction" => Result.Success<object?>(1M),
                "DecimalFunctionAsString" => Result.Success<object?>("13M"),
                "DateTimeFunctionAsString" => Result.Success<object?>(DateTime.Today.ToString(CultureInfo.InvariantCulture)),
                "DateTimeFunction" => Result.Success<object?>(DateTime.Today),
                "BooleanFunction" => Result.Success<object?>(true),
                "BooleanFunctionAsString" => Result.Success<object?>("true"),
                "UnknownExpressionString" => Result.Success<object?>("%#$&"),
                _ => Result.NotSupported<object?>("Only Parsed result function is supported")
            });
        _provider = new ServiceCollection().AddParsers().BuildServiceProvider(true);
        _scope = _provider.CreateScope();
        _expressionEvaluator = _scope.ServiceProvider.GetRequiredService<IExpressionEvaluator>(); // using the real expression evaluator here
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Invalid_When_Argument_Is_Not_Present()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentValueResult(1, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Constant()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("some value");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Delegate()
    {
        // Arrange
        var argument = CreateFunctionCallWithDelegateArgument();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("some value");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Constant_And_Ignores_DefaultValue()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", request, (object)"ignored");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("some value");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Function()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("MyNestedFunction");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("Evaluated result");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_With_DefaultValue_When_Argument_Is_Not_Present_But_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", request, (object)"some value");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("some value");
    }

    [Fact]
    public void GetArgumentStringValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentStringValueResult(1, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentStringValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunction");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentStringValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type string");
    }

    [Fact]
    public void GetArgumentStringValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentStringValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("some value");
    }

    [Fact]
    public void GetArgumentStringValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentStringValueResult(0, "SomeName", request, "default value");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("default value");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt32ValueResult(1, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Int32()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunction");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(1);
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type integer");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type integer");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Int32()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type integer");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Int32()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunctionAsString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13);
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", request, 13);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13);
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt64ValueResult(1, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Int64()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("LongFunction");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(1L);
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type long integer");
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type long integer");
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Int64()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type long integer");
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Int64()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("LongFunctionAsString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13L);
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", request, 13L);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13L);
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDecimalValueResult(1, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Decimal()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DecimalFunction");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(1L);
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type decimal");
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type decimal");
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Decimal()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type decimal");
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Decimal()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DecimalFunctionAsString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13M);
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", request, 13.5M);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(13.5M);
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentBooleanValueResult(1, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Boolean()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("BooleanFunction");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(true);
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type boolean");
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type boolean");
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Boolean()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type boolean");
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Boolean()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("BooleanFunctionAsString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(true);
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", request, true);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeTrue();
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(1, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_DateTime()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(DateTime.Today);
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunction");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type datetime");
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type datetime");
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_DateTime()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunctionAsString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("SomeName is not of type datetime");
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_DateTime()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", request);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(DateTime.Today);
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var dt = DateTime.Now;
        var request = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), _functionEvaluatorMock, _expressionEvaluator, CultureInfo.InvariantCulture, null);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", request, dt);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(dt);
    }

    public void Dispose()
    {
        _scope.Dispose();
        _provider.Dispose();
    }

    private static FunctionCall CreateFunctionCallWithoutArguments()
        => new FunctionCallBuilder()
            .WithName("Test")
            .Build();

    private static FunctionCall CreateFunctionCallWithConstantArgument()
        => new FunctionCallBuilder()
            .WithName("Test")
            .AddArguments(new ConstantArgumentBuilder().WithValue("some value"))
            .Build();

    private static FunctionCall CreateFunctionCallWithDelegateArgument()
        => new FunctionCallBuilder()
            .WithName("Test")
            .AddArguments(new DelegateArgumentBuilder().WithDelegate(() => "some value"))
            .Build();

    private static FunctionCall CreateFunctionCallWithFunctionArgument(string functionName)
        => new FunctionCallBuilder()
            .WithName("Test")
            .AddArguments(new FunctionArgumentBuilder().WithFunction(new FunctionCallBuilder().WithName(functionName)))
            .Build();
}
