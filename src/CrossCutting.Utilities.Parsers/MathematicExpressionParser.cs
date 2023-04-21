namespace CrossCutting.Utilities.Parsers;

public static partial class MathematicExpressionParser
{
    internal const string TemporaryDelimiter = "``";

    private static readonly IMathematicExpressionProcessor[] _expressionProcessors = new IMathematicExpressionProcessor[]
    {
        new Validate(),
        new Recursion(),
        new Operators(),
    };

    public static Result<object> Parse(string input, Func<string, Result<object>> parseExpressionDelegate)
    {
        var state = new MathematicExpressionState(input, parseExpressionDelegate, Parse);
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
            : parseExpressionDelegate(input);
    }
}
