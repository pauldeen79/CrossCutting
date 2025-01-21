namespace CrossCutting.Utilities.Parsers.Placeholders;

public class ExpressionStringPlaceholder : IPlaceholder
{
    private readonly IExpressionStringEvaluator _expressionStringEvaluator;

    public ExpressionStringPlaceholder(IExpressionStringEvaluator expressionStringEvaluator)
    {
        ArgumentGuard.IsNotNull(expressionStringEvaluator, nameof(expressionStringEvaluator));

        _expressionStringEvaluator = expressionStringEvaluator;
    }

    public Result<GenericFormattableString> Evaluate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        var result = _expressionStringEvaluator.Evaluate($"={value}", formatProvider, context, formattableStringParser);

        return result.Status switch
        {
            ResultStatus.NotFound => Result.Continue<GenericFormattableString>(),
            _ => Result.FromExistingResult(result, value => new GenericFormattableString(value))
        };
    }

    public Result Validate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        var result = _expressionStringEvaluator.Validate($"={value}", formatProvider, context, formattableStringParser);

        return result.Status switch
        {
            ResultStatus.NotFound => Result.Continue(),
            _ => result
        };
    }
}
