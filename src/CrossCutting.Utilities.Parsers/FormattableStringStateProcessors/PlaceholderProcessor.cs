namespace CrossCutting.Utilities.Parsers.FormattableStringStateProcessors;

public class PlaceholderProcessor : IFormattableStringStateProcessor
{
    public Result<FormattableStringParserResult> Process(FormattableStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (!state.InPlaceholder)
        {
            return Result.Continue<FormattableStringParserResult>();
        }

        state.PlaceholderBuilder.Append(state.Current);

        return Result.NoContent<FormattableStringParserResult>();
    }
}
