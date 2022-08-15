namespace CrossCutting.Common;

public static class NamedString
{
    public static string Format(string template, object parameters)
        => Format(template, parameters, false);

    public static string Format(string template, object parameters, bool ignoreCase)
        => parameters.ToExpandoObject().Aggregate
        (
            template,
            (previous, item) => Regex.Replace(previous, $"{{{item.Key}}}", item.Value.ToStringWithDefault(), CreateRegExOptions(ignoreCase))
        );

    private static RegexOptions CreateRegExOptions(bool ignoreCase)
        => ignoreCase
            ? RegexOptions.IgnoreCase
            : RegexOptions.None;
}
