namespace CrossCutting.Utilities.Parsers.Tests;

public class FlatTextParserTests
{
    [Fact]
    public void Can_Parse_FlatText_Without_TextQualifier()
    {
        // Arrange
        var input = "Value A\tValue B\tValue C";

        // Act
        var actual = FlatTextParser.Parse(input, '\t');

        // Assert
        actual.Should().BeEquivalentTo("Value A", "Value B", "Value C");
    }

    [Fact]
    public void Can_Parse_FlatText_With_TextQualifier()
    {
        // Arrange
        var input = "\"Value A\" \"Value B\" \"Value C\"";

        // Act
        var actual = FlatTextParser.Parse(input, ' ', '"');

        // Assert
        actual.Should().BeEquivalentTo("Value A", "Value B", "Value C");
    }

    [Fact]
    public void Can_Parse_Same_Text_As_SafeSplit()
    {
        // Arrange
        var input = "a,\"b,c\",";

        // Act
        var actual = FlatTextParser.Parse(input, ',', '"');

        // Assert
        actual.Should().BeEquivalentTo("a", "b,c", string.Empty);
    }


    [Fact]
    public void Empty_String_Results_In_Empty_Array()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var actual = FlatTextParser.Parse(input, ',', '"');

        // Assert
        actual.Should().BeEmpty();
    }
}
