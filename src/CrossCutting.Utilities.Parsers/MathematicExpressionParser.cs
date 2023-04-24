namespace CrossCutting.Utilities.Parsers;

public class MathematicExpressionParser : IMathematicExpressionParser
{
    public const string TemporaryDelimiter = "``";

    private readonly IExpressionParser _expressionParser;
    private readonly IEnumerable<IMathematicExpressionProcessor> _processors;

    public MathematicExpressionParser(IExpressionParser expressionParser, IEnumerable<IMathematicExpressionProcessor> processors)
    {
        _expressionParser = expressionParser;
        _processors = processors;
    }

    public Result<object?> Parse(string input, IFormatProvider formatProvider)
    {
        var state = new MathematicExpressionState(input, formatProvider, Parse);
        foreach (var processor in _processors)
        {
            var result = processor.Process(state);
            if (!result.IsSuccessful())
            {
                return Result<object?>.FromExistingResult(result);
            }
        }

        return state.Results.Any()
            ? state.Results.Last()
            : _expressionParser.Parse(input, formatProvider);
    }
}
