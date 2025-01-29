namespace CrossCutting.Utilities.Parsers.Tests.Extensions;

public class ExpressionStringEvaluatorExtensionsTests
{
    private IExpressionStringEvaluator SutMock { get; } = Substitute.For<IExpressionStringEvaluator>();
    private IFormattableStringParser FormattableStringParserMock { get; } = Substitute.For<IFormattableStringParser>();
    private const string Input = "Some input";
    private object Context { get; } = new object();
    private ExpressionStringEvaluatorSettings Settings { get; } = new ExpressionStringEvaluatorSettingsBuilder().WithFormatProvider(CultureInfo.InvariantCulture).Build();

    [Fact]
    public void Evaluate_Without_Context_And_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Evaluate(Input, Settings);

        // Assert
        SutMock.Received().Evaluate(Input, Settings, null, null);
    }

    [Fact]
    public void Evaluate_Without_Context_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Evaluate(Input, Settings, FormattableStringParserMock);

        // Assert
        SutMock.Received().Evaluate(Input, Settings, null, FormattableStringParserMock);
    }

    [Fact]
    public void Evaluate_Without_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Evaluate(Input, Settings, Context);

        // Assert
        SutMock.Received().Evaluate(Input, Settings, Context, null);
    }

    [Fact]
    public void Validate_Without_Context_And_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Validate(Input, Settings);

        // Assert
        SutMock.Received().Validate(Input, Settings, null, null);
    }

    [Fact]
    public void Validate_Without_Context_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Validate(Input, Settings, FormattableStringParserMock);

        // Assert
        SutMock.Received().Validate(Input, Settings, null, FormattableStringParserMock);
    }

    [Fact]
    public void Validate_Without_FormattableStringParser_Gets_Processed_Correctly()
    {
        // Act
        _ = SutMock.Validate(Input, Settings, Context);

        // Assert
        SutMock.Received().Validate(Input, Settings, Context, null);
    }
}
