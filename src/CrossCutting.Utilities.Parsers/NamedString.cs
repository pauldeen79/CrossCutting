namespace CrossCutting.Common.Utilities.Parsers;

public static class NamedString
{
    public static string Format(string template, IFormatProvider formatProvider, object parameters)
        => Format(template, formatProvider, parameters, false, 500);

    public static string Format(string template, IFormatProvider formatProvider, object parameters, bool ignoreCase)
        => Format(template, formatProvider, parameters, ignoreCase, 500);

    public static string Format(string template, IFormatProvider formatProvider, object parameters, bool ignoreCase, int matchTimeoutInMilliseconds)
        => parameters.ToExpandoObject().Aggregate
        (
            template,
            (previous, item) => Regex.Replace(previous, $"{{{item.Key}}}", ConvertToString(item.Value, formatProvider), CreateRegExOptions(ignoreCase), TimeSpan.FromMilliseconds(matchTimeoutInMilliseconds))
        );

    private static string ConvertToString(object value, IFormatProvider formatProvider)
    {
        if (value == null)
        {
            return string.Empty;
        }

        if (value is IFormattable f)
        {
            return f.ToString(null, formatProvider);
        }

        return value.ToString();
    }

    private static RegexOptions CreateRegExOptions(bool ignoreCase)
        => ignoreCase
            ? RegexOptions.IgnoreCase
            : RegexOptions.None;
}
