namespace CrossCutting.Utilities.Parsers.Tests;

public class FunctionDescriptorProviderTests
{
    public class GetAll : FunctionDescriptorProviderTests
    {
        [Fact]
        public void Returns_All_Available_Functions_Correctly()
        {
            // Arrange
            var functions = new IFunction[]
            {
                new MyFunction1(),
                new MyFunction2()
            };
            var genericFunctions = Enumerable.Empty<IGenericFunction>();
            var sut = new FunctionDescriptorProvider(new FunctionDescriptorMapper(), functions, genericFunctions);

            // Act
            var result = sut.GetAll();

            // Assert
            result.Count.ShouldBe(2);
            result.First().Name.ShouldBe(nameof(MyFunction1));
            result.First().Description.ShouldBeEmpty();
            result.Last().Name.ShouldBe("MyCustomFunctionName");
            result.Last().Description.ShouldBe("This is a very cool function");
        }

        [Fact]
        public void Returns_All_Registered_Available_Functions_Correctly()
        {
            // Arrange
            using var provider = new ServiceCollection()
                .AddParsers()
                .AddScoped<IFunction, MyFunction1>()
                .AddScoped<IFunction, MyFunction2>()
                .AddScoped<IFunction, PassThroughFunction>()
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();
            var sut = scope.ServiceProvider.GetRequiredService<IFunctionDescriptorProvider>();

            // Act
            var result = sut.GetAll();

            // Assert
            result.Count.ShouldBe(3);
        }

        [Fact]
        public void Returns_Typed_Function_Correctly()
        {
            // Arrange
            var functions = new IFunction[]
            {
                new MyTypedFunction()
            };
            var genericFunctions = Enumerable.Empty<IGenericFunction>();
            var sut = new FunctionDescriptorProvider(new FunctionDescriptorMapper(), functions, genericFunctions);

            // Act
            var result = sut.GetAll();

            // Assert
            result.Count.ShouldBe(1);
            result.First().Name.ShouldBe("MyTyped"); /// note that the Function suffix is removed, which seems logical for stuff like "ToUpperCaseFunction";
            result.First().ReturnValueType.ShouldBe(typeof(string)); /// because of implementation of ITypedFunction<string>;
        }

        [Fact]
        public void Returns_Generic_Function_Correctly()
        {
            // Arrange
            var functions = Enumerable.Empty<IFunction>();
            var genericFunctions = new IGenericFunction[]
            {
                new MyGenericFunction()
            };
            var sut = new FunctionDescriptorProvider(new FunctionDescriptorMapper(), functions, genericFunctions);

            // Act
            var result = sut.GetAll();

            // Assert
            result.Count.ShouldBe(1);
            result.First().Name.ShouldBe("MyGenericFunction");
            result.First().TypeArguments.Count.ShouldBe(1);
            result.First().TypeArguments.First().Name.ShouldBe("T");
            result.First().TypeArguments.First().Description.ShouldBe("Description of T type argument");
        }

        private sealed class MyFunction1 : IFunction
        {
            public Result<object?> Evaluate(FunctionCallContext context)
                => throw new NotImplementedException();
        }

        [FunctionName("MyCustomFunctionName")]
        [FunctionArgument("Argument1", typeof(string), "Description of argument 1", true)]
        [FunctionArgument("Argument2", typeof(int), "Description of argument 2", false)]
        [Description("This is a very cool function")]
        private sealed class MyFunction2 : IFunction
        {
            public Result<object?> Evaluate(FunctionCallContext context)
                => throw new NotImplementedException();
        }

        private sealed class MyTypedFunction : ITypedFunction<string>
        {
            public Result<object?> Evaluate(FunctionCallContext context)
            {
                throw new NotImplementedException();
            }

            public Result<string> EvaluateTyped(FunctionCallContext context)
            {
                throw new NotImplementedException();
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

        [FunctionName("MyGenericFunction")]
        [FunctionTypeArgument("T", "Description of T type argument")]
        private sealed class MyGenericFunction : IGenericFunction
        {
            public Result<object?> EvaluateGeneric<T>(FunctionCallContext context)
                => throw new NotImplementedException();
        }
    }
}
