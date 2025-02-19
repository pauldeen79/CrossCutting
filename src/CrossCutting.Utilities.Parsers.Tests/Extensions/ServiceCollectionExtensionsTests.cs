namespace CrossCutting.Utilities.Parsers.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddParsers_Registers_All_Types_Without_Errors()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act & Assert
        Action a = () => _ = serviceCollection.AddParsers().BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
        a.ShouldNotThrow();
    }
}
