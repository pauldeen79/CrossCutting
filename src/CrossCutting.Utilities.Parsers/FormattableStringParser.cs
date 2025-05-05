namespace CrossCutting.Utilities.Parsers;

public class FormattableStringParser : IFormattableStringParser
{
    private readonly IEnumerable<IPlaceholder> _placeholders;

    private const string TemporaryDelimiter = "\uE002";

    public FormattableStringParser(IEnumerable<IPlaceholder> placeholders)
    {
        placeholders = placeholders.IsNotNull(nameof(placeholders));

        _placeholders = placeholders;
    }

    public Result<GenericFormattableString> Parse(string format, FormattableStringParserSettings settings, object? context)
    {
        settings = settings.IsNotNull(nameof(settings));

        return ProcessRecursive(format, settings, context, false, 0);
    }

    public Result Validate(string format, FormattableStringParserSettings settings, object? context)
    {
        settings = settings.IsNotNull(nameof(settings));

        return ProcessRecursive(format, settings, context, true, 0);
    }

    private Result<GenericFormattableString> ProcessRecursive(string format, FormattableStringParserSettings settings, object? context, bool validateOnly, int currentRecursionLevel)
    {
        if (string.IsNullOrEmpty(format))
        {
            return Result.Success(new GenericFormattableString(string.Empty, []));
        }

        // Handle escaped markers (e.g., {{ -> {)
        var escapedStart = settings.PlaceholderStart + settings.PlaceholderStart;
        var escapedEnd = settings.PlaceholderEnd + settings.PlaceholderEnd;

        var remainder = format.Replace(escapedStart, "\uE000") // Temporarily replace escaped start marker
                              .Replace(escapedEnd, "\uE001");  // Temporarily replace escaped end marker

        var results = new List<Result<GenericFormattableString>>();
        do
        {
            var closeIndex = remainder.LastIndexOf(settings.PlaceholderEnd);
            if (closeIndex == -1)
            {
                break;
            }

            var openIndex = remainder.LastIndexOf(settings.PlaceholderStart);
            if (openIndex == -1)
            {
                return Result.Invalid<GenericFormattableString>($"PlaceholderStart sign '{settings.PlaceholderStart}' is missing");
            }

            var placeholder = remainder.Substring(openIndex + settings.PlaceholderStart.Length, closeIndex - openIndex - settings.PlaceholderStart.Length);
            var found = $"{settings.PlaceholderStart}{placeholder}{settings.PlaceholderEnd}";
            remainder = remainder.Replace(found, $"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}");
            var placeholderResult = ProcessOnPlaceholders(settings, context, validateOnly, placeholder);

            placeholderResult = CombineResults(placeholderResult, validateOnly, () => ProcessRecursive(format, settings, context, placeholderResult, validateOnly, currentRecursionLevel + 1));
            if (!placeholderResult.IsSuccessful() && !validateOnly)
            {
                return placeholderResult;
            }

            results.Add(placeholderResult);
        } while (NeedToRepeat(settings, remainder));

        if (settings.EscapeBraces)
        {
            // Fix FormatException when using ToString on FormattableString
            remainder = remainder.Replace("{", "{{")
                                 .Replace("}", "}}");
        }

        // Restore escaped markers
        // First, fix invalid format string {bla} to {{bla}} when escaped, else the ToString operation on FormattableString fails
        var start = settings.PlaceholderStart.StartsWith("{")
            ? settings.PlaceholderStart + settings.PlaceholderStart
            : settings.PlaceholderStart;
        var end = settings.PlaceholderEnd.StartsWith("}")
            ? settings.PlaceholderEnd + settings.PlaceholderEnd
            : settings.PlaceholderEnd;

        remainder = remainder.Replace("\uE000", start)
                             .Replace("\uE001", end);

        remainder = ReplaceTemporaryDelimiters(remainder, results);

        return validateOnly
            ? Result.Aggregate(results, Result.Success(new GenericFormattableString(string.Empty, [])), validationResults => Result.Invalid<GenericFormattableString>("Validation failed, see inner results for details", validationResults))
            : Result.Success(new GenericFormattableString(remainder, [.. results.Where(x => x.Value is not null).Select(x => x.Value!.ToString(settings.FormatProvider))]));
    }

    private Result<GenericFormattableString> ProcessRecursive(string format, FormattableStringParserSettings settings, object? context, Result<GenericFormattableString> placeholderResult, bool validateOnly, int currentRecursionLevel)
    {
        if (placeholderResult.Value?.Format == "{0}"
            && placeholderResult.Value.ArgumentCount == 1
            && placeholderResult.Value.GetArgument(0) is string placeholderResultValue
            && NeedRecurse(format, settings, placeholderResultValue)) //compare with input to prevent infinitive loop
        {
            if (currentRecursionLevel >= settings.MaximumRecursion)
            {
                return Result.Error<GenericFormattableString>($"Maximum of {settings.MaximumRecursion} recursions is reached");
            }

            placeholderResult = ProcessRecursive(placeholderResultValue, settings, context, validateOnly, currentRecursionLevel);
        }

        return placeholderResult;
    }

    private Result<GenericFormattableString> ProcessOnPlaceholders(FormattableStringParserSettings settings, object? context, bool validateOnly, string value)
        => _placeholders
            .Select(placeholder => validateOnly
                ? Result.FromExistingResult<GenericFormattableString>(placeholder.Validate(value, new PlaceholderSettings(settings.FormatProvider, settings.ValidateArgumentTypes), context, this))
                : placeholder.Evaluate(value, new PlaceholderSettings(settings.FormatProvider, settings.ValidateArgumentTypes), context, this))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid<GenericFormattableString>($"Unknown placeholder in value: {value}");

    private static string ReplaceTemporaryDelimiters(string remainder, List<Result<GenericFormattableString>> results)
    {
        for (var i = 0; i < results.Count; i++)
        {
            remainder = remainder.Replace($"{TemporaryDelimiter}{i}{TemporaryDelimiter}", $"{{{i}}}");
        }

        return remainder;
    }

    private static Result<GenericFormattableString> CombineResults(Result<GenericFormattableString> placeholderResult, bool validateOnly, Func<Result<GenericFormattableString>> dlg)
    {
        if (!placeholderResult.IsSuccessful() && !validateOnly)
        {
            return placeholderResult;
        }

        return dlg();
    }

    private static bool NeedToRepeat(FormattableStringParserSettings settings, string remainder)
        => remainder.IndexOf(settings.PlaceholderStart) > -1
        || remainder.IndexOf(settings.PlaceholderEnd) > -1;

    private static bool NeedRecurse(string format, FormattableStringParserSettings settings, string placeholderResultValue)
        => placeholderResultValue.Contains(settings.PlaceholderStart)
        && placeholderResultValue.Contains(settings.PlaceholderEnd)
        && placeholderResultValue != format;
}
