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
        var error = _processors
            .Select(x => x.Process(state))
            .FirstOrDefault(x => !x.IsSuccessful());
        
        if (error != null)
        {
            return Result<object?>.FromExistingResult(error);
        }

        return state.Results.Any()
            ? state.Results.Last()
            : _expressionParser.Parse(input, formatProvider);
    }
}
