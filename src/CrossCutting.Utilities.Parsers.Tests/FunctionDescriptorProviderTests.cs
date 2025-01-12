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
            var sut = new FunctionDescriptorProvider(functions);

            // Act
            var result = sut.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.First().Name.Should().Be(nameof(MyFunction1));
            result.First().Description.Should().BeEmpty();
            result.Last().Name.Should().Be("MyCustomFunctionName");
            result.Last().Description.Should().Be("This is a very cool function");
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
            result.Should().HaveCount(3);
        }

        private sealed class MyFunction1 : IFunction
        {
            public Result<object?> Evaluate(FunctionCallContext context)
                => throw new NotImplementedException();

            public Result Validate(FunctionCallContext context)
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

            public Result Validate(FunctionCallContext context)
                => throw new NotImplementedException();
        }

        private sealed class PassThroughFunction : IPassThroughFunction
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

            public Result Validate(FunctionCallContext context)
            {
                return Result.Success();
            }
        }
    }
}
