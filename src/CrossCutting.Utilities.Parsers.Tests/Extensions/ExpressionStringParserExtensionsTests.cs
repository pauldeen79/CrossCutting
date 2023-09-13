namespace CrossCutting.Utilities.Parsers.Tests.Extensions;

public class ExpressionStringParserExtensionsTests
{
    private IExpressionStringParser SutMock { get; } = Substitute.For<IExpressionStringParser>();
    private IFormattableStringParser FormattableStringParserMock { get; } = Substitute.For<IFormattableStringParser>();
    private const string Input = "Some input";
    private object Context { get; } = new object();
    private IFormatProvider FormatProvider { get; } = CultureInfo.InvariantCulture;
    
    [Fact]
    public void Parse_Without_Context_And_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Parse(Input, FormatProvider);

        // Assert
        SutMock.Received().Parse(Input, FormatProvider, null, null);
    }

    [Fact]
    public void Parse_Without_Context_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Parse(Input, FormatProvider, FormattableStringParserMock);

        // Assert
        SutMock.Received().Parse(Input, FormatProvider, null, FormattableStringParserMock);
    }

    [Fact]
    public void Parse_Without_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Parse(Input, FormatProvider, Context);

        // Assert
        SutMock.Received().Parse(Input, FormatProvider, Context, null);
    }
}
