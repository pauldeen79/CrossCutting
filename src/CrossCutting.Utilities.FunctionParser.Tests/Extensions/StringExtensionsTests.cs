namespace CrossCutting.Utilities.FunctionParser.Tests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void Can_SafeSplit_StringValue()
    {
        // Arrange
        var input = "a,'b,c',d";

        // Act
        var actual = input.SafeSplit(',', '\'', '\'');

        // Assert
        actual.Should().HaveCount(3);
        actual.ElementAt(0).Should().Be("a");
        actual.ElementAt(1).Should().Be("b,c");
        actual.ElementAt(2).Should().Be("d");
    }
}
