namespace CrossCutting.Utilities.Parsers.FormattableStringStateProcessors;

public class OpenSignProcessor : IFormattableStringStateProcessor
{
    public Result<string> Process(FormattableStringParserState state)
    {
        if (state.Current != FormattableStringParser.OpenSign)
        {
            return Result<string>.NotSupported();
        }

        if (FormattableStringParser.NextPositionIsSign(state.Input, state.Index, FormattableStringParser.OpenSign)
            || FormattableStringParser.PreviousPositionIsSign(state.Input, state.Index, FormattableStringParser.OpenSign))
        {
            return Result<string>.Continue();
        }

        if (state.InPlaceholder)
        {
            return Result<string>.Invalid("Recursive placeholder detected, this is not supported");
        }

        state.InPlaceholder = true;

        return Result<string>.NoContent();
    }
}
