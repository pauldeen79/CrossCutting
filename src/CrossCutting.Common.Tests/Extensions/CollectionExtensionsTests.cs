namespace CrossCutting.Common.Tests.Extensions;

public class CollectionExtensionsTests
{
    [Fact]
    public void Can_Use_AddRange_On_GenericCollection()
    {
        // Arrange
        var input = new Collection<string>();

        // Act
        input.AddRange(new[] { "a", "b", "c" });

        // Assert
        input.Should().BeEquivalentTo(new[] { "a", "b", "c" });
    }
}
