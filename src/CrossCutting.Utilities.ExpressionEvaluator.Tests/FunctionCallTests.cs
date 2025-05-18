namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public sealed class FunctionCallTests : TestBase
{
    public FunctionCallTests()
    {
        var function = Substitute.For<IFunction>();
        function
            .EvaluateAsync(Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
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
            .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
            .Returns(x => x.ArgAt<ExpressionEvaluatorContext>(0).Expression.EndsWith("()")
                ? function.EvaluateAsync(new FunctionCallContext(new FunctionCallBuilder()
                    .WithName(x.ArgAt<ExpressionEvaluatorContext>(0).Expression.ReplaceSuffix("()", string.Empty, StringComparison.Ordinal))
                    .WithMemberType(MemberType.Function), x.ArgAt<ExpressionEvaluatorContext>(0)), x.ArgAt<CancellationToken>(1))
                : EvaluateExpression(x));
        Evaluator
            .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
            .Returns(x => Expression.EvaluateAsync(x.ArgAt<ExpressionEvaluatorContext>(0), x.ArgAt<CancellationToken>(1)));
    }

    [Fact]
    public async Task GetArgumentValueResult_Returns_Invalid_When_Argument_Is_Not_Present()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentValueResultAsync(1, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public async Task GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Constant()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("some value");
    }

    [Fact]
    public async Task GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Constant_And_Ignores_DefaultValue()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentValueResultAsync(0, "SomeName", context, (object)"ignored", CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("some value");
    }

    [Fact]
    public async Task GetArgumentValueResult_Returns_Success_When_Argument_Is_Present_And_Function()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("MyNestedFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("Evaluated result");
    }

    [Fact]
    public async Task GetArgumentValueResult_Returns_Success_With_DefaultValue_When_Argument_Is_Not_Present_But_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentValueResultAsync(0, "SomeName", context, (object)"some value", CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("some value");
    }

    [Fact]
    public async Task GetArgumentStringValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentStringValueResultAsync(1, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public async Task GetArgumentStringValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentStringValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type string");
    }

    [Fact]
    public async Task GetArgumentStringValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentStringValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("some value");
    }

    [Fact]
    public async Task GetArgumentStringValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentStringValueResultAsync(0, "SomeName", context, "default value", CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("default value");
    }

    [Fact]
    public async Task GetArgumentInt32ValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt32ValueResultAsync(1, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public async Task GetArgumentInt32ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Int32()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt32ValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(1);
    }

    [Fact]
    public async Task GetArgumentInt32ValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt32ValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type integer");
    }

    [Fact]
    public async Task GetArgumentInt32ValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt32ValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type integer");
    }

    [Fact]
    public async Task GetArgumentInt32ValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Int32()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt32ValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type integer");
    }

    [Fact]
    public async Task GetArgumentInt32ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Int32()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt32ValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13);
    }

    [Fact]
    public async Task GetArgumentInt32ValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt32ValueResultAsync(0, "SomeName", context, 13, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13);
    }

    [Fact]
    public async Task GetArgumentInt64ValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt64ValueResultAsync(1, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public async Task GetArgumentInt64ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Int64()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("LongFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt64ValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(1L);
    }

    [Fact]
    public async Task GetArgumentInt64ValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt64ValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type long integer");
    }

    [Fact]
    public async Task GetArgumentInt64ValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt64ValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type long integer");
    }

    [Fact]
    public async Task GetArgumentInt64ValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Int64()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt64ValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type long integer");
    }

    [Fact]
    public async Task GetArgumentInt64ValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Int64()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("LongFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt64ValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13L);
    }

    [Fact]
    public async Task GetArgumentInt64ValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentInt64ValueResultAsync(0, "SomeName", context, 13L, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13L);
    }

    [Fact]
    public async Task GetArgumentDecimalValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDecimalValueResultAsync(1, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public async Task GetArgumentDecimalValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Decimal()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DecimalFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDecimalValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(1L);
    }

    [Fact]
    public async Task GetArgumentDecimalValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDecimalValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type decimal");
    }

    [Fact]
    public async Task GetArgumentDecimalValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDecimalValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type decimal");
    }

    [Fact]
    public async Task GetArgumentDecimalValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Decimal()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDecimalValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type decimal");
    }

    [Fact]
    public async Task GetArgumentDecimalValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Decimal()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DecimalFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDecimalValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13M);
    }

    [Fact]
    public async Task GetArgumentDecimalValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDecimalValueResultAsync(0, "SomeName", context, 13.5M, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(13.5M);
    }

    [Fact]
    public async Task GetArgumentBooleanValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentBooleanValueResultAsync(1, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public async Task GetArgumentBooleanValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Boolean()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("BooleanFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentBooleanValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public async Task GetArgumentBooleanValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentBooleanValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type boolean");
    }

    [Fact]
    public async Task GetArgumentBooleanValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentBooleanValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type boolean");
    }

    [Fact]
    public async Task GetArgumentBooleanValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_Boolean()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentBooleanValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type boolean");
    }

    [Fact]
    public async Task GetArgumentBooleanValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_Boolean()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("BooleanFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentBooleanValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public async Task GetArgumentBooleanValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentBooleanValueResultAsync(0, "SomeName", context, true, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeTrue();
    }

    [Fact]
    public async Task GetArgumentDateTimeValueResult_Returns_Invalid_When_ArgumentValue_Is_Invalid()
    {
        // Arrange
        var argument = CreateFunctionCallWithConstantArgument();
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDateTimeValueResultAsync(1, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Missing argument: SomeName");
    }

    [Fact]
    public async Task GetArgumentDateTimeValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_DateTime()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDateTimeValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(DateTime.Today);
    }

    [Fact]
    public async Task GetArgumentDateTimeValueResult_Returns_Invalid_When_ArgumentValue_Is_Not_Of_Type_String()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunction");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDateTimeValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type datetime");
    }

    [Fact]
    public async Task GetArgumentDateTimeValueResult_Returns_Parse_Result_Status_When_ArgumentValue_Is_String_But_Could_Not_Be_Parsed_As_Expression()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("UnknownExpressionString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDateTimeValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type datetime");
    }

    [Fact]
    public async Task GetArgumentDateTimeValueResult_Returns_Invalid_When_ArgumentValue_Is_String_But_Is_Parsed_To_Something_Else_Than_DateTime()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("NumericFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDateTimeValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("SomeName is not of type datetime");
    }

    [Fact]
    public async Task GetArgumentDateTimeValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_String_And_Is_Parsed_To_DateTime()
    {
        // Arrange
        var argument = CreateFunctionCallWithFunctionArgument("DateTimeFunctionAsString");
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDateTimeValueResultAsync(0, "SomeName", context, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(DateTime.Today);
    }

    [Fact]
    public async Task GetArgumentDateTimeValueResult_Returns_Success_With_DefaultValue_When_ArgumentValue_Is_Not_Found_And_DefaultValue_Is_Supplied()
    {
        // Arrange
        var argument = CreateFunctionCallWithoutArguments();
        var dt = DateTime.Now;
        var expressionEvaluatorContext = CreateContext("Dummy");
        var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build(), expressionEvaluatorContext);

        // Act
        var result = await argument.GetArgumentDateTimeValueResultAsync(0, "SomeName", context, dt, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(dt);
    }

    private static FunctionCall CreateFunctionCallWithoutArguments()
        => new FunctionCallBuilder()
            .WithName("Test")
            .WithMemberType(MemberType.Function)
            .Build();

    private static FunctionCall CreateFunctionCallWithConstantArgument()
        => new FunctionCallBuilder()
            .WithName("Test")
            .WithMemberType(MemberType.Function)
            .AddArguments("\"some value\"")
            .Build();

    private static FunctionCall CreateFunctionCallWithFunctionArgument(string functionName)
        => new FunctionCallBuilder()
            .WithName("Test")
            .WithMemberType(MemberType.Function)
            .AddArguments($"{functionName}()")
            .Build();
}
