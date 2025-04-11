namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class FunctionExpressionTests : TestBase
{
    protected IFunction Function { get; }
    public IFunctionDescriptorProvider FunctionDescriptorProvider { get; }
    public IFunctionCallArgumentValidator FunctionCallArgumentValidator { get; }

    protected FunctionExpression CreateSut(IFunction? function = null)
        => new FunctionExpression(new FunctionParser(), new FunctionResolver(FunctionDescriptorProvider, FunctionCallArgumentValidator, [function ?? Function], Enumerable.Empty<IGenericFunction>()));

    protected FunctionExpression CreateSut(IGenericFunction genericFunction)
        => new FunctionExpression(new FunctionParser(), new FunctionResolver(FunctionDescriptorProvider, FunctionCallArgumentValidator, Enumerable.Empty<IFunction>(), [genericFunction]));

    public FunctionExpressionTests()
    {
        Function = Substitute.For<IFunction>();
        FunctionDescriptorProvider = Substitute.For<IFunctionDescriptorProvider>();
        FunctionCallArgumentValidator = Substitute.For<IFunctionCallArgumentValidator>();
    }

    public class Evaluate : FunctionExpressionTests
    {
        // Manually mocked this because I can't get NSubstitute working with generic method...
        private sealed class MyGenericFunction : IGenericFunction
        {
            public Result<object?> EvaluateGeneric<T>(FunctionCallContext context)
                => Result.Success<object?>("function result value");
        }

        [Fact]
        public void Returns_Continue_When_Expression_Is_Not_A_FunctionCall()
        {
            // Arrange
            var context = CreateContext("no function");
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_Without_Arguments_That_Could_Be_Resolved()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            Function.Evaluate(Arg.Any<FunctionCallContext>()).Returns(Result.Success<object?>("function result value"));
            FunctionDescriptorProvider.GetAll().Returns([new FunctionDescriptorBuilder().WithName("MyFunction").WithFunctionType(Function.GetType()).WithReturnValueType(typeof(string)).Build()]);
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("function result value");
        }

        [Fact]
        public void Returns_Correct_Result_On_Generic_FunctionCall_Without_Arguments_That_Could_Be_Resolved()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String>()");
            var genericFunction = new MyGenericFunction();
            FunctionDescriptorProvider.GetAll().Returns([new FunctionDescriptorBuilder().WithName("MyFunction").WithFunctionType(genericFunction.GetType()).WithReturnValueType(typeof(string)).Build()]);
            var sut = CreateSut(genericFunction);

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("function result value");
        }

        [Fact]
        public void Returns_NotFound_On_FunctionCall_Without_Arguments_That_Could_Be_Resolved()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.ErrorMessage.ShouldBe("Unknown function: MyFunction");
        }

        [Fact]
        public void Returns_NotFound_When_ArgumentCount_Is_Incorrect()
        {
            // Arrange
            var context = CreateContext("MyFunction(context)");
            Function.Evaluate(Arg.Any<FunctionCallContext>()).Returns(Result.Success<object?>("function result value"));
            FunctionDescriptorProvider.GetAll().Returns(
                [
                    new FunctionDescriptorBuilder().WithName("MyFunction").WithFunctionType(Function.GetType()).WithReturnValueType(typeof(string)).Build(),
                    new FunctionDescriptorBuilder().WithName("MyFunction").WithFunctionType(Function.GetType()).WithReturnValueType(typeof(int)).Build()
                ]);
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.ErrorMessage.ShouldBe("No overload of the MyFunction function takes 1 arguments");
        }

        [Fact]
        public void Returns_NotFound_When_Function_Is_Registered_Twice_With_Same_ArgumentCount()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            Function.Evaluate(Arg.Any<FunctionCallContext>()).Returns(Result.Success<object?>("function result value"));
            FunctionDescriptorProvider.GetAll().Returns(
                [
                    new FunctionDescriptorBuilder().WithName("MyFunction").WithFunctionType(Function.GetType()).WithReturnValueType(typeof(string)).Build(),
                    new FunctionDescriptorBuilder().WithName("MyFunction").WithFunctionType(Function.GetType()).WithReturnValueType(typeof(int)).Build()
                ]);
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.ErrorMessage.ShouldBe("Function MyFunction with 0 arguments could not be identified uniquely");
        }

        [Fact]
        public void Returns_NotFound_When_Function_Could_Not_Be_Found_From_Descriptor()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            Function.Evaluate(Arg.Any<FunctionCallContext>()).Returns(Result.Success<object?>("function result value"));
            FunctionDescriptorProvider.GetAll().Returns([new FunctionDescriptorBuilder().WithName("MyFunction").WithFunctionType(GetType()).WithReturnValueType(typeof(string)).Build()]);
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.ErrorMessage.ShouldBe("Could not find function with type name CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions.FunctionExpressionTests+Evaluate");
        }

        [Fact]
        public void Returns_Invalid_On_Generic_FunctionCall_With_Wrong_Number_Of_Type_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String,System.String>()");
            var genericFunction = new MyGenericFunction();
            FunctionDescriptorProvider.GetAll().Returns([new FunctionDescriptorBuilder().WithName("MyFunction").WithFunctionType(genericFunction.GetType()).WithReturnValueType(typeof(string)).Build()]);
            var sut = CreateSut(genericFunction);

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("The type or method has 1 generic parameter(s), but 2 generic argument(s) were provided. A generic argument must be provided for each generic parameter.");
        }

        [Fact]
        public void Returns_Invalid_On_Argument_Validation_Errors()
        {
            // Arrange
            var context = CreateContext("MyFunction(1)");
            Function
                .Evaluate(Arg.Any<FunctionCallContext>())
                .Returns(Result.Success<object?>("function result value"));
            FunctionDescriptorProvider
                .GetAll()
                .Returns([new FunctionDescriptorBuilder()
                    .WithName("MyFunction")
                    .WithFunctionType(Function.GetType())
                    .WithReturnValueType(typeof(string))
                    .AddArguments(new FunctionDescriptorArgumentBuilder().WithName("MyArgument").WithType(typeof(string)))
                    .Build()]);
            FunctionCallArgumentValidator
                .Validate(Arg.Any<FunctionDescriptorArgument>(), Arg.Any<string>(), Arg.Any<FunctionCallContext>())
                .Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Invalid).WithErrorMessage("I want a string, you give me an int!"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Validation of function MyFunction failed, see validation errors for more details");
            result.ValidationErrors.Count.ShouldBe(1);
            result.ValidationErrors.First().ErrorMessage.ShouldBe("I want a string, you give me an int!");
            result.ValidationErrors.First().MemberNames.Count.ShouldBe(1);
            result.ValidationErrors.First().MemberNames.First().ShouldBe("MyArgument");
        }

        [Fact]
        public void Returns_Non_Successful_Result_From_Parsing_When_Parsing_Is_Not_Successful()
        {
            // Arrange
            var context = CreateContext("MyFunction())");
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Input has additional characters after last close bracket");
        }
    }

    public class Parse : FunctionExpressionTests
    {
        [Fact]
        public void Returns_Continue_When_Expression_Is_Not_A_FunctionCall()
        {
            // Arrange
            var context = CreateContext("no function");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            result.ExpressionType.ShouldBe(typeof(FunctionExpression));
            result.SourceExpression.ShouldBe("no function");
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_Without_Arguments_That_Could_Be_Resolved()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            FunctionDescriptorProvider.GetAll().Returns([new FunctionDescriptorBuilder().WithName("MyFunction").WithFunctionType(Function.GetType()).WithReturnValueType(typeof(string)).Build()]);
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionType.ShouldBe(typeof(FunctionExpression));
            result.ResultType.ShouldBe(typeof(string));
            result.SourceExpression.ShouldBe("MyFunction()");
        }

        [Fact]
        public void Returns_NotFound_On_FunctionCall_Without_Arguments_That_Could_Be_Resolved()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.ExpressionType.ShouldBe(typeof(FunctionExpression));
            result.ResultType.ShouldBeNull();
            result.SourceExpression.ShouldBe("MyFunction()");
            result.ErrorMessage.ShouldBe("Unknown function: MyFunction");
        }

        [Fact]
        public void Returns_Non_Successful_Result_From_Parsing_When_Parsing_Is_Not_Successful()
        {
            // Arrange
            var context = CreateContext("MyFunction())");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ExpressionType.ShouldBe(typeof(FunctionExpression));
            result.ResultType.ShouldBeNull();
            result.ErrorMessage.ShouldBe("Input has additional characters after last close bracket");
        }
    }
}
