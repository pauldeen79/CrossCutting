namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringParserState
{
    public string Input { get; }
    public IFormatProvider FormatProvider { get; }
    public object? Context { get; }
    public IExpressionStringParser Parser { get; }

    public ExpressionStringParserState(string input, IFormatProvider formatProvider, object? context, IExpressionStringParser parser)
    {
        Input = input;
        FormatProvider = formatProvider;
        Context = context;
        Parser = parser;
    }
}
