namespace CrossCutting.Utilities.Parsers;

public class FormattableStringParser : IFormattableStringParser
{
    public const char OpenSign = '{';
    public const char CloseSign = '}';

    private readonly IPlaceholderProcessor _placeholderProcessor;
    private readonly IEnumerable<IFormattableStringStateProcessor> _processors;

    public FormattableStringParser(
        IPlaceholderProcessor placeholderProcessor,
        IEnumerable<IFormattableStringStateProcessor> processors)
    {
        _placeholderProcessor = placeholderProcessor;
        _processors = processors;
    }

    public Result<string> Parse(string input)
    {
        var state = new FormattableStringParserState(input, _placeholderProcessor);

        for (var index = 0; index < input.Length; index++)
        {
            state.Current = input[index];
            state.Index = index;

            foreach (var processor in _processors)
            {
                var processorResult = processor.Process(state);
                if (processorResult.Status != ResultStatus.NotSupported && !processorResult.IsSuccessful())
                {
                    return processorResult;
                }
                else if (processorResult.Status == ResultStatus.NoContent)
                {
                    break;
                }
            }
        }

        if (state.InPlaceholder)
        {
            return Result<string>.Invalid("Missing close sign '}'. To use the '{' character, you have to escape it with an additional '{' character");
        }

        return Result<string>.Success(state.ResultBuilder.ToString());
    }

    public static bool NextPositionIsSign(string input, int index, char sign)
    {
        if (index + 1 == input.Length)
        {
            // We're at the end of the string, so this is not possible
            return false;
        }

        return input[index + 1] == sign;
    }

    public static bool PreviousPositionIsSign(string input, int index, char sign)
    {
        if (index == 0)
        {
            // We're at the end of the string, so this is not possible
            return false;
        }

        return input[index - 1] == sign;
    }
}
