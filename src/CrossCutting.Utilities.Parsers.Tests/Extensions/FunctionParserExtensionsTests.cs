namespace CrossCutting.Utilities.Parsers.Tests.Extensions;

public class FunctionParserExtensionsTests
{
    private IFunctionParser SutMock { get; } = Substitute.For<IFunctionParser>();
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
        SutMock.Received().Parse(Input, Arg.Is<FunctionParserSettings>(x => x.FormatProvider == FormatProvider && x.FormattableStringParser == null), Arg.Any<object?>());
    }

    [Fact]
    public void Parse_Without_Context_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Parse(Input, FormatProvider, FormattableStringParserMock);

        // Assert
        SutMock.Received().Parse(Input, Arg.Is<FunctionParserSettings>(x => x.FormatProvider == FormatProvider && x.FormattableStringParser == FormattableStringParserMock), Arg.Any<object?>());
    }

    [Fact]
    public void Parse_Without_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Parse(Input, FormatProvider, Context);

        // Assert
        SutMock.Received().Parse(Input, Arg.Is<FunctionParserSettings>(x => x.FormatProvider == FormatProvider && x.FormattableStringParser ==  null), Context);
    }
}
