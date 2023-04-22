namespace CrossCutting.Utilities.Parsers.FormattableStringStateProcessors;

public class ResultProcessor : IFormattableStringStateProcessor
{
    public Result<string> Process(FormattableStringParserState state)
    {
        state.ResultBuilder.Append(state.Current);

        return Result<string>.NoContent();
    }
}
