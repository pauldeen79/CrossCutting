namespace CrossCutting.Utilities.Parsers.Tests.Extensions;

public class ExpressionStringParserExtensionsTests
{
    private Mock<IExpressionStringParser> SutMock { get; } = new();
    private Mock<IFormattableStringParser> FormattableStringParserMock { get; } = new();
    private const string Input = "Some input";
    private object Context { get; } = new object();
    private IFormatProvider FormatProvider { get; } = CultureInfo.InvariantCulture;
    
    [Fact]
    public void Parse_Without_Context_And_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Object.Parse(Input, FormatProvider);

        // Assert
        SutMock.Verify(x => x.Parse(Input, FormatProvider, null, null), Times.Once);
    }

    [Fact]
    public void Parse_Without_Context_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Object.Parse(Input, FormatProvider, FormattableStringParserMock.Object);

        // Assert
        SutMock.Verify(x => x.Parse(Input, FormatProvider, null, FormattableStringParserMock.Object), Times.Once);
    }

    [Fact]
    public void Parse_Without_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Object.Parse(Input, FormatProvider, Context);

        // Assert
        SutMock.Verify(x => x.Parse(Input, FormatProvider, Context, null), Times.Once);
    }
}
