namespace CrossCutting.Utilities.Parsers;

public class MathematicExpressionParser : IMathematicExpressionParser
{
    internal const string TemporaryDelimiter = "``";

    private static readonly IMathematicExpressionProcessor[] _expressionProcessors = new IMathematicExpressionProcessor[]
    {
        new Validate(),
        new Recursion(),
        new Operators(),
    };

    private readonly IExpressionParser _expressionParser;

    public MathematicExpressionParser(IExpressionParser expressionParser)
    {
        _expressionParser = expressionParser;
    }

    public Result<object> Parse(string input, IFormatProvider formatProvider)
    {
        var state = new MathematicExpressionState(input, formatProvider, _expressionParser, Parse);
        foreach (var processor in _expressionProcessors)
        {
            var result = processor.Process(state);
            if (!result.IsSuccessful())
            {
                return Result<object>.FromExistingResult(result);
            }
        }

        return state.Results.Any()
            ? state.Results.Last()
            : _expressionParser.Parse(input, formatProvider);
    }
}
