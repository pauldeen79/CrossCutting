namespace CrossCutting.Utilities.Parsers.Tests;

public class FunctionCallContextTests : IDisposable
{
    private readonly IFunctionEvaluator _functionEvaluatorMock = Substitute.For<IFunctionEvaluator>();
    private readonly IExpressionEvaluator _expressionEvaluatorMock = Substitute.For<IExpressionEvaluator>();
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;
    private bool disposedValue;

    protected static FunctionEvaluatorSettings CreateSettings()
        => new FunctionEvaluatorSettingsBuilder();

    public FunctionCallContextTests()
    {
        _functionEvaluatorMock
            //<FunctionParseResult, IExpressionParser, object?>((result, _, _)
            .Evaluate(Arg.Any<FunctionCall>(), Arg.Any<FunctionEvaluatorSettings>(), Arg.Any<object?>())
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
        _expressionEvaluatorMock
            .Evaluate(Arg.Any<string>(), Arg.Any<ExpressionEvaluatorSettings>(), Arg.Any<object?>())
            .Returns(Result.Success<object?>("some value"));
        _provider = new ServiceCollection().AddParsers().BuildServiceProvider(true);
        _scope = _provider.CreateScope();
    }

    public class Constructor : FunctionCallContextTests
    {
        [Fact]
        public void Throws_On_Null_FunctionCall()
        {
            // Act & Assert
            Action a = () => _ = new FunctionCallContext(null!, Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), CreateSettings(), null);
            a.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Throws_On_Null_FunctionEvaluator()
        {
            // Act & Assert
            Action a = () => _ = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), null!, Substitute.For<IExpressionEvaluator>(), CreateSettings(), null);
            a.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Throws_On_Null_ExpressionEvaluator()
        {
            // Act & Assert
            Action a = () => _ = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), null!, CreateSettings(), null);
            a.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Throws_On_Null_FormatProvider()
        {
            // Act & Assert
            Action a = () => _ = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), null!, null);
            a.ShouldThrow<ArgumentNullException>();
        }
    }

    public class GetArgumentValueResultNoDefaultValue : FunctionCallContextTests
    {
        [Fact]
        public void Returns_Success_When_Argument_Is_Present_And_Constant()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithConstantArgument();

            // Act
            var result = sut.GetArgumentValueResult(0, "SomeName");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("some value");
        }
    }

    public class GetArgumentValueResultDefaultValue : FunctionCallContextTests
    {
        [Fact]
        public void Returns_Success_When_Argument_Is_Present_And_Constant()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithConstantArgument();

            // Act
            var result = sut.GetArgumentValueResult(0, "SomeName", (object?)"ignored default value");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("some value");
        }
    }

    public class GetArgumentValueResultGenericNoDefaultValue : FunctionCallContextTests
    {
        [Fact]
        public void Returns_Success_When_Argument_Is_Present_And_Constant()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithConstantArgument();

            // Act
            var result = sut.GetArgumentValueResult<string>(0, "SomeName");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("some value");
        }
    }

    public class GetArgumentValueResultGenericDefaultValue : FunctionCallContextTests
    {
        [Fact]
        public void Returns_Success_When_Argument_Is_Present_And_Constant()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithConstantArgument();

            // Act
            var result = sut.GetArgumentValueResult(0, "SomeName", "ignored default value");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("some value");
        }
    }

    public class GetArgumentStringValueResultNoDefaultValue : FunctionCallContextTests
    {
        [Fact]
        public void Returns_Success_When_Argument_Is_Present_And_Constant()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithConstantArgument();

            // Act
            var result = sut.GetArgumentStringValueResult(0, "SomeName");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("some value");
        }
    }

    public class GetArgumentStringValueResultDefaultValue : FunctionCallContextTests
    {
        [Fact]
        public void Returns_Success_When_Argument_Is_Present_And_Constant()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithConstantArgument();

            // Act
            var result = sut.GetArgumentStringValueResult(0, "SomeName", "ignored default value");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("some value");
        }
    }

    public class GetArgumentInt32ValueResult : FunctionCallContextTests
    {
        [Fact]
        public void Returns_Success_When_ArgumentValue_Is_Of_Type_Int32()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithFunctionArgument("NumericFunction");

            // Act
            var result = sut.GetArgumentInt32ValueResult(0, "SomeName");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1);
        }

        [Fact]
        public void Returns_Success_When_ArgumentValue_Is_Of_Type_Int32_Ignored_DefaultValue()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithFunctionArgument("NumericFunction");

            // Act
            var result = sut.GetArgumentInt32ValueResult(0, "SomeName", 13);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1);
        }
    }

    public class GetArgumentInt64ValueResult : FunctionCallContextTests
    {
        [Fact]
        public void Returns_Success_When_ArgumentValue_Is_Of_Type_Int64()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithFunctionArgument("LongFunction");

            // Act
            var result = sut.GetArgumentInt64ValueResult(0, "SomeName");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1L);
        }

        [Fact]
        public void Returns_Success_When_ArgumentValue_Is_Of_Type_Int64_Ignored_DefaultValue()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithFunctionArgument("LongFunction");

            // Act
            var result = sut.GetArgumentInt64ValueResult(0, "SomeName", 13);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1L);
        }
    }

    public class GetArgumentDecimalValueResult : FunctionCallContextTests
    {
        [Fact]
        public void Returns_Success_When_ArgumentValue_Is_Of_Type_Decimal()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithFunctionArgument("DecimalFunction");

            // Act
            var result = sut.GetArgumentDecimalValueResult(0, "SomeName");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1L);
        }

        [Fact]
        public void Returns_Success_When_ArgumentValue_Is_Of_Type_Decimal_Ignored_DefaultValue()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithFunctionArgument("DecimalFunction");

            // Act
            var result = sut.GetArgumentDecimalValueResult(0, "SomeName", 3.4M);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(1L);
        }
    }

    public class GetArgumentBooleanValueResult : FunctionCallContextTests
    {
        [Fact]
        public void GetArgumentBooleanValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Boolean()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithFunctionArgument("BooleanFunction");

            // Act
            var result = sut.GetArgumentBooleanValueResult(0, "SomeName");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }

        [Fact]
        public void GetArgumentBooleanValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_Boolean_Ignored_DefaultValue()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithFunctionArgument("BooleanFunction");

            // Act
            var result = sut.GetArgumentBooleanValueResult(0, "SomeName", false);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }
    }

    public class GetArgumentDateTimeValueResult : FunctionCallContextTests
    {
        [Fact]
        public void GetArgumentDateTimeValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_DateTime()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithFunctionArgument("DateTimeFunction");

            // Act
            var result = sut.GetArgumentDateTimeValueResult(0, "SomeName");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(DateTime.Today);
        }

        [Fact]
        public void GetArgumentDateTimeValueResult_Returns_Success_When_ArgumentValue_Is_Of_Type_DateTime_Ignored_DefautValue()
        {
            // Arrange
            var sut = CreateFunctionCallContextWithFunctionArgument("DateTimeFunction");

            // Act
            var result = sut.GetArgumentDateTimeValueResult(0, "SomeName", DateTime.Today.AddDays(-1));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(DateTime.Today);
        }
    }

    protected FunctionCallContext CreateFunctionCallContextWithoutArguments()
        => new FunctionCallContext(new FunctionCallBuilder()
            .WithName("Test")
            .Build(), _functionEvaluatorMock, _expressionEvaluatorMock, CreateSettings(), null);

    protected FunctionCallContext CreateFunctionCallContextWithConstantArgument()
        => new FunctionCallContext(new FunctionCallBuilder()
            .WithName("Test")
            .AddArguments(new ConstantArgumentBuilder().WithValue("some value"))
            .Build(), _functionEvaluatorMock, _expressionEvaluatorMock, CreateSettings(), null);

    protected FunctionCallContext CreateFunctionCallContextWithFunctionArgument(string functionName)
        => new FunctionCallContext(new FunctionCallBuilder()
            .WithName("Test")
            .AddArguments(new FunctionArgumentBuilder().WithFunction(new FunctionCallBuilder().WithName(functionName)))
            .Build(), _functionEvaluatorMock, _expressionEvaluatorMock, CreateSettings(), null);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _scope.Dispose();
                _provider.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
