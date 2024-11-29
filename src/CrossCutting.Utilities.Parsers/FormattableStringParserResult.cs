namespace CrossCutting.Utilities.Parsers;

public class FormattableStringParserResult(string format, object[] arguments) : FormattableString
{
    private readonly object[] _arguments = arguments.IsNotNull(nameof(arguments));

    public FormattableStringParserResult(object? value) : this("{0}", [value!])
    {
    }

    public override int ArgumentCount
        => _arguments.Length;

    public override string Format { get; } = format.IsNotNull(nameof(format));

    public override object GetArgument(int index)
        => _arguments[index];

    public override object[] GetArguments()
        => _arguments;

    public override string ToString(IFormatProvider formatProvider)
        => string.Format(formatProvider, Format, _arguments);

    public override string ToString()
        => string.Format(Format, _arguments);

    public static FormattableStringParserResult FromString(string s) => new(s);

    public static implicit operator string(FormattableStringParserResult r) => r?.ToString()!;

    public static implicit operator FormattableStringParserResult(string s) => FromString(s);
}
