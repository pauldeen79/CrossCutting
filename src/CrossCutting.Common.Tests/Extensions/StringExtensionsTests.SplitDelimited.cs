namespace CrossCutting.Utilities.Parsers.Tests;

public partial class StringExtensionsTests
{
    [Fact]
    public void Can_Split_String_Without_TextQualifier()
    {
        // Arrange
        var input = "Value A\tValue B\tValue C";

        // Act
        var actual = input.SplitDelimited('\t');

        // Assert
        actual.Should().BeEquivalentTo("Value A", "Value B", "Value C");
    }

    [Fact]
    public void Can_Split_String_With_TextQualifier()
    {
        // Arrange
        var input = "\"Value A\" \"Value B\" \"Value C\"";

        // Act
        var actual = input.SplitDelimited(' ', '"');

        // Assert
        actual.Should().BeEquivalentTo("Value A", "Value B", "Value C");
    }

    [Fact]
    public void Can_Split_String_With_TextQualifier_But_Not_Present()
    {
        // Arrange
        var input = "Value A Value B Value C";

        // Act
        var actual = input.SplitDelimited(' ', '"');

        // Assert
        actual.Should().BeEquivalentTo("Value", "A", "Value", "B", "Value", "C");
    }

    [Fact]
    public void Can_Split_Same_Text_As_SafeSplit()
    {
        // Arrange
        var input = "a,\"b,c\",";

        // Act
        var actual = input.SplitDelimited(',', '"');

        // Assert
        actual.Should().BeEquivalentTo("a", "b,c", string.Empty);
    }

    [Fact]
    public void Empty_String_Results_In_Empty_Array()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var actual = input.SplitDelimited(',');

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void Can_Split_String_Without_TextQualifier_Using_Delimiter_At_The_End()
    {
        // Arrange
        var input = "Value A,Value B,Value C,";

        // Act
        var actual = input.SplitDelimited(',');

        // Assert
        actual.Should().BeEquivalentTo("Value A", "Value B", "Value C", string.Empty);
    }

    [Fact]
    public void Can_Split_String_Where_Delimiter_Is_Within_TextQualifiers()
    {
        // Arrange
        var input = "\"A+B+C\"";

        // Act
        var actual = input.SplitDelimited('+', '"');

        // Assert
        actual.Should().BeEquivalentTo("A+B+C");
    }
}
