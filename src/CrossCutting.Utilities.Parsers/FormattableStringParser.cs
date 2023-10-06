namespace CrossCutting.Utilities.Parsers;

public class FormattableStringParser : IFormattableStringParser
{
    public const char OpenSign = '{';
    public const char CloseSign = '}';

    private readonly IEnumerable<IFormattableStringStateProcessor> _processors;

    public FormattableStringParser(IEnumerable<IFormattableStringStateProcessor> processors)
    {
        processors = processors.IsNotNull(nameof(processors));

        _processors = processors;
    }

    public static IFormattableStringParser Create(params IPlaceholderProcessor[] processors)
    {
        processors = processors.IsNotNull(nameof(processors));

        return new FormattableStringParser(
            new IFormattableStringStateProcessor[]
            {
                new OpenSignProcessor(),
                new CloseSignProcessor(processors),
                new PlaceholderProcessor(),
                new ResultProcessor()
            });
    }

    public Result<string> Parse(string input, IFormatProvider formatProvider, object? context)
    {
        input = input.IsNotNull(nameof(input));
        formatProvider = formatProvider.IsNotNull(nameof(formatProvider));

        var state = new FormattableStringParserState(input, formatProvider, context, this);

        for (var index = 0; index < input.Length; index++)
        {
            state.Update(input[index], index);

            foreach (var processor in _processors)
            {
                var processorResult = processor.Process(state);
                if (!processorResult.IsSuccessful())
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
