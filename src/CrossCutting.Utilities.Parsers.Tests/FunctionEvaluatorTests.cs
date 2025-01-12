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
            .AddSingleton<IFunction, MyTypedFunction>()
            .AddSingleton<IFunction, MyFunction2>()
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
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Function call is required");
    }

    [Fact]
    public void Evaluate_Returns_Success_Result_On_Valid_Input()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, expressionEvaluator, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("function result");
    }

    [Fact]
    public void Evaluate_Returns_Failure_Result_On_Failure()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Error").Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, expressionEvaluator, default(object?));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom");
    }

    [Fact]
    public void Evaluate_Returns_Success_Failure_On_Non_Valid_Input()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyFunction2").AddArguments(new EmptyArgumentBuilder()).Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, expressionEvaluator, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not evaluate function MyFunction2, see inner results for more details");
        result.InnerResults.Should().ContainSingle();
        result.InnerResults.Single().ErrorMessage.Should().Be("Argument Argument1 is required");
    }

    [Fact]
    public void Evaluate_Returns_Invalid_Result_When_FunctionCall_Is_Unknown()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("WrongName").Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Unknown function: WrongName");
    }

    [Fact]
    public void EvaluateTyped_Returns_Invalid_Result_On_Null_Input()
    {
        // Arrange
        var functionCall = default(FunctionCall);
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<string>(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Function call is required");
    }

    [Fact]
    public void EvaluateTyped_Returns_Invalid_Result_On_Wrong_Return_Type()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<int>(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not cast System.String to System.Int32");
    }

    [Fact]
    public void EvaluateTyped_Returns_Success_Result_On_Valid_Input_Untyped()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<string>(functionCall!, expressionEvaluator, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("function result");
    }

    [Fact]
    public void EvaluateTyped_Returns_Success_Result_On_Valid_Input_Typed()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyTypedFunction").Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<string>(functionCall!, expressionEvaluator, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("function result");
    }

    [Fact]
    public void EvaluateTyped_Returns_Failure_Result_On_Failure()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Error").Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<string>(functionCall!, expressionEvaluator, default(object?));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom");
    }

    [Fact]
    public void EvaluateTyped_Returns_Invalid_Result_When_FunctionCall_Is_Unknown()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("WrongName").Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<string>(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Unknown function: WrongName");
    }

    [Fact]
    public void Validate_Returns_Invalid_Result_On_Null_Input()
    {
        // Arrange
        var functionCall = default(FunctionCall);
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Function call is required");
    }

    [Fact]
    public void Validate_Returns_Success_Result_On_Valid_Input()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, expressionEvaluator, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Validate_Returns_Failure_Result_On_Valid_Input()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Error").Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, expressionEvaluator, default(object?));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom");
    }

    [Fact]
    public void Validate_Returns_Invalid_Result_When_FunctionCall_Is_Unknown()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("WrongName").Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Unknown function: WrongName");
    }

    [Fact]
    public void Validate_Returns_Invalid_Result_When_Overload_Does_Not_Have_The_Right_Amount_Of_Arguments()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Overload").Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("No overload of the Overload function takes 0 arguments");
    }

    [Fact]
    public void Validate_Returns_Invalid_Result_When_Overload_ArgumentCount_Is_Registered_Twice()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Overload").AddArguments(new EmptyArgumentBuilder(), new EmptyArgumentBuilder()).Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Function Overload with 2 arguments could not be identified uniquely");
    }

    [Fact]
    public void Validate_Returns_Result_From_Validation_When_Overload_ArgumentCount_Is_Correct_And_Registered_Once()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Overload").AddArguments(new ConstantArgumentBuilder().WithValue("some value")).Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                           .Returns(Result.Success<object?>("some value"));
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
    }

    [Fact]
    public void Validate_Returns_Result_From_Validation_When_Overload_ArgumentCount_Is_Correct_But_Argument_Is_Of_Wrong_Type()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Overload").AddArguments(new ConstantArgumentBuilder().WithValue("13")).Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                           .Returns(Result.Success<object?>(13));
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not evaluate function Overload, see inner results for more details");
        result.InnerResults.Should().ContainSingle();
        result.InnerResults.Single().Status.Should().Be(ResultStatus.Invalid);
        result.InnerResults.Single().ErrorMessage.Should().Be("Argument Argument1 is not of type System.String");
    }

    [Fact]
    public void Validate_Returns_Result_From_Validation_When_Overload_ArgumentCount_Is_Correct_But_Argument_Has_Non_Succesful_Result()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Overload").AddArguments(new ConstantResultArgumentBuilder().WithResult(Result.Error<object?>("Kaboom"))).Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                           .Returns(Result.Success<object?>(13));
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not evaluate function Overload, see inner results for more details");
        result.InnerResults.Should().ContainSingle();
        result.InnerResults.Single().Status.Should().Be(ResultStatus.Error);
        result.InnerResults.Single().ErrorMessage.Should().Be("Kaboom");
    }

    [Fact]
    public void Validate_Returns_Result_From_Validation_When_Overload_ArgumentCount_Is_Correct_But_Required_Argument_Is_Missing()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Overload").AddArguments(new ConstantArgumentBuilder().WithValue("null")).Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                           .Returns(Result.Success<object?>(null));
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not evaluate function Overload, see inner results for more details");
        result.InnerResults.Should().ContainSingle();
        result.InnerResults.Single().Status.Should().Be(ResultStatus.Invalid);
        result.InnerResults.Single().ErrorMessage.Should().Be("Argument Argument1 is required");
    }

    [Fact]
    public void Validate_Returns_Non_Successful_Result_As_Invalid_From_Validation_When_Overload_ArgumentCount_Is_Correct_And_Registered_Once()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyFunction2").AddArguments(new ConstantArgumentBuilder().WithValue("some value")).Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                           .Returns(Result.Success<object?>("some value"));
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, expressionEvaluator);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Custom validation message");
    }

    [Fact]
    public void Validate_Returns_Error_Result_When_Function_Could_Not_Be_Found_For_FunctionDescriptor()
    {
        // Arrange
        var functionDescriptorProvider = Substitute.For<IFunctionDescriptorProvider>();
        var functionDescriptor = new FunctionDescriptorBuilder()
            .WithName("MyFunction")
            .WithFunctionType(typeof(string))
            .Build();
        functionDescriptorProvider.GetAll().Returns([functionDescriptor]);
        var sut = new FunctionEvaluator(functionDescriptorProvider, Enumerable.Empty<IFunction>());
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();

        // Act
        var result = sut.Validate(functionCall, expressionEvaluator);

        // Arrange
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Could not find function with type name System.String");
    }

    public void Dispose()
    {
        _scope.Dispose();
        _provider.Dispose();
    }

    private IFunctionEvaluator CreateSut() => _scope.ServiceProvider.GetRequiredService<IFunctionEvaluator>();

    private sealed class ErrorFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Error<object?>("Kaboom");
        }

        public Result Validate(FunctionCallContext context)
        {
            return Result.Error<object?>("Kaboom");
        }
    }

    [FunctionName("MyFunction")]
    private sealed class MyFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Success<object?>("function result");
        }

        public Result Validate(FunctionCallContext context)
        {
            return Result.Success();
        }
    }

    [FunctionName("MyTypedFunction")]
    private sealed class MyTypedFunction : ITypedFunction<string>
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Success<object?>("function result");
        }

        public Result<string> EvaluateTyped(FunctionCallContext context)
        {
            return Result.Success<string>("function result");
        }

        public Result Validate(FunctionCallContext context)
        {
            return Result.Success();
        }
    }

    [FunctionName("MyFunction2")]
    [FunctionArgument("Argument1", typeof(string))]
    private sealed class MyFunction2 : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Success<object?>("function result");
        }

        public Result Validate(FunctionCallContext context)
        {
            return Result.Invalid("Custom validation message");
        }
    }

    [FunctionName("Overload")]
    [FunctionArgument("Argument1", typeof(string))]
    private sealed class OverloadTest1 : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            throw new NotImplementedException();
        }

        public Result Validate(FunctionCallContext context)
        {
            return Result.Continue();
        }
    }

    [FunctionName("Overload")]
    [FunctionArgument("Argument1", typeof(string))]
    [FunctionArgument("Argument2", typeof(string))]
    private sealed class OverloadTest2 : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            throw new NotImplementedException();
        }

        public Result Validate(FunctionCallContext context)
        {
            return Result.Continue();
        }
    }

    [FunctionName("Overload")]
    [FunctionArgument("Argument1", typeof(string))]
    [FunctionArgument("Argument2", typeof(int))]
    private sealed class OverloadTest3 : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            throw new NotImplementedException();
        }

        public Result Validate(FunctionCallContext context)
        {
            return Result.Continue();
        }
    }
}
