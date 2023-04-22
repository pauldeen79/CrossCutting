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

        if (FormattableStringParser.NextPositionIsSign(state.Input, state.Index, FormattableStringParser.CloseSign)
            || FormattableStringParser.PreviousPositionIsSign(state.Input, state.Index, FormattableStringParser.CloseSign))
        {
            return Result<string>.Continue();
        }

        if (!state.InPlaceholder)
        {
            return Result<string>.Invalid("Missing open sign '{'. To use the '}' character, you have to escape it with an additional '}' character");
        }

        var placeholderResult = _processor.Process(state.PlaceholderBuilder.ToString());
        if (!placeholderResult.IsSuccessful())
        {
            return placeholderResult;
        }

        state.InPlaceholder = false;
        state.ResultBuilder.Append(placeholderResult.Value!);
        state.PlaceholderBuilder.Clear();

        return Result<string>.NoContent();
    }
}
