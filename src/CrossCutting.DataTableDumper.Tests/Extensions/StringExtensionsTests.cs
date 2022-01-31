namespace CrossCutting.DataTableDumper.Tests.Extensions;

public class StringExtensionsTests
{
    [Theory,
        InlineData(null, null),
        InlineData("", ""),
        InlineData("value without pipe", "value without pipe"),
        InlineData("value with |", "value with _")]
    public void EscapePipes_Returns_Correct_Value(string input, string expectedOutput)
    {
        // Act
        var actual = input.EscapePipes();

        // Assert
        actual.Should().Be(expectedOutput);
    }

    [Theory,
        InlineData(null, null),
        InlineData("", ""),
        InlineData("value without pipe", "value without pipe"),
        InlineData("value with _", "value with |")]
    public void UnescapePipes_Returns_Correct_Value(string input, string expectedOutput)
    {
        // Act
        var actual = input.UnescapePipes();

        // Assert
        actual.Should().Be(expectedOutput);
    }
}
