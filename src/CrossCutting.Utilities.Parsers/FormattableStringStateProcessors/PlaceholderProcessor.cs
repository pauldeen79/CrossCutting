namespace CrossCutting.Utilities.Parsers.FormattableStringStateProcessors;

public class PlaceholderProcessor : IFormattableStringStateProcessor
{
    public Result<string> Process(FormattableStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (!state.InPlaceholder)
        {
            return Result<string>.NotSupported();
        }

        state.PlaceholderBuilder.Append(state.Current);

        return Result<string>.NoContent();
    }
}
