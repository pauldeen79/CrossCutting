namespace CrossCutting.Utilities.Parsers;

public class MathematicExpressionParser : IMathematicExpressionParser
{
    public const string TemporaryDelimiter = "``";

    private readonly IExpressionParser _expressionParser;
    private readonly IEnumerable<IMathematicExpressionProcessor> _processors;

    public MathematicExpressionParser(IExpressionParser expressionParser, IEnumerable<IMathematicExpressionProcessor> processors)
    {
        ArgumentGuard.IsNotNull(expressionParser, nameof(expressionParser));
        ArgumentGuard.IsNotNull(processors, nameof(processors));

        _expressionParser = expressionParser;
        _processors = processors;
    }

    internal static bool IsMathematicExpression(string found)
    {
        ArgumentGuard.IsNotNull(found, nameof(found));

        return Array.Exists(MathematicOperators.Aggregators, x => found.Contains(x.Character.ToString()));
    }

    public Result<object?> Parse(string input, IFormatProvider formatProvider, object? context)
    {
        if (input is null)
        {
            return Result.Invalid<object?>("Input is required");
        }

        var state = new MathematicExpressionState(input, formatProvider, context, Parse);
        var error = _processors
            .Select(x => x.Process(state))
            .FirstOrDefault(x => !x.IsSuccessful());

        if (error is not null)
        {
            return Result.FromExistingResult<object?>(error);
        }

        return state.Results.Count > 0
            ? state.Results.ElementAt(state.Results.Count - 1)
            : _expressionParser
                .Parse(input, formatProvider, context)
                .Transform(x => x.ErrorMessage?.StartsWith("Unknown expression type found in fragment: ") == true
                    ? Result.NotFound<object?>()
                    : x);
    }

    public Result Validate(string input, IFormatProvider formatProvider, object? context)
    {
        if (input is null)
        {
            return Result.Invalid("Input is required");
        }

        var state = new MathematicExpressionState(input, formatProvider, context, Parse);
        var error = _processors
            .OfType<Validate>()
            .Select(x => x.Process(state))
            .FirstOrDefault(x => !x.IsSuccessful());

        if (error is not null)
        {
            return error;
        }

        return state.Results.Count > 0
            ? state.Results.ElementAt(state.Results.Count - 1)
            : _expressionParser
                .Validate(input, formatProvider, context)
                .Transform(x => x.ErrorMessage?.StartsWith("Unknown expression type found in fragment: ") == true
                    ? Result.NotFound()
                    : x);
    }
}
