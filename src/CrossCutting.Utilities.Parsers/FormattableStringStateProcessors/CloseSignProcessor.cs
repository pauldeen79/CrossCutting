namespace CrossCutting.Utilities.Parsers.FormattableStringStateProcessors;

public class CloseSignProcessor : IFormattableStringStateProcessor
{
    private readonly IPlaceholderProcessor _processor;

    public CloseSignProcessor(IPlaceholderProcessor processor)
    {
        _processor = processor;
    }

    public Result<string> Process(FormattableStringParserState state)
    {
        if (state.Current != FormattableStringParser.CloseSign)
        {
            return Result<string>.NotSupported();
        }

        if (state.NextPositionIsSign(FormattableStringParser.CloseSign)
            || state.PreviousPositionIsSign(FormattableStringParser.CloseSign))
        {
            if (state.NextPositionIsSign(FormattableStringParser.CloseSign))
            {
                state.Escape();
            }

            return Result<string>.Continue();
        }

        if (!state.InPlaceholder)
        {
            return Result<string>.Invalid("Missing open sign '{'. To use the '}' character, you have to escape it with an additional '}' character");
        }

        var placeholderResult = _processor.Process(state.PlaceholderBuilder.ToString(), state.FormatProvider, state.Context);
        if (!placeholderResult.IsSuccessful())
        {
            return placeholderResult;
        }

        state.ClosePlaceholder(placeholderResult.Value!);

        return Result<string>.NoContent();
    }
}
