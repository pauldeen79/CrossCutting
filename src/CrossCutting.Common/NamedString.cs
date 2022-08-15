namespace CrossCutting.Common;

public static class NamedString
{
    public static string Format(string template, object parameters)
        => Format(template, parameters, false);

    public static string Format(string template, object parameters, bool ignoreCase)
    {
        var parametersDictionary = parameters.ToExpandoObject() as IDictionary<string, object>;

        foreach (var item in parametersDictionary)
        {
            template = Regex.Replace(template, $"{{{item.Key}}}", item.Value.ToStringWithDefault(), ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        return template;
    }
}
