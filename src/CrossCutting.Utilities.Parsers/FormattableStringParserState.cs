namespace CrossCutting.Utilities.Parsers;

public class FormattableStringParserState
{
    public string Input { get; }
    public IFormatProvider FormatProvider { get; }
    public object? Context { get; }

    public StringBuilder ResultBuilder { get; } = new();
    public StringBuilder PlaceholderBuilder { get; } = new();
    public bool InPlaceholder { get; private set; }
    public char Current { get; private set; }
    public int Index { get; private set; }
    public bool IsEscaped { get; private set; }

    public FormattableStringParserState(string input, IFormatProvider formatProvider, object? context)
    {
        Input = input;
        FormatProvider = formatProvider;
        Context = context;
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

    internal FormattableStringParserState Update(char current, int index)
    {
        Current = current;
        Index = index;
        return this;
    }

    public void Escape() => IsEscaped = true;

    public void ResetEscape() => IsEscaped = false;

    public void StartPlaceholder() => InPlaceholder = true;

    public void ClosePlaceholder(string value)
    {
        InPlaceholder = false;
        ResultBuilder.Append(value);
        PlaceholderBuilder.Clear();
    }
}
