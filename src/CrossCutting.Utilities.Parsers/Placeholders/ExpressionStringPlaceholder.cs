namespace CrossCutting.Utilities.Parsers.Placeholders;

public class ExpressionStringPlaceholder : IPlaceholder
{
    private readonly IExpressionStringEvaluator _expressionStringEvaluator;

    public ExpressionStringPlaceholder(IExpressionStringEvaluator expressionStringEvaluator)
    {
        ArgumentGuard.IsNotNull(expressionStringEvaluator, nameof(expressionStringEvaluator));

        _expressionStringEvaluator = expressionStringEvaluator;
    }

    public Result<GenericFormattableString> Evaluate(string value, PlaceholderSettings settings, object? context, IFormattableStringParser formattableStringParser)
    {
        settings = ArgumentGuard.IsNotNull(settings, nameof(settings));

        var result = _expressionStringEvaluator.Evaluate($"={value}", new ExpressionStringEvaluatorSettings(settings.FormatProvider, settings.ValidateArgumentTypes), context, formattableStringParser);

        return result.Status switch
        {
            ResultStatus.NotFound => Result.Continue<GenericFormattableString>(),
            _ => Result.FromExistingResult(result, value => new GenericFormattableString(value))
        };
    }

    public Result Validate(string value, PlaceholderSettings settings, object? context, IFormattableStringParser formattableStringParser)
    {
        settings = ArgumentGuard.IsNotNull(settings, nameof(settings));

        var result = _expressionStringEvaluator.Validate($"={value}", new ExpressionStringEvaluatorSettings(settings.FormatProvider, settings.ValidateArgumentTypes), context, formattableStringParser);

        return result.Status switch
        {
            ResultStatus.NotFound => Result.Continue(),
            _ => result
        };
    }
}
