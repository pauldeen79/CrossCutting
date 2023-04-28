namespace CrossCutting.Utilities.Parsers.Tests;

public sealed class ExpressionParserTests : IDisposable
{
    private readonly ServiceProvider _provider;

    public ExpressionParserTests() => _provider = new ServiceCollection().AddParsers().BuildServiceProvider();

    [Fact]
    public void Parse_Parses_true_Correctly()
    {
        // Arrange
        var input = "true";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(false);
    }

    [Fact]
    public void Parse_Parses_null_Correctly()
    {
        // Arrange
        var input = "null";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeNull();
    }

    [Fact]
    public void Parse_Parses_context_Correctly()
    {
        // Arrange
        var input = "context";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture, "context value");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo("context value");
    }

    [Fact]
    public void Parse_Parses_decimal_Correctly()
    {
        // Arrange
        var input = "1.5";

        // Act
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

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
        var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(new DateTime(2019, 1, 2));
    }

    public void Dispose() => _provider.Dispose();

    private IExpressionParser CreateSut() => _provider.GetRequiredService<IExpressionParser>();
}
