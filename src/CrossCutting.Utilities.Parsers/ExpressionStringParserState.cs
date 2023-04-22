namespace CrossCutting.Utilities.Parsers;

internal class ExpressionStringParserState
{
    internal string Input { get; }
    internal IFormatProvider FormatProvider { get; }
    internal IExpressionParser ExpressionParser { get; } //TODO: Review if we can remove this by using DI everywhere
    internal Func<string, Result<string>> PlaceholderDelegate { get; }
    internal IFunctionParser FunctionParser { get; }

    internal ExpressionStringParserState(
        string input,
        IFormatProvider formatProvider,
        IExpressionParser expressionParser,
        Func<string, Result<string>> placeholderDelegate,
        IFunctionParser functionParser)
    {
        Input = input;
        FormatProvider = formatProvider;
        ExpressionParser = expressionParser;
        PlaceholderDelegate = placeholderDelegate;
        FunctionParser = functionParser;
    }
}
