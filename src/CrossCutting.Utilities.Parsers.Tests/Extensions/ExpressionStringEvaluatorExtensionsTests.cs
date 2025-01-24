namespace CrossCutting.Utilities.Parsers.Tests.Extensions;

public class ExpressionStringEvaluatorExtensionsTests
{
    private IExpressionStringEvaluator SutMock { get; } = Substitute.For<IExpressionStringEvaluator>();
    private IFormattableStringParser FormattableStringParserMock { get; } = Substitute.For<IFormattableStringParser>();
    private const string Input = "Some input";
    private object Context { get; } = new object();
    private IFormatProvider FormatProvider { get; } = CultureInfo.InvariantCulture;

    [Fact]
    public void Evaluate_Without_Context_And_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Evaluate(Input, FormatProvider);

        // Assert
        SutMock.Received().Evaluate(Input, FormatProvider, null, null);
    }

    [Fact]
    public void Evaluate_Without_Context_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Evaluate(Input, FormatProvider, FormattableStringParserMock);

        // Assert
        SutMock.Received().Evaluate(Input, FormatProvider, null, FormattableStringParserMock);
    }

    [Fact]
    public void Evaluate_Without_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Evaluate(Input, FormatProvider, Context);

        // Assert
        SutMock.Received().Evaluate(Input, FormatProvider, Context, null);
    }

    [Fact]
    public void Validate_Without_Context_And_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Validate(Input, FormatProvider);

        // Assert
        SutMock.Received().Validate(Input, FormatProvider, null, null);
    }

    [Fact]
    public void Validate_Without_Context_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Validate(Input, FormatProvider, FormattableStringParserMock);

        // Assert
        SutMock.Received().Validate(Input, FormatProvider, null, FormattableStringParserMock);
    }

    [Fact]
    public void Validate_Without_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Validate(Input, FormatProvider, Context);

        // Assert
        SutMock.Received().Validate(Input, FormatProvider, Context, null);
    }
}
