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
            .AddSingleton<IFunction, PassThroughFunction>()
            .AddSingleton<IFunction, OptionalParameterTest>()
            .AddSingleton<IFunction, AnimalFunction>()
            .AddSingleton<IFunction, MyInterfaceFunction>()
            .AddSingleton<IFunction, ObjectArgumentFunction>()
            .AddSingleton<IFunction, ObjectResultFunction>()
            .AddSingleton<IFunction, StringArgumentFunction>()
            .AddSingleton<IGenericFunction, CastFunction>()
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
    }

    [Fact]
    public void Evaluate_Returns_Invalid_On_Null_Input()
    {
        // Arrange
        var functionCall = default(FunctionCall);
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall!, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Function call is required");
    }

    [Fact]
    public void Evaluate_Returns_Success_Result_On_Valid_Input()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("function result");
    }

    [Fact]
    public void Evaluate_Returns_Success_Result_On_Valid_Input_With_Optional_Parameter()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Optional").Build();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(string.Empty);
    }

    [Fact]
    public void Evaluate_Returns_Failure_Result_On_Failure()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Error").Build();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall, CreateSettings(), default(object?));

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Kaboom");
    }

    [Fact]
    public void Evaluate_Returns_Failure_On_Non_Valid_Input()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("MyFunction2")
            .AddArguments(new EmptyArgumentBuilder())
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Could not evaluate function MyFunction2, see inner results for more details");
        result.InnerResults.Count.ShouldBe(1);
        result.InnerResults.Single().ErrorMessage.ShouldBe("Argument Argument1 is required");
    }

    [Fact]
    public void Evaluate_Returns_Invalid_Result_When_FunctionCall_Is_Unknown()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("WrongName").Build();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Unknown function: WrongName");
    }

    [Fact]
    public void Evaluate_Returns_Result_Of_DyamicDescriptorsFunction_When_Available()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("PassThrough").Build();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall, CreateSettings(), default(object?));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("Custom value");

    }

    [Fact]
    public void EvaluateTyped_Returns_Invalid_Result_On_Null_Input()
    {
        // Arrange
        var functionCall = default(FunctionCall);
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<string>(functionCall!, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Function call is required");
    }

    [Fact]
    public void EvaluateTyped_Returns_Invalid_Result_On_Wrong_Return_Type()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<int>(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Could not cast System.String to System.Int32");
    }

    [Fact]
    public void EvaluateTyped_Returns_Success_Result_On_Valid_Input_Untyped()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<string>(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("function result");
    }

    [Fact]
    public void EvaluateTyped_Returns_Success_Result_On_Valid_Input_Typed()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyTypedFunction").Build();
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<string>(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("function result");
    }

    [Fact]
    public void EvaluateTyped_Returns_Failure_Result_On_Failure()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Error").Build();
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<string>(functionCall, CreateSettings(), default(object?));

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Kaboom");
    }

    [Fact]
    public void EvaluateTyped_Returns_Invalid_Result_When_FunctionCall_Is_Unknown()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("WrongName").Build();
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<string>(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Unknown function: WrongName");
    }

    [Fact]
    public void Validate_Returns_Invalid_Result_On_Null_Input()
    {
        // Arrange
        var functionCall = default(FunctionCall);
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall!, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Function call is required");
    }

    [Fact]
    public void Validate_Returns_Success_Result_On_Valid_Input()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(string));
    }

    [Fact]
    public void Validate_Returns_Success_Result_On_Valid_Input_With_Derived_Type()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("AnimalFunction")
            .AddArguments(new ConstantArgumentBuilder().WithValue(new Monkey()))
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeNull();
    }

    [Fact]
    public void Validate_Returns_Success_Result_On_Valid_Input_With_Interface_Type()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("MyInterfaceFunction")
            .AddArguments(new ConstantArgumentBuilder().WithValue(new MyImplementation()))
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeNull();
    }

    [Fact]
    public void Validate_Returns_Success_Result_On_Valid_Input_With_Typed_Result()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("MyTypedFunction").Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(typeof(string));
    }

    [Fact]
    public void Validate_Returns_Success_Result_On_Valid_Input_With_Optional_Parameter()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Optional").Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Validate_Returns_Success_Result_On_Valid_Input_With_Object_Argument()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("ObjectArgumentFunction")
            .AddArguments(new ConstantArgumentBuilder().WithValue(13)) // Int32 is a value type, which needs a special case on Type.IsAssignablFrom
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Validate_Returns_Failure_Result_On_Valid_Input()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Error").Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings(), default(object?));

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Kaboom");
    }

    [Fact]
    public void Validate_Returns_Invalid_Result_When_FunctionCall_Is_Unknown()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("WrongName").Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Unknown function: WrongName");
    }

    [Fact]
    public void Validate_Returns_Invalid_Result_When_Overload_Does_Not_Have_The_Right_Amount_Of_Arguments()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder().WithName("Overload").Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("No overload of the Overload function takes 0 arguments");
    }

    [Fact]
    public void Validate_Returns_Invalid_Result_When_Overload_ArgumentCount_Is_Registered_Twice()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("Overload")
            .AddArguments(new EmptyArgumentBuilder(), new EmptyArgumentBuilder())
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Function Overload with 2 arguments could not be identified uniquely");
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
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Validate_Returns_Result_From_Validation_When_Overload_ArgumentCount_Is_Correct_But_Argument_Is_Of_Wrong_Type()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("Overload")
            .AddArguments(new ConstantArgumentBuilder().WithValue(13))
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Could not evaluate function Overload, see inner results for more details");
        result.InnerResults.Count.ShouldBe(1);
        result.InnerResults.Single().Status.ShouldBe(ResultStatus.Invalid);
        result.InnerResults.Single().ErrorMessage.ShouldBe("Argument Argument1 is not of type System.String");
    }

    [Fact]
    public void Validate_Returns_Result_From_Validation_When_Overload_ArgumentCount_Is_Correct_And_Argument_Is_Of_Wrong_Type_But_ValidateArgumentTypes_Setting_Is_False()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("Overload")
            .AddArguments(new ConstantArgumentBuilder().WithValue(13))
            .Build();
        var sut = CreateSut();
        var settings = new FunctionEvaluatorSettingsBuilder()
            
            .WithValidateArgumentTypes(false)
            .Build();

        // Act
        var result = sut.Validate(functionCall, settings);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Validate_Returns_Result_From_Validation_When_Overload_ArgumentCount_Is_Correct_But_Argument_Has_Non_Succesful_Result_Constant()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("Overload")
            .AddArguments(new ConstantResultArgumentBuilder().WithResult(Result.Error<object?>("Kaboom")))
            .Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                           .Returns(Result.Success<object?>(13));
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Could not evaluate function Overload, see inner results for more details");
        result.InnerResults.Count.ShouldBe(1);
        result.InnerResults.Single().Status.ShouldBe(ResultStatus.Error);
        result.InnerResults.Single().ErrorMessage.ShouldBe("Kaboom");
    }

    [Fact]
    public void Validate_Returns_Result_From_Validation_When_Overload_ArgumentCount_Is_Correct_But_Argument_Has_Non_Succesful_Result_Delegate()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("Overload")
            .AddArguments(new DelegateResultArgumentBuilder().WithDelegate(() => Result.Error<object?>("Kaboom")).WithValidationDelegate(() => Result.Error<Type>("Kaboom")))
            .Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                           .Returns(Result.Success<object?>(13));
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Could not evaluate function Overload, see inner results for more details");
        result.InnerResults.Count.ShouldBe(1);
        result.InnerResults.Single().Status.ShouldBe(ResultStatus.Error);
        result.InnerResults.Single().ErrorMessage.ShouldBe("Kaboom");
    }

    [Fact]
    public void Validate_Returns_Result_From_Validation_When_Overload_ArgumentCount_Is_Correct_But_Required_Argument_Is_Missing()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("Overload")
            .AddArguments(new ConstantArgumentBuilder().WithValue(null))
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Could not evaluate function Overload, see inner results for more details");
        result.InnerResults.Count.ShouldBe(1);
        result.InnerResults.Single().Status.ShouldBe(ResultStatus.Invalid);
        result.InnerResults.Single().ErrorMessage.ShouldBe("Argument Argument1 is required");
    }

    [Fact]
    public void Validate_Returns_Non_Successful_Result_As_Invalid_From_Validation_When_Overload_ArgumentCount_Is_Correct_And_Registered_Once()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("MyFunction2")
            .AddArguments(new DelegateArgumentBuilder().WithDelegate(() => "some value").WithValidationDelegate(() => typeof(int)))
            .Build();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluator.Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                           .Returns(Result.Success<object?>("some value"));
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Could not evaluate function MyFunction2, see inner results for more details");
        result.InnerResults.Count.ShouldBe(1);
        result.InnerResults.Single().ErrorMessage.ShouldBe("Argument Argument1 is not of type System.String");
    }

    [Fact]
    public void Validate_Returns_Error_Result_When_Function_Could_Not_Be_Found_For_FunctionDescriptor()
    {
        // Arrange
        var functionDescriptorProvider = Substitute.For<IFunctionDescriptorProvider>();
        var functionCallArgumentValidator = Substitute.For<IFunctionCallArgumentValidator>();
        var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
        var functionDescriptor = new FunctionDescriptorBuilder()
            .WithName("MyFunction")
            .WithFunctionType(typeof(string))
            .Build();
        functionDescriptorProvider.GetAll().Returns([functionDescriptor]);
        var sut = new FunctionEvaluator(functionDescriptorProvider, functionCallArgumentValidator, expressionEvaluator, Enumerable.Empty<IFunction>(), Enumerable.Empty<IGenericFunction>());
        var functionCall = new FunctionCallBuilder().WithName("MyFunction").Build();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Arrange
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Could not find function with type name System.String");
    }

    [Fact]
    public void Validate_Returns_Correct_Result_When_Using_Function_Argument()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("Overload")
            .AddArguments(new FunctionArgumentBuilder().WithFunction(new FunctionCallBuilder().WithName("MyTypedFunction")))
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Validate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Use_Different_Type_In_Typed_Function_When_It_Fits()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("StringArgumentFunction")
            .AddArguments(new FunctionArgumentBuilder().WithFunction(new FunctionCallBuilder().WithName("ObjectResultFunction")))
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("My value");
    }

    [Fact]
    public void Can_Cast_Result_To_Specified_Type_In_Function()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("Cast")
            .AddArguments(new ConstantArgumentBuilder().WithValue(13))
            .AddTypeArguments(new ExpressionTypeArgumentBuilder().WithExpression($"typeof({typeof(short).FullName})"))
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe((short)13);
    }

    [Fact]
    public void Can_Cast_Result_Typed_To_Specified_Type_In_Function()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("Cast")
            .AddArguments(new ConstantArgumentBuilder().WithValue(13))
            .AddTypeArguments(new ExpressionTypeArgumentBuilder().WithExpression($"typeof({typeof(short).FullName})"))
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.EvaluateTyped<short>(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe((short)13);
    }

    [Fact]
    public void Invalid_Number_Of_TypeArguments_Returns_Invalid_Result()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("Cast")
            .AddArguments(new ConstantArgumentBuilder().WithValue(13))
            .AddTypeArguments
            (
                new ExpressionTypeArgumentBuilder().WithExpression($"typeof({typeof(short).FullName})"),
                new ExpressionTypeArgumentBuilder().WithExpression($"typeof({typeof(short).FullName})")
            )
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("The type or method has 1 generic parameter(s), but 2 generic argument(s) were provided. A generic argument must be provided for each generic parameter.");
    }

    [Fact]
    public void Error_Result_From_TypeArguments_Are_Returned_Correctly()
    {
        // Arrange
        var functionCall = new FunctionCallBuilder()
            .WithName("Cast")
            .AddArguments(new ConstantArgumentBuilder().WithValue(13))
            .AddTypeArguments(new ConstantResultTypeArgumentBuilder().WithValue(Result.Error<Type>("Kaboom")))
            .Build();
        var sut = CreateSut();

        // Act
        var result = sut.Evaluate(functionCall, CreateSettings());

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Kaboom");
    }

    public void Dispose()
    {
        _scope.Dispose();
        _provider.Dispose();
    }

    private IFunctionEvaluator CreateSut()
        => _scope.ServiceProvider.GetRequiredService<IFunctionEvaluator>();

    private static FunctionEvaluatorSettings CreateSettings()
        => new FunctionEvaluatorSettingsBuilder();

    private sealed class ErrorFunction : IValidatableFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Error<object?>("Kaboom");
        }

        public Result<Type> Validate(FunctionCallContext context)
        {
            return Result.Error<Type>("Kaboom");
        }
    }

    [FunctionName("MyFunction")]
    [FunctionResultType(typeof(string))]
    private sealed class MyFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Success<object?>("function result");
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
            return Result.Success("function result");
        }
    }

    [FunctionName("MyFunction2")]
    [FunctionArgument("Argument1", typeof(string))]
    private sealed class MyFunction2 : IValidatableFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Success<object?>("function result");
        }

        public Result<Type> Validate(FunctionCallContext context)
        {
            return Result.Invalid<Type>("Custom validation message");
        }
    }

    private sealed class PassThroughFunction : IDynamicDescriptorsFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            if (context.FunctionCall.Name != "PassThrough")
            {
                return Result.Continue<object?>();
            }

            return Result.Success<object?>("Custom value");
        }

        public IEnumerable<FunctionDescriptor> GetDescriptors()
        {
            yield return new FunctionDescriptorBuilder().WithName("PassThrough").WithFunctionType(GetType()).Build();
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
    }

    [FunctionName("Optional")]
    [FunctionArgument("Argument", typeof(string), false)]
    private sealed class OptionalParameterTest : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            var argumentResult = context.GetArgumentStringValueResult(0, "Argument", string.Empty);
            return Result.FromExistingResult<object?>(argumentResult);
        }
    }

    [FunctionName("AnimalFunction")]
    [FunctionArgument("Argument", typeof(Animal))]
    private sealed class AnimalFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            throw new NotImplementedException();
        }
    }

    [FunctionName("MyInterfaceFunction")]
    [FunctionArgument("Argument", typeof(IMyInterface))]
    private sealed class MyInterfaceFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            throw new NotImplementedException();
        }
    }

    [FunctionName("ObjectArgumentFunction")]
    [FunctionArgument("Argument", typeof(object))]
    private sealed class ObjectArgumentFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            throw new NotImplementedException();
        }
    }

    [FunctionName("ObjectResultFunction")]
    private sealed class ObjectResultFunction : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Success<object?>("My value");
        }
    }

    [FunctionName("StringArgumentFunction")]
    [FunctionArgument("Argument1", typeof(string))]
    private sealed class StringArgumentFunction : ITypedFunction<string>
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return EvaluateTyped(context).Transform<object?>(x => x);
        }

        public Result<string> EvaluateTyped(FunctionCallContext context)
        {
            return context.GetArgumentStringValueResult(0, "Argument1");
        }
    }

    [FunctionName("Cast")]
    [FunctionArgument("Expression", typeof(object), "Expression to cast")]
    [FunctionTypeArgument("T")]
    private sealed class CastFunction : IGenericFunction
    {
        public Result<object?> EvaluateGeneric<T>(FunctionCallContext context)
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            return new ResultDictionaryBuilder()
                .Add("Expression", () => context.GetArgumentValueResult(0, "Expression"))
                .Build()
                .OnSuccess(results => Result.Success(Convert.ChangeType(results.GetValue("Expression"), typeof(T))));
        }
    }

#pragma warning disable S2094 // Classes should not be empty
    private abstract class Animal { }
#pragma warning restore S2094 // Classes should not be empty
    private sealed class Monkey : Animal { }

    private interface IMyInterface { }
    private sealed class MyImplementation : IMyInterface { }
}
