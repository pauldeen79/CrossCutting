﻿namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class FunctionExpressionComponentTests : TestBase
{
    protected IFunction Function => Mocks.GetOrCreate<IFunction>(ClassFactory);
    protected IMemberCallArgumentValidator MemberCallArgumentValidator => Mocks.GetOrCreate<IMemberCallArgumentValidator>(ClassFactory);

    protected FunctionExpressionComponent CreateSut(IFunction? function = null)
        => new FunctionExpressionComponent(new FunctionParser(), new MemberResolver(MemberDescriptorProvider, MemberCallArgumentValidator, [function ?? Function]));

    protected FunctionExpressionComponent CreateSut(IGenericFunction genericFunction)
        => new FunctionExpressionComponent(new FunctionParser(), new MemberResolver(MemberDescriptorProvider, MemberCallArgumentValidator, [genericFunction]));

    public class EvaluateAsync : FunctionExpressionComponentTests
    {
        // Manually mocked this because I can't get NSubstitute working with generic method...
        private sealed class MyGenericFunction : IGenericFunction
        {
            public Task<Result<object?>> EvaluateGenericAsync<T>(FunctionCallContext context, CancellationToken token)
                => Task.FromResult(Result.Success<object?>("function result value"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("no function")]
        [InlineData("object.ToString()")]
        public async Task Returns_Continue_When_Expression_Is_Not_A_FunctionCall(string expression)
        {
            // Arrange
            var context = CreateContext(expression);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Correct_Result_On_FunctionCall_Without_Arguments_That_Could_Be_Resolved()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            Function
                .EvaluateAsync(Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success<object?>("function result value"));
            MemberCallArgumentValidator
                .ValidateAsync(Arg.Any<MemberDescriptor>(), Arg.Any<IMember>(), Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
                .Returns(x => new MemberCallArgumentValidator().ValidateAsync(x.ArgAt<MemberDescriptor>(0), x.ArgAt<IMember>(1), x.ArgAt<FunctionCallContext>(2), x.ArgAt<CancellationToken>(3)));
            MemberDescriptorProvider.GetAll().Returns(Result.Success<IReadOnlyCollection<MemberDescriptor>>([new MemberDescriptorBuilder()
                .WithName("MyFunction")
                .WithMemberType(MemberType.Function)
                .WithImplementationType(Function.GetType())
                .WithReturnValueType(typeof(string))]));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("function result value");
        }

        [Fact]
        public async Task Returns_Correct_Result_On_Generic_FunctionCall_Without_Arguments_That_Could_Be_Resolved()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String>()");
            var genericFunction = new MyGenericFunction();
            MemberDescriptorProvider
                .GetAll()
                .Returns(Result.Success<IReadOnlyCollection<MemberDescriptor>>([new MemberDescriptorBuilder()
                    .WithName("MyFunction")
                    .WithMemberType(MemberType.GenericFunction)
                    .WithImplementationType(genericFunction.GetType())
                    .WithReturnValueType(typeof(string))]));
            MemberCallArgumentValidator
                .ValidateAsync(Arg.Any<MemberDescriptor>(), Arg.Any<IMember>(), Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
                .Returns(x => new MemberCallArgumentValidator().ValidateAsync(x.ArgAt<MemberDescriptor>(0), x.ArgAt<IMember>(1), x.ArgAt<FunctionCallContext>(2), x.ArgAt<CancellationToken>(3)));
            var sut = CreateSut(genericFunction);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("function result value");
        }

        [Fact]
        public async Task Returns_NotFound_On_FunctionCall_Without_Arguments_That_Could_Be_Resolved()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            MemberDescriptorProvider
                .GetAll()
                .Returns(Result.Success<IReadOnlyCollection<MemberDescriptor>>(new List<MemberDescriptor>()));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.ErrorMessage.ShouldBe("Unknown function: MyFunction");
        }

        [Fact]
        public async Task Returns_NotFound_When_ArgumentCount_Is_Incorrect()
        {
            // Arrange
            var context = CreateContext("MyFunction(context)");
            Function.EvaluateAsync(Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success<object?>("function result value"));
            MemberDescriptorProvider.GetAll().Returns(Result.Success<IReadOnlyCollection<MemberDescriptor>>(
                [
                    new MemberDescriptorBuilder().WithName("MyFunction").WithImplementationType(Function.GetType()).WithMemberType(MemberType.Function).WithReturnValueType(typeof(string)),
                    new MemberDescriptorBuilder().WithName("MyFunction").WithImplementationType(Function.GetType()).WithMemberType(MemberType.Function).WithReturnValueType(typeof(int)),
                ]));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.ErrorMessage.ShouldBe("No overload of the MyFunction function takes 1 arguments");
        }

        [Fact]
        public async Task Returns_NotFound_When_Function_Is_Registered_Twice_With_Same_ArgumentCount()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            Function.EvaluateAsync(Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success<object?>("function result value"));
            MemberDescriptorProvider.GetAll().Returns(Result.Success<IReadOnlyCollection<MemberDescriptor>>(
                [
                    new MemberDescriptorBuilder().WithName("MyFunction").WithMemberType(MemberType.Function).WithImplementationType(Function.GetType()).WithReturnValueType(typeof(string)),
                    new MemberDescriptorBuilder().WithName("MyFunction").WithMemberType(MemberType.Function).WithImplementationType(Function.GetType()).WithReturnValueType(typeof(int)),
                ]));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.ErrorMessage.ShouldBe("Function MyFunction with 0 arguments could not be identified uniquely");
        }

        [Fact]
        public async Task Returns_NotFound_When_Function_Could_Not_Be_Found_From_Descriptor()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            Function.EvaluateAsync(Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success<object?>("function result value"));
            MemberDescriptorProvider.GetAll().Returns(Result.Success<IReadOnlyCollection<MemberDescriptor>>([new MemberDescriptorBuilder()
                .WithName("MyFunction")
                .WithMemberType(MemberType.Function)
                .WithImplementationType(GetType())
                .WithReturnValueType(typeof(string))]));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.ErrorMessage.ShouldBe("Could not find member with type name CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents.FunctionExpressionComponentTests+EvaluateAsync");
        }

        [Fact]
        public async Task Returns_Invalid_On_Generic_FunctionCall_With_Wrong_Number_Of_Type_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String,System.String>()");
            var genericFunction = new MyGenericFunction();
            MemberDescriptorProvider
                .GetAll()
                .Returns(Result.Success<IReadOnlyCollection<MemberDescriptor>>([new MemberDescriptorBuilder()
                    .WithName("MyFunction")
                    .WithMemberType(MemberType.GenericFunction)
                    .WithImplementationType(genericFunction.GetType())
                    .WithReturnValueType(typeof(string))]));
            MemberCallArgumentValidator
                .ValidateAsync(Arg.Any<MemberDescriptor>(), Arg.Any<IMember>(), Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
                .Returns(x => new MemberCallArgumentValidator().ValidateAsync(x.ArgAt<MemberDescriptor>(0), x.ArgAt<IMember>(1), x.ArgAt<FunctionCallContext>(2), x.ArgAt<CancellationToken>(3)));
            var sut = CreateSut(genericFunction);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("The type or method has 1 generic parameter(s), but 2 generic argument(s) were provided. A generic argument must be provided for each generic parameter.");
        }

        [Fact]
        public async Task Returns_Invalid_On_Argument_Validation_Errors()
        {
            // Arrange
            var context = CreateContext("MyFunction(1)");
            Function
                .EvaluateAsync(Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success<object?>("function result value"));
            MemberDescriptorProvider
                .GetAll()
                .Returns(Result.Success<IReadOnlyCollection<MemberDescriptor>>([new MemberDescriptorBuilder()
                    .WithName("MyFunction")
                    .WithMemberType(MemberType.Function)
                    .WithImplementationType(Function.GetType())
                    .WithReturnValueType(typeof(string))
                    .AddArguments(new MemberDescriptorArgumentBuilder().WithName("MyArgument").WithType(typeof(string)))]));
            MemberCallArgumentValidator
                .ValidateAsync(Arg.Any<MemberDescriptor>(), Arg.Any<IMember>(), Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
                .Returns(x => new MemberCallArgumentValidator().ValidateAsync(x.ArgAt<MemberDescriptor>(0), x.ArgAt<IMember>(1), x.ArgAt<FunctionCallContext>(2), x.ArgAt<CancellationToken>(3)));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Validation of member MyFunction failed, see validation errors for more details");
            result.ValidationErrors.Count.ShouldBe(1);
            result.ValidationErrors.First().ErrorMessage.ShouldBe("Argument MyArgument is not of type System.String");
            result.ValidationErrors.First().MemberNames.Count.ShouldBe(1);
            result.ValidationErrors.First().MemberNames.First().ShouldBe("MyArgument");
        }

        [Fact]
        public async Task Returns_Non_Successful_Result_From_Parsing_When_Parsing_Is_Not_Successful()
        {
            // Arrange
            var context = CreateContext("MyFunction())");
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Input has additional characters after last close bracket");
        }
    }

    public class ParseAsync : FunctionExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Continue_When_Expression_Is_Not_A_FunctionCall()
        {
            // Arrange
            var context = CreateContext("no function");
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            result.ExpressionComponentType.ShouldBe(typeof(FunctionExpressionComponent));
            result.SourceExpression.ShouldBe("no function");
        }

        [Fact]
        public async Task Returns_Correct_Result_On_FunctionCall_Without_Arguments_That_Could_Be_Resolved()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            MemberDescriptorProvider
                .GetAll()
                .Returns(Result.Success<IReadOnlyCollection<MemberDescriptor>>([new MemberDescriptorBuilder()
                    .WithName("MyFunction")
                    .WithMemberType(MemberType.Function)
                    .WithImplementationType(Function.GetType())
                    .WithReturnValueType(typeof(string))]));
            MemberCallArgumentValidator
                .ValidateAsync(Arg.Any<MemberDescriptor>(), Arg.Any<IMember>(), Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
                .Returns(x => new MemberCallArgumentValidator().ValidateAsync(x.ArgAt<MemberDescriptor>(0), x.ArgAt<IMember>(1), x.ArgAt<FunctionCallContext>(2), x.ArgAt<CancellationToken>(3)));
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(typeof(FunctionExpressionComponent));
            result.ResultType.ShouldBe(typeof(string));
            result.SourceExpression.ShouldBe("MyFunction()");
        }

        [Fact]
        public async Task Returns_NotFound_On_FunctionCall_Without_Arguments_That_Could_Be_Resolved()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            MemberDescriptorProvider
                .GetAll()
                .Returns(Result.Success<IReadOnlyCollection<MemberDescriptor>>(new List<MemberDescriptor>()));
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.ExpressionComponentType.ShouldBe(typeof(FunctionExpressionComponent));
            result.ResultType.ShouldBeNull();
            result.SourceExpression.ShouldBe("MyFunction()");
            result.ErrorMessage.ShouldBe("Unknown function: MyFunction");
        }

        [Fact]
        public async Task Returns_Non_Successful_Result_From_Parsing_When_Parsing_Is_Not_Successful()
        {
            // Arrange
            var context = CreateContext("MyFunction())");
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ExpressionComponentType.ShouldBe(typeof(FunctionExpressionComponent));
            result.ResultType.ShouldBeNull();
            result.ErrorMessage.ShouldBe("Input has additional characters after last close bracket");
        }
    }
}
