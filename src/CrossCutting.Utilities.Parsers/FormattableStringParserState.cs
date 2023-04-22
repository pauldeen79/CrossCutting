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

    public bool NextPositionIsSign(char sign)
    {
        if (Index + 1 == Input.Length)
        {
            // We're at the end of the string, so this is not possible
            return false;
        }

        return Input[Index + 1] == sign;
    }

    public bool PreviousPositionIsSign(char sign)
    {
        if (Index == 0)
        {
            // We're at the end of the string, so this is not possible
            return false;
        }

        return Input[Index - 1] == sign;
    }
}
