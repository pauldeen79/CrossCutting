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
            .AddSingleton<IFunction, ErrorFunction>()
            .AddSingleton<IFunction, OverloadTest1>()
            .AddSingleton<IFunction, OverloadTest2>()
            .AddSingleton<IFunction, OverloadTest3>()
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
    }

    [Fact]
    public void Evaluate_Returns_Invalid_On_Null_Input()
    {
        // Arrange
        var functionCall = default(FunctionCall);
        var parser = Substitute.For<IExpressionEvaluator>();
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
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();
        var parser = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, parser, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("function result");
    }

    [Fact]
    public void Evaluate_Returns_First_Failure_Result_From_FunctionResultParser()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Error").Build();
        var parser = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, parser, default(object?));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom");
    }

    [Fact]
    public void Evaluate_Returns_Invalid_When_FunctionCall_Is_Unknown()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("WrongName").Build();
        var parser = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Unknown function: WrongName");
    }

    [Fact]
    public void Validate_Returns_Invalid_On_Null_Input()
    {
        // Arrange
        var functionCall = default(FunctionCall);
        var parser = Substitute.For<IExpressionEvaluator>();
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
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();
        var parser = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, parser, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Validate_Returns_First_Failure_Result_From_FunctionResultParser()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Error").Build();
        var parser = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, parser, default(object?));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom");
    }

    [Fact]
    public void Validate_Returns_Invalid_When_FunctionCall_Is_Unknown()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("WrongName").Build();
        var parser = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Unknown function: WrongName");
    }

    [Fact]
    public void Validate_Returns_Invalid_When_Overload_Does_Not_Have_The_Right_Amount_Of_Arguments()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Overload").Build();
        var parser = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("No overload of the Overload function takes 0 arguments");
    }

    [Fact]
    public void Validate_Returns_Invalid_When_Overload_ArgumentCount_Is_Registered_Twice()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Overload").AddArguments(new EmptyArgumentBuilder(), new EmptyArgumentBuilder()).Build();
        var parser = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Function Overload with 2 arguments could not be identified uniquely");
    }

    [Fact]
    public void Validate_Returns_Result_From_Validation_When_Overload_ArgumentCount_Is_Correct_And_Registered_Once()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Overload").AddArguments(new EmptyArgumentBuilder()).Build();
        var parser = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, parser);

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
    }

    [Fact]
    public void Validate_Returns_Error_When_Function_Could_Not_Be_Found_For_FunctionDescriptor()
    {
        // Arrange
        var functionDescriptorProvider = Substitute.For<IFunctionDescriptorProvider>();
        var functionDescriptor = new FunctionDescriptorBuilder().WithName("MyFunction").WithTypeName("MyTypeName").Build();
        functionDescriptorProvider.GetAll().Returns([functionDescriptor]);
        var sut = new FunctionEvaluator(functionDescriptorProvider, Enumerable.Empty<IFunction>());
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();

        // Act
        var result = sut.Validate(functionCall, expressionEvaluator);

        // Arrange
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Could not find function with type name MyTypeName");
    }

    public void Dispose()
    {
        _scope.Dispose();
        _provider.Dispose();
    }

    private IFunctionEvaluator CreateSut() => _scope.ServiceProvider.GetRequiredService<IFunctionEvaluator>();

    private sealed class ErrorFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        {
            return Result.Error<object?>("Kaboom");
        }

        public Result Validate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        {
            return Result.Error<object?>("Kaboom");
        }
    }

    [FunctionName("MyFunction")]
    private sealed class MyFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        {
            return Result.Success<object?>("function result");
        }

        public Result Validate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        {
            return Result.Success();
        }
    }

    [FunctionName("Overload")]
    [FunctionArgument("Argument1", typeof(string))]
    private sealed class OverloadTest1 : IFunction
    {
        public Result<object?> Evaluate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        {
            throw new NotImplementedException();
        }

        public Result Validate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        {
            return Result.Continue();
        }
    }

    [FunctionName("Overload")]
    [FunctionArgument("Argument1", typeof(string))]
    [FunctionArgument("Argument2", typeof(string))]
    private sealed class OverloadTest2 : IFunction
    {
        public Result<object?> Evaluate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        {
            throw new NotImplementedException();
        }

        public Result Validate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        {
            return Result.Continue();
        }
    }

    [FunctionName("Overload")]
    [FunctionArgument("Argument1", typeof(string))]
    [FunctionArgument("Argument2", typeof(int))]
    private sealed class OverloadTest3 : IFunction
    {
        public Result<object?> Evaluate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        {
            throw new NotImplementedException();
        }

        public Result Validate(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
        {
            return Result.Continue();
        }
    }
}
