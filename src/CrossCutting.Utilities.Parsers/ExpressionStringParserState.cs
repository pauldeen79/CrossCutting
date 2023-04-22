namespace CrossCutting.Utilities.Parsers;

internal class ExpressionStringParserState
{
    internal string Input { get; }
    internal IFormatProvider FormatProvider { get; }
    internal IExpressionParser ExpressionParser { get; } //TODO: Review if we can remove this by using DI everywhere
    internal IPlaceholderProcessor PlaceholderProcessor { get; } // TODO: Review if we can remove this by using DI everywhere
    internal IFunctionResultParser FunctionResultParser { get; } //TODO: Review if we can remove this by using DI everywhere

    internal ExpressionStringParserState(
        string input,
        IFormatProvider formatProvider,
        IExpressionParser expressionParser,
        IPlaceholderProcessor placeholderProcessor,
        IFunctionResultParser functionResultParser)
    {
        Input = input;
        FormatProvider = formatProvider;
        ExpressionParser = expressionParser;
        PlaceholderProcessor = placeholderProcessor;
        FunctionResultParser = functionResultParser;
    }
}
