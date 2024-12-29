namespace CrossCutting.Utilities.Parsers;

public class FormattableStringParser : IFormattableStringParser
{
    private readonly IEnumerable<IPlaceholderProcessor> _processors;

    private const string TemporaryDelimiter = "\uE002";

    public FormattableStringParser(IEnumerable<IPlaceholderProcessor> processors)
    {
        processors = processors.IsNotNull(nameof(processors));

        _processors = processors;
    }

    public Result<FormattableStringParserResult> Parse(string input, FormattableStringParserSettings settings, object? context)
    {
        settings = settings.IsNotNull(nameof(settings));

        return ParseRecursive(input, settings, context, false, 0);
    }

    public Result Validate(string input, FormattableStringParserSettings settings, object? context)
    {
        settings = settings.IsNotNull(nameof(settings));

        return ParseRecursive(input, settings, context, true, 0);
    }

    private Result<FormattableStringParserResult> ParseRecursive(string input, FormattableStringParserSettings settings, object? context, bool validateOnly, int currentRecursionLevel)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Result.Success(new FormattableStringParserResult(string.Empty, []));
        }

        // Handle escaped markers (e.g., {{ -> {)
        var escapedStart = settings.PlaceholderStart + settings.PlaceholderStart;
        var escapedEnd = settings.PlaceholderEnd + settings.PlaceholderEnd;

        var remainder = input.Replace(escapedStart, "\uE000") // Temporarily replace escaped start marker
                             .Replace(escapedEnd, "\uE001");  // Temporarily replace escaped end marker

        var results = new List<Result<FormattableStringParserResult>>();
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
                return Result.Invalid<FormattableStringParserResult>($"PlaceholderStart sign '{settings.PlaceholderStart}' is missing");
            }

            var placeholder = remainder.Substring(openIndex + settings.PlaceholderStart.Length, closeIndex - openIndex - settings.PlaceholderStart.Length);
            var found = $"{settings.PlaceholderStart}{placeholder}{settings.PlaceholderEnd}";
            remainder = remainder.Replace(found, $"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}");
            var placeholderResult = PerformAction(settings, context, validateOnly, placeholder);

            placeholderResult = CombineResults(placeholderResult, validateOnly, () => ProcessRecurse(input, settings, context, placeholderResult, validateOnly, currentRecursionLevel + 1));
            if (!placeholderResult.IsSuccessful() && !validateOnly)
            {
                return placeholderResult;
            }

            results.Add(placeholderResult);
        } while (NeedToRepeat(settings, remainder));

        if (settings.EscapeBraces)
        {
            // Fix FormatException when using ToString on FormattableStringParserResult
            remainder = remainder.Replace("{", "{{")
                                 .Replace("}", "}}");
        }

        // Restore escaped markers
        // First, fix invalid format string {bla} to {{bla}} when escaped, else the ToString operation on FormattableStringParserResult fails
        var start = settings.PlaceholderStart.StartsWith("{") ? settings.PlaceholderStart + settings.PlaceholderStart : settings.PlaceholderStart;
        var end = settings.PlaceholderEnd.StartsWith("}") ? settings.PlaceholderEnd + settings.PlaceholderEnd : settings.PlaceholderEnd;
        remainder = remainder.Replace("\uE000", start)
                             .Replace("\uE001", end);

        remainder = ReplaceTemporaryDelimiters(remainder, results);

        if (validateOnly)
        {
            return Result.Aggregate(results, Result.Success(new FormattableStringParserResult(string.Empty, [])), validationResults => Result.Invalid<FormattableStringParserResult>("Validation failed, see inner results for details", validationResults));
        }

        return Result.Success(new FormattableStringParserResult(remainder, [.. results.Select(x => x.Value?.ToString(settings.FormatProvider))]));
    }

    private Result<FormattableStringParserResult> PerformAction(FormattableStringParserSettings settings, object? context, bool validateOnly, string placeholder)
        => _processors
            .OrderBy(x => x.Order)
            .Select(processor => validateOnly
                ? processor.Validate(placeholder, settings.FormatProvider, context, this)
                : processor.Process(placeholder, settings.FormatProvider, context, this))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid<FormattableStringParserResult>($"Unknown placeholder in value: {placeholder}");

    private Result<FormattableStringParserResult> ProcessRecurse(string input, FormattableStringParserSettings settings, object? context, Result<FormattableStringParserResult> placeholderResult, bool validateOnly, int currentRecursionLevel)
    {
        if (placeholderResult.Value?.Format == "{0}"
            && placeholderResult.Value.ArgumentCount == 1
            && placeholderResult.Value.GetArgument(0) is string placeholderResultValue
            && NeedRecurse(placeholderResultValue, settings, input)) //compare with input to prevent infinitive loop
        {
            if (currentRecursionLevel >= settings.MaximumRecursion)
            {
                return Result.Error<FormattableStringParserResult>($"Maximum of {settings.MaximumRecursion} recursions is reached");
            }

            placeholderResult = ParseRecursive(placeholderResultValue, settings, context, validateOnly, currentRecursionLevel);
        }

        return placeholderResult;
    }

    private static string ReplaceTemporaryDelimiters(string remainder, List<Result<FormattableStringParserResult>> results)
    {
        for (var i = 0; i < results.Count; i++)
        {
            remainder = remainder.Replace($"{TemporaryDelimiter}{i}{TemporaryDelimiter}", $"{{{i}}}");
        }

        return remainder;
    }

    private static Result<FormattableStringParserResult> CombineResults(Result<FormattableStringParserResult> placeholderResult, bool validateOnly, Func<Result<FormattableStringParserResult>> dlg)
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

    private static bool NeedRecurse(string placeholderResultValue, FormattableStringParserSettings settings, string input)
        => placeholderResultValue.Contains(settings.PlaceholderStart)
            && placeholderResultValue.Contains(settings.PlaceholderEnd)
            && placeholderResultValue != input;
}
