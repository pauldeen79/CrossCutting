namespace CrossCutting.Utilities.Parsers.Tests;

public class ParseResultTests
{
    [Fact]
    public void Error_With_Only_ErrorMessage_Works()
    {
        // Act
        var result = ParseResult.Error<string, string>("Kaboom");

        // Assert
        result.ErrorMessages.Should().BeEquivalentTo("Kaboom");
        result.Values.Should().BeEmpty();
    }
}
