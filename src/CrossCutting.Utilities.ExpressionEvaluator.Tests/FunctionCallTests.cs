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
