namespace CrossCutting.Utilities.Parsers.FormattableStringStateProcessors;

public class OpenSignProcessor : IFormattableStringStateProcessor
{
    public Result<FormattableStringParserResult> Process(FormattableStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Current != FormattableStringParser.OpenSign)
        {
            return Result.Continue<FormattableStringParserResult>();
        }

        if (state.NextPositionIsSign(FormattableStringParser.OpenSign)
            || state.PreviousPositionIsSign(FormattableStringParser.OpenSign))
        {
            return Result.Continue<FormattableStringParserResult>();
        }

        if (state.InPlaceholder)
        {
            return Result.Invalid<FormattableStringParserResult>("Recursive placeholder detected, this is not supported");
        }

        state.StartPlaceholder();

        return Result.NoContent<FormattableStringParserResult>();
    }
}
