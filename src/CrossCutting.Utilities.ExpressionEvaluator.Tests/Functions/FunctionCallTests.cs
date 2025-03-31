namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Functions;

public sealed class FunctionCallTests : TestBase
{
    public FunctionCallTests()
    {
        var function = Substitute.For<IFunction>();
        function
            .Evaluate(Arg.Any<FunctionCallContext>())
            .Returns(x => x.ArgAt<FunctionCallContext>(0).FunctionCall.Name switch
            {
                "MyNestedFunction" => Result.Success<object?>("Evaluated result"),
                "NumericFunction" => Result.Success<object?>(1),
                "NumericFunctionAsString" => Result.Success<object?>("13"),
                "LongFunction" => Result.Success<object?>(1L),
                "LongFunctionAsString" => Result.Success<object?>("13L"),
                "DecimalFunction" => Result.Success<object?>(1M),
                "DecimalFunctionAsString" => Result.Success<object?>("13M"),
                "DateTimeFunctionAsString" => Result.Success<object?>(DateTime.Today.ToString(x.ArgAt<FunctionCallContext>(0).Context.Settings.FormatProvider)),
                "DateTimeFunction" => Result.Success<object?>(DateTime.Today),
                "BooleanFunction" => Result.Success<object?>(true),
                "BooleanFunctionAsString" => Result.Success<object?>("true"),
                "UnknownExpressionString" => Result.Success<object?>("%#$&"),
                _ => Result.NotSupported<object?>("Only Parsed result function is supported")
            });
        Expression
            .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
            .Returns(x => x.ArgAt<ExpressionEvaluatorContext>(0).Expression.EndsWith("()")
                ? function.Evaluate(new FunctionCallContext(new FunctionCallBuilder().WithName(x.ArgAt<ExpressionEvaluatorContext>(0).Expression.ReplaceSuffix("()", string.Empty, StringComparison.Ordinal)), x.ArgAt<ExpressionEvaluatorContext>(0)))
                : EvaluateExpression(x));
        Evaluator
            .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
            .Returns(x => Expression.Evaluate(x.ArgAt<ExpressionEvaluatorContext>(0)));
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Invalid_When_Argument_Is_Not_Present()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentValueResult(1, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Constant()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("some value");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Constant_And_Ignores_DefaultValue()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", context, (object)"ignored");

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("some value");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Function()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("MyNestedFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("Evaluated result");
    }

    [Fact]
    public void GetArgumentValueResult_Returns_Success_With_DefaultValue_When_Argument_Is_Not_Present_But_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentValueResult(0, "SomeName", context, (object)"some value");

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("some value");
    }

    [Fact]
    public void GetArgumentStringValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentStringValueResult(1, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentStringValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentStringValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type string");
    }

    [Fact]
    public void GetArgumentStringValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentStringValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("some value");
    }

    [Fact]
    public void GetArgumentStringValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentStringValueResult(0, "SomeName", context, "default value");

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("default value");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt32ValueResult(1, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Int32()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(1);
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type integer");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type integer");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Int32()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type integer");
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Int32()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13);
    }

    [Fact]
    public void GetArgumentInt32ValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt32ValueResult(0, "SomeName", context, 13);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13);
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt64ValueResult(1, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Int64()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("LongFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(1L);
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type long integer");
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type long integer");
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Int64()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type long integer");
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Int64()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("LongFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13L);
    }

    [Fact]
    public void GetArgumentInt64ValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentInt64ValueResult(0, "SomeName", context, 13L);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13L);
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDecimalValueResult(1, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Decimal()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DecimalFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(1L);
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type decimal");
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type decimal");
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Decimal()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type decimal");
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Decimal()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DecimalFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13M);
    }

    [Fact]
    public void GetArgumentDecimalValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDecimalValueResult(0, "SomeName", context, 13.5M);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13.5M);
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentBooleanValueResult(1, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Boolean()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("BooleanFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type boolean");
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type boolean");
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Boolean()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type boolean");
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Boolean()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("BooleanFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void GetArgumentBooleanValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentBooleanValueResult(0, "SomeName", context, true);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeTrue();
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(1, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_DateTime()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(DateTime.Today);
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type datetime");
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type datetime");
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_DateTime()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type datetime");
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_DateTime()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", context);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(DateTime.Today);
    }

    [Fact]
    public void GetArgumentDateTimeValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var dt = DateTime.Now;
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), expressionEvaluatorContext);

        // Act
        var result = argument.GetArgumentDateTimeValueResult(0, "SomeName", context, dt);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dt);
    }

    private static FunctionCall CreateFunctionCallWithoutArguments()
        => new FunctionCallBuilder()
            .WithName("Test")
            .Build();

    private static FunctionCall CreateFunctionCallWithConstantArgument()
        => new FunctionCallBuilder()
            .WithName("Test")
            .AddArguments("\"some value\"")
            .Build();

    private static FunctionCall CreateFunctionCallWithFunctionArgument(string functionName)
        => new FunctionCallBuilder()
            .WithName("Test")
            .AddArguments($"{functionName}()")
            .Build();
}
