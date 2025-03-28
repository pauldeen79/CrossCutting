namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class FunctionExpressionTests : TestBase
{
    protected IFunction Function { get; }
    public IFunctionDescriptorProvider FunctionDescriptorProvider { get; }
    public IFunctionCallArgumentValidator FunctionCallArgumentValidator { get; }

    protected FunctionExpression CreateSut(IFunction? function = null)
        => new FunctionExpression(FunctionDescriptorProvider, FunctionCallArgumentValidator, [function ?? Function], Enumerable.Empty<IGenericFunction>());

    protected FunctionExpression CreateSut(IGenericFunction genericFunction)
        => new FunctionExpression(FunctionDescriptorProvider, FunctionCallArgumentValidator, Enumerable.Empty<IFunction>(), [genericFunction]);

    public FunctionExpressionTests()
    {
        Function = Substitute.For<IFunction>();
        FunctionDescriptorProvider = Substitute.For<IFunctionDescriptorProvider>();
        FunctionCallArgumentValidator = Substitute.For<IFunctionCallArgumentValidator>();
    }

    public class Evaluate : FunctionExpressionTests
    {
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

    public class ParseFunctionCall : FunctionExpressionTests
    {
        [Fact]
        public void Returns_NotFound_When_Expression_Is_Not_A_FunctionCall()
        {
            // Arrange
            var context = CreateContext("no function");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.Value.ShouldBeNull();
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_Without_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction()");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBeEmpty();
            result.Value.Arguments.ShouldBeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction(argument1, argument2, argument3)");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBeEmpty();
            result.Value.Arguments.ShouldBe(["argument1", "argument2", "argument3"]);
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Generics_Without_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String>()");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBe([typeof(string)]);
            result.Value.Arguments.ShouldBeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Generics_And_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String>(argument1, argument2, argument3)");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBe([typeof(string)]);
            result.Value.Arguments.ShouldBe(["argument1", "argument2", "argument3"]);
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Nested_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction(argument1, MySubFunction(argument2), argument3)");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBeEmpty();
            result.Value.Arguments.ShouldBe(["argument1", "MySubFunction(argument2)", "argument3"]);
        }

        [Fact]
        public void Returns_Invalid_On_FunctionName_With_WhiteSpace()
        {
            // Arrange
            var context = CreateContext("My Function()");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Function name may not contain whitespace");
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Spaces_Tabs_And_NewLines()
        {
            // Arrange
            var context = CreateContext("       MyFunction(\r\n\r\n\r\n   \t\t)\t\t\t");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBeEmpty();
            result.Value.Arguments.ShouldBeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_With_Quotes()
        {
            // Arrange
            var context = CreateContext("MyFunction(argument1, \"argument2, argument3\", argument4)");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBeEmpty();
            result.Value.Arguments.ShouldBe(["argument1", "\"argument2, argument3\"", "argument4"]);
        }

        [Fact]
        public void Returns_Invalid_When_Too_Many_Close_Brackets_Were_Found()
        {
            // Arrange
            var context = CreateContext("MyFunction())");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Input has additional characters after last close bracket");
        }

        [Fact]
        public void Returns_Invalid_When_Generic_Type_Is_Unknown()
        {
            // Arrange
            var context = CreateContext("MyFunction<unknowntype>()");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown type: unknowntype");
        }
    }
}
