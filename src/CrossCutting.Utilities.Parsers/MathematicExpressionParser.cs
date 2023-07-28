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

    internal static bool IsMathematicExpression(string found)
        => Array.Exists(MathematicOperators.Aggregators, x => found.Contains(x.Character.ToString()));

    public Result<object?> Parse(string input, IFormatProvider formatProvider, object? context)
    {
        var state = new MathematicExpressionState(input, formatProvider, context, Parse);
        var error = _processors
            .Select(x => x.Process(state))
            .FirstOrDefault(x => !x.IsSuccessful());
        
        if (error is not null)
        {
            return Result<object?>.FromExistingResult(error);
        }

        return state.Results.Any()
            ? state.Results[state.Results.Count - 1]
            : _expressionParser
                .Parse(input, formatProvider, context)
                .Transform(x => x.ErrorMessage?.StartsWith("Unknown expression type found in fragment: ") == true
                    ? Result<object?>.NotFound()
                    : x);
    }
}
