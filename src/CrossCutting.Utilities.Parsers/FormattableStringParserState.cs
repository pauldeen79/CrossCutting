namespace CrossCutting.Utilities.Parsers;

public class FormattableStringParserState
{
    public string Input { get; }
    public IFormatProvider FormatProvider { get; }
    public object? Context { get; }
    public IFormattableStringParser Parser { get; }

    public StringBuilder ResultBuilder { get; } = new();
    public StringBuilder PlaceholderBuilder { get; } = new();
    public bool InPlaceholder { get; private set; }
    public char Current { get; private set; }
    public int Index { get; private set; }
    public bool IsEscaped { get; private set; }
    public string ResultFormat => ResultBuilder.ToString();
    public Collection<object> ResultArguments { get; } = new();

    public FormattableStringParserState(string input, IFormatProvider formatProvider, object? context, IFormattableStringParser parser)
    {
        ArgumentGuard.IsNotNull(input, nameof(input));
        ArgumentGuard.IsNotNull(formatProvider, nameof(formatProvider));
        ArgumentGuard.IsNotNull(parser, nameof(parser));

        Input = input;
        FormatProvider = formatProvider;
        Context = context;
        Parser = parser;
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

    public FormattableStringParserState Update(char current, int index)
    {
        Current = current;
        Index = index;
        return this;
    }

    public void Escape() => IsEscaped = true;

    public void ResetEscape() => IsEscaped = false;

    public void StartPlaceholder() => InPlaceholder = true;

    public void ClosePlaceholder(FormattableStringParserResult value)
    {
        value = value.IsNotNull(nameof(value));

        InPlaceholder = false;
        ResultBuilder.Append(value.Format);
        ResultArguments.AddRange(value.GetArguments());
        PlaceholderBuilder.Clear();
    }
}
