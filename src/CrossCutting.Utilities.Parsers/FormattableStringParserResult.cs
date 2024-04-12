namespace CrossCutting.Utilities.Parsers;

public class FormattableStringParserResult : FormattableString
{
    private readonly object[] _arguments;

    public FormattableStringParserResult(object? value) : this("{0}", [value!])
    {
    }

    public FormattableStringParserResult(string format, object[] arguments)
    {
        Format = format.IsNotNull(nameof(format));
        _arguments = arguments.IsNotNull(nameof(arguments));
    }

    public override int ArgumentCount
        => _arguments.Length;

    public override string Format { get; }

    public override object GetArgument(int index)
        => _arguments[index];

    public override object[] GetArguments()
        => _arguments;

    public override string ToString(IFormatProvider formatProvider)
        => string.Format(formatProvider, Format, _arguments);

    //TODO: Review if we need this
    //public static implicit operator string(FormattableStringParserResult r) => r?.ToString()!;
}
