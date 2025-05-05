namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class MemberDescriptorProviderTests
{
    public class GetAll : MemberDescriptorProviderTests
    {
        [Fact]
        public void Returns_All_Available_Functions_Correctly()
        {
            // Arrange
            var members = new IMember[]
            {
                new MyFunction1(),
                new MyFunction2()
            };
            var sut = new MemberDescriptorProvider(new MemberDescriptorMapper(), members);

            // Act
            var result = sut.GetAll();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(2);
            result.Value.First().Name.ShouldBe(nameof(MyFunction1));
            result.Value.First().Description.ShouldBeEmpty();
            result.Value.Last().Name.ShouldBe("MyCustomFunctionName");
            result.Value.Last().Description.ShouldBe("This is a very cool function");
        }

        [Fact]
        public void Returns_All_Registered_Available_Functions_Correctly()
        {
            // Arrange
            using var provider = new ServiceCollection()
                .AddExpressionEvaluator()
                .AddSingleton<IFunction, MyFunction1>()
                .AddSingleton<IFunction, MyFunction2>()
                .AddSingleton<IFunction, PassThroughFunction>()
                .BuildServiceProvider(true);
            var sut = provider.GetRequiredService<IMemberDescriptorProvider>();

            // Act
            var result = sut.GetAll();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBeGreaterThanOrEqualTo(3);
        }

        [Fact]
        public void Returns_Typed_Function_Correctly()
        {
            // Arrange
            var members = new IMember[]
            {
                new MyTypedFunction()
            };
            var sut = new MemberDescriptorProvider(new MemberDescriptorMapper(), members);

            // Act
            var result = sut.GetAll();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(1);
            result.Value.First().Name.ShouldBe("MyTyped"); /// note that the Function suffix is removed, which seems logical for stuff like "ToUpperCaseFunction";
            result.Value.First().ReturnValueType.ShouldBe(typeof(string)); /// because of implementation of ITypedFunction<string>;
        }

        [Fact]
        public void Returns_Generic_Function_Correctly()
        {
            // Arrange
            var members = new IMember[]
            {
                new MyGenericFunction()
            };
            var sut = new MemberDescriptorProvider(new MemberDescriptorMapper(), members);

            // Act
            var result = sut.GetAll();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(1);
            result.Value.First().Name.ShouldBe("MyGenericFunction");
            result.Value.First().TypeArguments.Count.ShouldBe(1);
            result.Value.First().TypeArguments.First().Name.ShouldBe("T");
            result.Value.First().TypeArguments.First().Description.ShouldBe("Description of T type argument");
            result.Value.First().ReturnValueType.ShouldBe(typeof(string));
            result.Value.First().Results.Count.ShouldBe(1);
            result.Value.First().Results.First().Status.ShouldBe(ResultStatus.Ok);
            result.Value.First().Results.First().Description.ShouldBe("Success");
        }

        [Fact]
        public void Returns_Non_Successful_Result_Correctly()
        {
            // Arrange
            var members = new IMember[]
            {
                new MyFunction1(),
                new MyFunction2()
            };
            var mapper = Substitute.For<IMemberDescriptorMapper>();
            mapper
                .Map(Arg.Any<IMember>(), Arg.Any<Type?>())
                .Returns(Result.Error<IReadOnlyCollection<MemberDescriptor>>("Kaboom"));
            var sut = new MemberDescriptorProvider(mapper, members);

            // Act
            var result = sut.GetAll();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        private sealed class MyFunction1 : IFunction
        {
            public Result<object?> Evaluate(FunctionCallContext context)
                => throw new NotImplementedException();
        }

        [MemberName("MyCustomFunctionName")]
        [MemberArgument("Argument1", typeof(string), "Description of argument 1", true)]
        [MemberArgument("Argument2", typeof(int), "Description of argument 2", false)]
        [Description("This is a very cool function")]
        private sealed class MyFunction2 : IFunction
        {
            public Result<object?> Evaluate(FunctionCallContext context)
                => throw new NotImplementedException();
        }

        [MemberResultType(typeof(string))]
        private sealed class MyTypedFunction : IFunction
        {
            public Result<object?> Evaluate(FunctionCallContext context)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class PassThroughFunction : IFunction, IDynamicDescriptorsProvider
        {
            public Result<object?> Evaluate(FunctionCallContext context)
            {
                if (context.FunctionCall.Name != "PassThrough")
                {
                    return Result.Continue<object?>();
                }

                return Result.Success<object?>("Custom value");
            }

            public Result<IReadOnlyCollection<MemberDescriptor>> GetDescriptors()
            {
                return Result.Success<IReadOnlyCollection<MemberDescriptor>>(new List<MemberDescriptor>([new MemberDescriptorBuilder()
                    .WithName("PassThrough")
                    .WithMemberType(MemberType.Function)
                    .WithImplementationType(GetType())]));
            }
        }

        [MemberName("MyGenericFunction")]
        [MemberTypeArgument("T", "Description of T type argument")]
        [MemberResult(ResultStatus.Ok, "Success")]
        [MemberResultType(typeof(string))]
        private sealed class MyGenericFunction : IGenericFunction
        {
            public Result<object?> EvaluateGeneric<T>(FunctionCallContext context)
                => throw new NotImplementedException();
        }
    }
}
