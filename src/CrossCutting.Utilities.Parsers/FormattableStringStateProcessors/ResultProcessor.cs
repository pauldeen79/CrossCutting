namespace CrossCutting.Utilities.Parsers.FormattableStringStateProcessors;

public class ResultProcessor : IFormattableStringStateProcessor
{
    public Result<FormattableStringParserResult> Process(FormattableStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        state.ResultBuilder.Append(state.Current);

        return Result.NoContent<FormattableStringParserResult>();
    }
}
