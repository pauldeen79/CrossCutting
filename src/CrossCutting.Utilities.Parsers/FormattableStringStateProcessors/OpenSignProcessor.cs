namespace CrossCutting.Utilities.Parsers.FormattableStringStateProcessors;

public class OpenSignProcessor : IFormattableStringStateProcessor
{
    public Result<string> Process(FormattableStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Current != FormattableStringParser.OpenSign)
        {
            return Result<string>.NotSupported();
        }

        if (state.NextPositionIsSign(FormattableStringParser.OpenSign)
            || state.PreviousPositionIsSign(FormattableStringParser.OpenSign))
        {
            if (state.NextPositionIsSign(FormattableStringParser.OpenSign))
            {
                state.Escape();
            }

            return Result<string>.Continue();
        }

        if (state.InPlaceholder)
        {
            return Result<string>.Invalid("Recursive placeholder detected, this is not supported");
        }

        state.StartPlaceholder();

        return Result<string>.NoContent();
    }
}
