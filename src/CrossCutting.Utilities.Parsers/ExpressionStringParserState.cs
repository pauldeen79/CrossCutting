namespace CrossCutting.Utilities.Parsers;

internal class ExpressionStringParserState
{
    internal string Input { get; }
    internal Func<string, Result<object>> ParseExpressionDelegate { get; }
    internal Func<string, Result<string>> PlaceholderDelegate { get; }
    internal Func<FunctionParseResult, Result<object>> ParseFunctionDelegate { get; }

    internal ExpressionStringParserState(
        string input,
        Func<string, Result<object>> parseExpressionDelegate,
        Func<string, Result<string>> placeholderDelegate,
        Func<FunctionParseResult, Result<object>> parseFunctionDelegate)
    {
        Input = input;
        ParseExpressionDelegate = parseExpressionDelegate;
        PlaceholderDelegate = placeholderDelegate;
        ParseFunctionDelegate = parseFunctionDelegate;
    }
}
