namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class FunctionEvaluatorTests : IDisposable
{
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;

    public FunctionEvaluatorTests()
    {
        _provider = new ServiceCollection()
        .AddParsers()
            .AddSingleton<IFunction, MyFunction>()
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
    }

    [Fact]
    public void Evaluate_Returns_Invalid_On_Null_Input()
    {
        // Arrange
        var functionCall = default(FunctionCall);
        var parser = Substitute.For<IExpressionParser>();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Function call is required");
    }

    [Fact]
    public void Evaluate_Returns_First_Success_Result_From_FunctionResultParser()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithFunctionName("MyFunction").Build();
        var parser = Substitute.For<IExpressionParser>();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("function result");
    }

    [Fact]
    public void Evaluate_Returns_First_Failure_Result_From_FunctionResultParser()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithFunctionName("Error").Build();
        var parser = Substitute.For<IExpressionParser>();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom");
    }

    [Fact]
    public void Evaluate_Returns_NotSupported_When_FunctionCall_Is_Unknown()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithFunctionName("WrongName").Build();
        var parser = Substitute.For<IExpressionParser>();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.NotSupported);
        result.ErrorMessage.Should().Be("Unknown function: WrongName");
    }

    [Fact]
    public void Validate_Returns_Invalid_On_Null_Input()
    {
        // Arrange
        var functionCall = default(FunctionCall);
        var parser = Substitute.For<IExpressionParser>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Function call is required");
    }

    [Fact]
    public void Validate_Returns_First_Success_Result_From_FunctionResultParser()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithFunctionName("MyFunction").Build();
        var parser = Substitute.For<IExpressionParser>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Validate_Returns_First_Failure_Result_From_FunctionResultParser()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithFunctionName("Error").Build();
        var parser = Substitute.For<IExpressionParser>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom");
    }

    [Fact]
    public void Validate_Returns_Invalid_When_FunctionCall_Is_Unknown()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithFunctionName("WrongName").Build();
        var parser = Substitute.For<IExpressionParser>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Unknown function: WrongName");
    }

    public void Dispose()
    {
        _scope.Dispose();
        _provider.Dispose();
    }

    private IFunctionEvaluator CreateSut() => _scope.ServiceProvider.GetRequiredService<IFunctionEvaluator>();

    private sealed class MyFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCall functionCall, object? context, IFunctionEvaluator evaluator, IExpressionParser parser)
        {
            if (!functionCall.FunctionName.In("MyFunction", "Error"))
            {
                return Result.Continue<object?>();
            }

            if (functionCall.FunctionName == "Error")
            {
                return Result.Error<object?>("Kaboom");
            }

            return Result.Success<object?>("function result");
        }

        public Result Validate(FunctionCall functionCall, object? context, IFunctionEvaluator evaluator, IExpressionParser parser)
        {
            if (!functionCall.FunctionName.In("MyFunction", "Error"))
            {
                return Result.Continue();
            }

            if (functionCall.FunctionName == "Error")
            {
                return Result.Error<object?>("Kaboom");
            }

            // Aparently, this function does not care about the given arguments
            return Result.Success();
        }
    }
}
