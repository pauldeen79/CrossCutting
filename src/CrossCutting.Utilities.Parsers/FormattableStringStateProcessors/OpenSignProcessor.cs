namespace CrossCutting.Utilities.Parsers.FormattableStringStateProcessors;

public class OpenSignProcessor : IFormattableStringStateProcessor
{
    public Result<string> Process(FormattableStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Current != FormattableStringParser.OpenSign)
        {
            return Result.Continue<string>();
        }

        if (state.NextPositionIsSign(FormattableStringParser.OpenSign)
            || state.PreviousPositionIsSign(FormattableStringParser.OpenSign))
        {
            if (state.NextPositionIsSign(FormattableStringParser.OpenSign))
            {
                state.Escape();
            }

            return Result.Continue<string>();
        }

        if (state.InPlaceholder)
        {
            return Result.Invalid<string>("Recursive placeholder detected, this is not supported");
        }

        state.StartPlaceholder();

        return Result.NoContent<string>();
    }
}
