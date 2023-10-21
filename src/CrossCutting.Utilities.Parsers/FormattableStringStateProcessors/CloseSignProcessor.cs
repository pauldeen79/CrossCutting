namespace CrossCutting.Utilities.Parsers.FormattableStringStateProcessors;

public class CloseSignProcessor : IFormattableStringStateProcessor
{
    private readonly IEnumerable<IPlaceholderProcessor> _processors;

    public CloseSignProcessor(IEnumerable<IPlaceholderProcessor> processors)
    {
        _processors = processors;
    }

    public Result<string> Process(FormattableStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Current != FormattableStringParser.CloseSign)
        {
            return Result.Continue<string>();
        }

        if (state.NextPositionIsSign(FormattableStringParser.CloseSign)
            || state.PreviousPositionIsSign(FormattableStringParser.CloseSign))
        {
            if (state.NextPositionIsSign(FormattableStringParser.CloseSign))
            {
                state.Escape();
            }

            return Result.Continue<string>();
        }

        if (!state.InPlaceholder)
        {
            return Result.Invalid<string>("Missing open sign '{'. To use the '}' character, you have to escape it with an additional '}' character");
        }

        var placeholderResult = _processors
            .OrderBy(x => x.Order)
            .Select(x => x.Process(state.PlaceholderBuilder.ToString(), state.FormatProvider, state.Context, state.Parser))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid<string>($"Unknown placeholder in value: {state.PlaceholderBuilder}");

        if (!placeholderResult.IsSuccessful())
        {
            return placeholderResult;
        }

        state.ClosePlaceholder(placeholderResult.Value!);

        return Result.NoContent<string>();
    }
}
