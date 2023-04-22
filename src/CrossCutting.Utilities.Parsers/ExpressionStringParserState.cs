namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringParserState
{
    public string Input { get; }
    public IFormatProvider FormatProvider { get; }
    public IPlaceholderProcessor PlaceholderProcessor { get; } // TODO: Review if we can remove this by using DI everywhere
    public IFunctionResultParser FunctionResultParser { get; } //TODO: Review if we can remove this by using DI everywhere

    public ExpressionStringParserState(
        string input,
        IFormatProvider formatProvider,
        IPlaceholderProcessor placeholderProcessor,
        IFunctionResultParser functionResultParser)
    {
        Input = input;
        FormatProvider = formatProvider;
        PlaceholderProcessor = placeholderProcessor;
        FunctionResultParser = functionResultParser;
    }
}
