namespace CrossCutting.Utilities.Parsers;

public class FormattableStringParserState
{
    public string Input { get; }

    public StringBuilder ResultBuilder { get; } = new();
    public StringBuilder PlaceholderBuilder { get; } = new();
    public bool InPlaceholder { get; set; }
    public char Current { get; set; }
    public int Index { get; set; }

    public FormattableStringParserState(string input)
    {
        Input = input;
    }
}
