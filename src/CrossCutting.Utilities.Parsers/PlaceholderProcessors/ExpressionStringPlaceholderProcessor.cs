namespace CrossCutting.Utilities.Parsers.PlaceholderProcessors;

public class ExpressionStringPlaceholderProcessor : IPlaceholderProcessor
{
    private readonly IExpressionStringParser _expressionStringParser;

    public ExpressionStringPlaceholderProcessor(IExpressionStringParser expressionStringParser)
    {
        ArgumentGuard.IsNotNull(expressionStringParser, nameof(expressionStringParser));

        _expressionStringParser = expressionStringParser;
    }

    public int Order => 990;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        var result = _expressionStringParser.Parse($"={value}", formatProvider, context, formattableStringParser);

        if (result.Status == ResultStatus.NotFound)
        {
            return Result.Continue<string>();
        }

        return Result.FromExistingResult(
            result,
            value => value.ToString(formatProvider, string.Empty));
    }
}
