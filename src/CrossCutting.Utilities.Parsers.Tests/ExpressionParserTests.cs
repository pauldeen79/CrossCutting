namespace CrossCutting.Utilities.Parsers.Tests;

public class ExpressionParserTests
{
    [Fact]
    public void Parse_Parses_true_Correctly()
    {
        // Arrange
        var input = "true";

        // Act
        var result = new ExpressionParser().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(true);
    }

    [Fact]
    public void Parse_Parses_false_Correctly()
    {
        // Arrange
        var input = "false";

        // Act
        var result = new ExpressionParser().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Parses_decimal_Correctly()
    {
        // Arrange
        var input = "1.5";

        // Act
        var result = new ExpressionParser().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(1.5M);
    }

    [Fact]
    public void Parse_Parses_int_Correctly()
    {
        // Arrange
        var input = "2";

        // Act
        var result = new ExpressionParser().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(2);
    }

    [Fact]
    public void Parse_Parses_long_Correctly()
    {
        // Arrange
        var input = "3147483647";

        // Act
        var result = new ExpressionParser().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(3147483647L);
    }

    [Fact]
    public void Parse_Parses_string_Correctly()
    {
        // Arrange
        var input = "\"Hello world!\"";

        // Act
        var result = new ExpressionParser().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo("Hello world!");
    }

    [Fact]
    public void Parse_Parses_DateTime_Correctly()
    {
        // Arrange
        var input = "01/02/2019";

        // Act
        var result = new ExpressionParser().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(new DateTime(2019, 1, 2));
    }

}
