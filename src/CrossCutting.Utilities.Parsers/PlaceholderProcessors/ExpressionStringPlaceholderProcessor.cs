namespace CrossCutting.Utilities.Parsers.PlaceholderProcessors;

public class ExpressionStringPlaceholderProcessor : IPlaceholder
{
    private readonly IExpressionStringParser _expressionStringParser;

    public ExpressionStringPlaceholderProcessor(IExpressionStringParser expressionStringParser)
    {
        ArgumentGuard.IsNotNull(expressionStringParser, nameof(expressionStringParser));

        _expressionStringParser = expressionStringParser;
    }

    public int Order => 990;

    public Result<FormattableStringParserResult> Evaluate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        var result = _expressionStringParser.Parse($"={value}", formatProvider, context, formattableStringParser);

        return result.Status switch
        {
            ResultStatus.NotFound => Result.Continue<FormattableStringParserResult>(),
            _ => Result.FromExistingResult(result, value => new FormattableStringParserResult(value))
        };
    }

    public Result Validate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        var result = _expressionStringParser.Validate($"={value}", formatProvider, context, formattableStringParser);

        return result.Status switch
        {
            ResultStatus.NotFound => Result.Continue(),
            _ => result
        };
    }
}
