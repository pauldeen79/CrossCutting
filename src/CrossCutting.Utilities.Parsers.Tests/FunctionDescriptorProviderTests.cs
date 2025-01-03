namespace CrossCutting.Utilities.Parsers.Tests;

public class FunctionDescriptorProviderTests
{
    public class GetAll : FunctionDescriptorProviderTests
    {
        [Fact]
        public void Returns_All_Available_Functions_Correctly()
        {
            // Arrange
            var functionDescriptors = new[]
            {
                new FunctionDescriptorBuilder().WithName("Function1").Build(),
                new FunctionDescriptorBuilder().WithName("Function2").Build()
            };
            var sut = new FunctionDescriptorProvider(functionDescriptors);

            // Act
            var result = sut.GetAll();

            // Assert
            result.Should().BeEquivalentTo(functionDescriptors);
        }

        [Fact]
        public void Returns_All_Registered_Available_Functions_Correctly()
        {
            // Arrange
            var functionDescriptor = new FunctionDescriptorBuilder().WithName("MyFunction").Build();
            using var provider = new ServiceCollection()
                .AddParsers()
                .AddSingleton(functionDescriptor)
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();
            var sut = scope.ServiceProvider.GetRequiredService<IFunctionDescriptorProvider>();

            // Act
            var result = sut.GetAll();

            // Assert
            result.Should().ContainSingle();
            result.Single().Should().BeEquivalentTo(functionDescriptor);
        }
    }
}
