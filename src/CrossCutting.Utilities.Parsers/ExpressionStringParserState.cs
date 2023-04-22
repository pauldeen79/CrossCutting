namespace CrossCutting.Utilities.Parsers;

public class ExpressionStringParserState
{
    public string Input { get; }
    public IFormatProvider FormatProvider { get; }

    public ExpressionStringParserState(string input, IFormatProvider formatProvider)
    {
        Input = input;
        FormatProvider = formatProvider;
    }
}
