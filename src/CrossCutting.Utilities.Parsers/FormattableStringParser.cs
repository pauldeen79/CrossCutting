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

        if (string.IsNullOrEmpty(input))
        {
            return Result.Success(new FormattableStringParserResult(input ?? string.Empty, []));
        }

        // Handle escaped markers (e.g., {{ -> {)
        var escapedStart = settings.PlaceholderStart + settings.PlaceholderStart;
        var escapedEnd = settings.PlaceholderEnd + settings.PlaceholderEnd;

        var remainder = input;
        remainder = remainder.Replace(escapedStart, "\uE000") // Temporarily replace escaped start marker
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
            var placeholderResult = _processors
                .OrderBy(x => x.Order)
                .Select(x => x.Process(placeholder, settings.FormatProvider, context, this))
                .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                    ?? Result.Invalid<FormattableStringParserResult>($"Unknown placeholder in value: {placeholder}");

            if (!placeholderResult.IsSuccessful())
            {
                return placeholderResult;
            }

            if (placeholderResult.Value?.Format == "{0}"
                && placeholderResult.Value.ArgumentCount == 1
                && placeholderResult.Value.GetArgument(0) is string placeholderResultValue
                && placeholderResultValue.Contains(settings.PlaceholderStart)
                && placeholderResultValue.Contains(settings.PlaceholderEnd)
                && placeholderResultValue != input) //compare with input to prevent infinitive loop
            {
                placeholderResult = Parse(placeholderResultValue, settings, context);
            }

            results.Add(placeholderResult);
        } while (remainder.IndexOf(settings.PlaceholderStart) > -1 || remainder.IndexOf(settings.PlaceholderEnd) > -1);

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

        for (var i = 0; i < results.Count; i++)
        {
            remainder = remainder.Replace($"{TemporaryDelimiter}{i}{TemporaryDelimiter}", $"{{{i}}}");
        }

        return Result.Success(new FormattableStringParserResult(remainder, [.. results.Select(x => x.Value?.ToString(settings.FormatProvider))]));
    }
}
