namespace CrossCutting.Utilities.Parsers.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Gets generic type from a type string in this format: TypeName&lt;arguments&gt;
    /// </summary>
    /// <param name="instance"></param>
    /// <returns>TypeName</returns>
    public static string WithoutGenerics(this string instance)
    {
        var index = instance.IndexOf('<');
        return index == -1
            ? instance
            : instance.Substring(0, index);
    }

    /// <summary>
    /// Gets generic type argument from a type string in this format: TypeName&lt;arguments&gt;
    /// </summary>
    /// <param name="value">Full typename, i.e. TypeName&lt;arguments&gt;</param>
    /// <returns>arguments</returns>
    public static string GetGenericArguments(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var open = value.IndexOf("<");
        if (open == -1)
        {
            return string.Empty;
        }

        var close = value.LastIndexOf(">");

        if (close == -1)
        {
            return string.Empty;
        }

        return value.Substring(open + 1, close - open - 1);
    }
}
