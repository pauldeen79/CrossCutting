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
        actual.Should().BeEquivalentTo("a", "b,c", "d");
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
        actual.Should().BeEquivalentTo("a", "b,c", "d");
    }

    [Fact]
    public void Can_SafeSplit_With_Empty_Last_Item()
    {
        // Arrange
        var input = "a,\"b,c\",";

        // Act
        var actual = input.SafeSplit(',', '\"', '\"');

        // Assert
        actual.Should().BeEquivalentTo("a", "b,c", string.Empty);
    }

    [Fact]
    public void Empty_String_Results_In_Empty_Array()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var actual = input.SafeSplit(',', '\"', '\"');

        // Assert
        actual.Should().BeEmpty();
    }
}
