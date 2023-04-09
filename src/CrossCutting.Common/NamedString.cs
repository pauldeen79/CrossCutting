namespace CrossCutting.Common;

public static class NamedString
{
    public static string Format(string template, object parameters)
        => Format(template, parameters, false, 500);

    public static string Format(string template, object parameters, bool ignoreCase)
        => Format(template, parameters, ignoreCase, 500);

    public static string Format(string template, object parameters, bool ignoreCase, int matchTimeoutInMilliseconds)
        => parameters.ToExpandoObject().Aggregate
        (
            template,
            (previous, item) => Regex.Replace(previous, $"{{{item.Key}}}", item.Value.ToStringWithDefault(), CreateRegExOptions(ignoreCase), TimeSpan.FromMilliseconds(matchTimeoutInMilliseconds))
        );

    private static RegexOptions CreateRegExOptions(bool ignoreCase)
        => ignoreCase
            ? RegexOptions.IgnoreCase
            : RegexOptions.None;
}
