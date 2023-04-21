namespace CrossCutting.Utilities.Parsers;

internal class ExpressionStringParserState
{
    internal string Input { get; }
    internal IFormatProvider FormatProvider { get; }
    internal Func<string, IFormatProvider, Result<object>> ParseExpressionDelegate { get; }
    internal Func<string, Result<string>> PlaceholderDelegate { get; }
    internal Func<FunctionParseResult, Result<object>> ParseFunctionDelegate { get; }

    internal ExpressionStringParserState(
        string input,
        IFormatProvider formatProvider,
        Func<string, IFormatProvider, Result<object>> parseExpressionDelegate,
        Func<string, Result<string>> placeholderDelegate,
        Func<FunctionParseResult, Result<object>> parseFunctionDelegate)
    {
        Input = input;
        FormatProvider = formatProvider;
        ParseExpressionDelegate = parseExpressionDelegate;
        PlaceholderDelegate = placeholderDelegate;
        ParseFunctionDelegate = parseFunctionDelegate;
    }
}
