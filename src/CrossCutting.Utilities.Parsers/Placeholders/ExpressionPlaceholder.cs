namespace CrossCutting.Utilities.Parsers.Placeholders;

public class ExpressionPlaceholder : IPlaceholder
{
    private readonly IExpressionEvaluator _expressionEvaluator;

    public ExpressionPlaceholder(IExpressionEvaluator expressionEvaluator)
    {
        ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        _expressionEvaluator = expressionEvaluator;
    }

    public Result<GenericFormattableString> Evaluate(string value, PlaceholderSettings settings, object? context, IFormattableStringParser formattableStringParser)
    {
        settings = ArgumentGuard.IsNotNull(settings, nameof(settings));

        var result = _expressionEvaluator.Evaluate(value, settings.FormatProvider, context);

        return result.Status switch
        {
            ResultStatus.NotSupported => Result.Continue<GenericFormattableString>(),
            _ => Result.FromExistingResult(result, value => new GenericFormattableString(value))
        };
    }

    public Result Validate(string value, PlaceholderSettings settings, object? context, IFormattableStringParser formattableStringParser)
    {
        settings = ArgumentGuard.IsNotNull(settings, nameof(settings));

        var result = _expressionEvaluator.Validate(value, settings.FormatProvider, context);

        if (result.Status == ResultStatus.Invalid && result.ErrorMessage?.StartsWith("Unknown expression type found in fragment:") == true)
        {
            return Result.Continue();
        }

        return result;
    }
}
