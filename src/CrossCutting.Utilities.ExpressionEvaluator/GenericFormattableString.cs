namespace CrossCutting.Utilities.ExpressionEvaluator;

public sealed class GenericFormattableString(string format, object[] arguments) : FormattableString, IEquatable<GenericFormattableString>
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

    public static GenericFormattableString FromString(string s)
        => new(s);

    public static implicit operator string(GenericFormattableString r)
        => r?.ToString()!;

    public static implicit operator GenericFormattableString(string s)
        => FromString(s);

#pragma warning disable S3875 // "operator==" should not be overloaded on reference types
    public static bool operator ==(GenericFormattableString a, GenericFormattableString b)
#pragma warning restore S3875 // "operator==" should not be overloaded on reference types
        => a?.ToString() == b?.ToString();

    public static bool operator !=(GenericFormattableString a, GenericFormattableString b)
        => !(a == b);

    public override bool Equals(object obj)
        => obj is GenericFormattableString && this == (GenericFormattableString)obj;

    public bool Equals(GenericFormattableString other)
        => other?.ToString() == ToString();

    public override int GetHashCode()
        => ToString().GetHashCode();
}
