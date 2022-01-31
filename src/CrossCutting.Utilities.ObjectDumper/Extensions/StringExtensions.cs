namespace CrossCutting.Utilities.ObjectDumper.Extensions;

public static class StringExtensions
{
    public static string DumpTypeName(this string typeName) => string.Format(" [{0}]", typeName.FixTypeName() ?? "{NULL}");

    public static string JsonQuote(this string instance)
    {
        if (string.IsNullOrEmpty(instance))
        {
            return "\"\"";
        }

        char c;
        int i;
        int len = instance.Length;
        var sb = new StringBuilder(len + 4);
        sb.Append('"');
        for (i = 0; i < len; i++)
        {
            c = instance[i];
            switch (c)
            {
                case '\\':
                case '"':
                case '/':
                    sb.Append('\\');
                    sb.Append(c);
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                default:
                    if (c < ' ')
                    {
                        var t = "000" + ((int)c).ToString("X");
                        sb
                            .Append("\\u")
                            .Append(t, t.Length - 4, t.Length - 4);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                    break;
            }
        }
        sb.Append('"');
        return sb.ToString();
    }

    /// <summary>
    /// Fixes the name of the type.
    /// </summary>
    /// <param name="instance">The type name to fix.</param>
    /// <returns></returns>
    public static string FixTypeName(this string instance)
    {
        int startIndex;
        while (true)
        {
            startIndex = instance.IndexOf(", ");
            if (startIndex == -1)
            {
                break;
            }

            int secondIndex = instance.IndexOf("]", startIndex + 1);
            if (secondIndex == -1)
            {
                break;
            }

            instance = instance.Substring(0, startIndex) + instance.Substring(secondIndex + 1);
        }

        while (true)
        {
            startIndex = instance.IndexOf("`");
            if (startIndex == -1)
            {
                break;
            }

            instance = instance.Substring(0, startIndex) + instance.Substring(startIndex + 2);
        }

        //remove assebmly name from type, e.g. System.String, mscorlib bla bla bla -> System.String
        var index = instance.IndexOf(", ");
        if (index > -1)
        {
            instance = instance.Substring(0, index);
        }

        return FixAnonymousTypeName(instance
            .Replace("[[", "<")
            .Replace(",[", ",")
            .Replace(",]", ">" /*","*/)
            .Replace("]", ">")
            .Replace("[>", "[]") //hacking here! caused by the previous statements...
            .Replace("System.Void", "void")
            .Replace("+", ".")
            .Replace("&", ""));
    }

    private static string FixAnonymousTypeName(string instance)
    {
        var isAnonymousType = instance.Contains("AnonymousType")
            && (instance.Contains("<>") || instance.Contains("VB$"));

        var arraySuffix = instance.EndsWith("[]")
            ? "[]"
            : string.Empty;

        return isAnonymousType
            ? $"AnonymousType{arraySuffix}"
            : instance;
    }
}
