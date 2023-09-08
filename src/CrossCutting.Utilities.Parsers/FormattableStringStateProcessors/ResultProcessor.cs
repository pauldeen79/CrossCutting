namespace CrossCutting.Utilities.Parsers.FormattableStringStateProcessors;

public class ResultProcessor : IFormattableStringStateProcessor
{
    public Result<string> Process(FormattableStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.IsEscaped)
        {
            state.ResetEscape();
        }
        else
        {
            state.ResultBuilder.Append(state.Current);
        }

        return Result<string>.NoContent();
    }
}
