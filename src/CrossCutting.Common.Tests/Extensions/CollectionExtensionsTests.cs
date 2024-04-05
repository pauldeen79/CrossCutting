namespace CrossCutting.Common.Tests.Extensions;

public class CollectionExtensionsTests
{
    [Fact]
    public void Can_Use_AddRange_On_GenericCollection()
    {
        // Arrange
        var input = new Collection<string>();

        // Act
        input.AddRange(["a", "b", "c"]);

        // Assert
        input.Should().BeEquivalentTo("a", "b", "c");
    }
}
