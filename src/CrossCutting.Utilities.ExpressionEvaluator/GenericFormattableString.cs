namespace CrossCutting.Utilities.ExpressionEvaluator;

public class GenericFormattableString(string format, object[] arguments) : FormattableString
{
    private readonly object[] _arguments = arguments.IsNotNull(nameof(arguments));

    public GenericFormattableString(object? value) : this("{0}", [value!])
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

    public static GenericFormattableString FromString(string s) => new(s);

    public static implicit operator string(GenericFormattableString r) => r?.ToString()!;

    public static implicit operator GenericFormattableString(string s) => FromString(s);
}
