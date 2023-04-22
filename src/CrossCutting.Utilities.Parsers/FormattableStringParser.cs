namespace CrossCutting.Utilities.Parsers;

public class FormattableStringParser : IFormattableStringParser
{
    public const char OpenSign = '{';
    public const char CloseSign = '}';

    private readonly IEnumerable<IFormattableStringStateProcessor> _processors;

    public FormattableStringParser(IEnumerable<IFormattableStringStateProcessor> processors)
    {
        _processors = processors;
    }

    public Result<string> Parse(string input)
    {
        var state = new FormattableStringParserState(input);

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
}
