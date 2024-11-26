namespace CrossCutting.Utilities.Parsers;

public class FormattableStringParser : IFormattableStringParser
{
    private readonly IEnumerable<IPlaceholderProcessor> _processors;

    public FormattableStringParser(IEnumerable<IPlaceholderProcessor> processors)
    {
        processors = processors.IsNotNull(nameof(processors));

        _processors = processors;
    }

    public Result<FormattableStringParserResult> Parse(string input, FormattableStringParserSettings settings, object? context)
    {
        input = input.IsNotNull(nameof(input));
        settings = settings.IsNotNull(nameof(settings));

        if (string.IsNullOrEmpty(input))
        {
            return Result.Success(new FormattableStringParserResult(input, []));
        }

        // Handle escaped markers (e.g., {{ -> {)
        var escapedStart = settings.PlaceholderStart + settings.PlaceholderStart;
        var escapedEnd = settings.PlaceholderEnd + settings.PlaceholderEnd;

        input = input.Replace(escapedStart, "\uE000") // Temporarily replace escaped start marker
                     .Replace(escapedEnd, "\uE001");  // Temporarily replace escaped end marker

        // Build a regex to match placeholders
        var placeholderPattern = $"{settings.PlaceholderStart}(.*?){settings.PlaceholderEnd}";
        var regex = new Regex(placeholderPattern);

        // Perform replacement
        var results = new List<Result<FormattableStringParserResult>>();
        var result = regex.Replace(input, match =>
        {
            var placeholder = match.Groups[1].Value; // Extract placeholder name
            var state = new FormattableStringParserState(placeholder, settings, context, this);

            var placeholderResult = _processors
                .OrderBy(x => x.Order)
                .Select(x => x.Process(placeholder, state.Settings.FormatProvider, state.Context, state.Parser))
                .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                    ?? Result.Invalid<FormattableStringParserResult>($"Unknown placeholder in value: {placeholder}");
            results.Add(placeholderResult);

            return placeholderResult.Value?.Format ?? string.Empty;
        });

        var err = results.Select(x => new { Item = x, IsSuccessful = x.IsSuccessful() }).FirstOrDefault(x => !x.IsSuccessful);
        if (err is not null)
        {
            return err.Item;
        }

        // Restore escaped markers
        result = result.Replace("\uE000", settings.PlaceholderStart)
                       .Replace("\uE001", settings.PlaceholderEnd);

        return Result.Success(new FormattableStringParserResult(result, [.. results.Select(x => x.Value!)]));
    }
}
