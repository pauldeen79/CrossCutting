namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class FunctionCallContextTests : TestBase
{
    public FunctionCallContextTests()
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
                "DateTimeFunctionAsString" => Result.Success<object?>(DateTime.Today.ToString(CultureInfo.InvariantCulture)),
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
                : new ExpressionEvaluatorMock(Expression).EvaluateAsync(x.ArgAt<ExpressionEvaluatorContext>(0), x.ArgAt<CancellationToken>(1)));
    }

    public class Constructor : FunctionCallContextTests
    {
        [Fact]
        public void Throws_On_Null_FunctionCall()
        {
            // Act & Assert
            Action a = () => _ = new FunctionCallContext(null!, CreateContext("Dummy"));
            a.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Throws_On_Null_FunctionEvaluator()
        {
            // Act & Assert
            Action a = () => _ = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), null!);
            a.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Throws_On_MemberType_Unknown()
        {
            // Act & Assert
            Action a = () => _ = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), CreateContext("Dummy"));
            a.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void Throws_On_No_FunctionCall_Found()
        {
            // Act & Assert
            Action a = () => _ = new FunctionCallContext(new DotExpressionComponentState(CreateContext("Dummy"), new FunctionParser(), Result.Continue<object?>(), "function()"));
            a.ShouldThrow<InvalidOperationException>();
        }
    }

    public class GetArgumentValueResultNoDefaultValue : FunctionCallContextTests
    {
        [Fact]
        public async Task Returns_Success_When_Argument_Is_Present_And_Constant()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithConstantArgument();

            // Act
            var result = await sut.GetArgumentValueResultAsync(0, "SomeName", CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("some value");
        }
    }

    public class GetArgumentValueResultDefaultValue : FunctionCallContextTests
    {
        [Fact]
        public async Task Returns_Success_When_Argument_Is_Present_And_Constant()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithConstantArgument();

            // Act
            var result = await sut.GetArgumentValueResultAsync(0, "SomeName", (object?)"ignored default value", CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("some value");
        }
    }

    public class GetArgumentValueResultGenericNoDefaultValue : FunctionCallContextTests
    {
        [Fact]
        public async Task Returns_Success_When_Argument_Is_Present_And_Constant()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithConstantArgument();

            // Act
            var result = await sut.GetArgumentValueResultAsync<string>(0, "SomeName", CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("some value");
        }
    }

    public class GetArgumentValueResultGenericDefaultValue : FunctionCallContextTests
    {
        [Fact]
        public async Task Returns_Success_When_Argument_Is_Present_And_Constant()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithConstantArgument();

            // Act
            var result = await sut.GetArgumentValueResultAsync(0, "SomeName", "ignored default value", CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("some value");
        }
    }

    public class GetInstanceValueResult : FunctionCallContextTests
    {
        [Fact]
        public async Task Returns_Error_When_InstanceValue_Is_Of_Wrong_Type()
        {
            // Arrange
            var state = CreateDotExpressionComponentState("\"string value\".Dummy()", "string value", "Dummy()");
            var sut = new FunctionCallContext(state);

            // Act
            var result = await sut.GetInstanceValueResultAsync<int>(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Instance value is not of type System.Int32");
        }

        [Fact]
        public async Task Returns_Error_When_InstanceValue_Is_Null()
        {
            // Arrange
            var state = CreateDotExpressionComponentState("null.Dummy()", null, "Dummy()");
            var sut = new FunctionCallContext(state);

            // Act
            var result = await sut.GetInstanceValueResultAsync<int>(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Instance value is null");
        }

        [Fact]
        public async Task Returns_Ok_When_InstanceValue_Is_Of_Correct_Type()
        {
            // Arrange
            var state = CreateDotExpressionComponentState("\"string value\".Dummy()", "string value", "Dummy()");
            var sut = new FunctionCallContext(state);

            // Act
            var result = await sut.GetInstanceValueResultAsync<string>(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("string value");
        }
    }

    protected FunctionCallContext CreateFunctionCallContextWithConstantArgument()
        => new FunctionCallContext(new FunctionCallBuilder()
            .WithName("Test")
            .WithMemberType(MemberType.Function)
            .AddArguments("\"some value\"")
            .Build(), CreateContext("Dummy"));

    protected FunctionCallContext CreateFunctionCallContextWithFunctionArgument(string functionName)
        => new FunctionCallContext(new FunctionCallBuilder()
            .WithName("Test")
            .WithMemberType(MemberType.Function)
            .AddArguments($"{functionName}()")
            .Build(), CreateContext("Dummy"));
}
