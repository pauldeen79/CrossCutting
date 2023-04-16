namespace CrossCutting.Utilities.FunctionParser.Tests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void Can_SafeSplit_StringValue_With_Equal_Open_And_Close_Token()
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

    [Fact]
    public void Can_SafeSplit_StringValue_With_Different_Open_And_Close_Token()
    {
        // Arrange
        var input = "a,[b,c],d";

        // Act
        var actual = input.SafeSplit(',', '[', ']');

        // Assert
        actual.Should().HaveCount(3);
        actual.ElementAt(0).Should().Be("a");
        actual.ElementAt(1).Should().Be("b,c");
        actual.ElementAt(2).Should().Be("d");
    }

    [Fact]
    public void Can_SafeSplit_With_Empty_Last_Item()
    {
        // Arrange
        var input = "a,\"b,c\",";

        // Act
        var actual = input.SafeSplit(',', '\"', '\"');

        // Assert
        actual.Should().HaveCount(3);
        actual.ElementAt(0).Should().Be("a");
        actual.ElementAt(1).Should().Be("b,c");
        actual.ElementAt(2).Should().BeEmpty();
    }
}
