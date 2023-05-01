using System.ComponentModel.Design.Serialization;

namespace CrossCutting.Common.Extensions;

public static class StringExtensions
{
    private static readonly string[] _trueKeywords = new[] { "true", "t", "1", "y", "yes", "ja", "j" };
    private static readonly string[] _falseKeywords = new[] { "false", "f", "0", "n", "no", "nee" };

    /// <summary>
    /// Performs a is null or empty check, and returns another value when this evaluates to true.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="whenNullOrEmpty">The when null or empty.</param>
    /// <returns></returns>
    public static string WhenNullOrEmpty(this string? instance, string whenNullOrEmpty)
    {
        if (instance == null || string.IsNullOrEmpty(instance))
        {
            return whenNullOrEmpty;
        }

        return instance;
    }

    /// <summary>
    /// Performs a is null or empty check, and returns another value when this evaluates to true.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="whenNullOrEmptyDelegate">The delegate to invoke when null or empty.</param>
    /// <returns></returns>
    public static string WhenNullOrEmpty(this string? instance, Func<string> whenNullOrEmptyDelegate)
    {
        if (instance == null || string.IsNullOrEmpty(instance))
        {
            return whenNullOrEmptyDelegate();
        }

        return instance;
    }

    /// <summary>
    /// Performs a is null or whitespace check, and returns another value when this evaluates to true.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="whenNullOrWhiteSpace">The when null or white space.</param>
    /// <returns></returns>
    public static string WhenNullOrWhitespace(this string? instance, string whenNullOrWhiteSpace)
    {
        if (instance == null || string.IsNullOrWhiteSpace(instance))
        {
            return whenNullOrWhiteSpace;
        }

        return instance;
    }

    /// <summary>
    /// Performs a is null or whitespace check, and returns another value when this evaluates to true.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="whenNullOrWhiteSpaceDelegate">The when null or white space.</param>
    /// <returns></returns>
    public static string WhenNullOrWhitespace(this string? instance, Func<string> whenNullOrWhiteSpaceDelegate)
    {
        if (instance == null || string.IsNullOrWhiteSpace(instance))
        {
            return whenNullOrWhiteSpaceDelegate();
        }

        return instance;
    }

    /// <summary>
    /// Determines whether the specified instance is true.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <returns></returns>
    public static bool IsTrue(this string? instance)
        => instance != null
            && _trueKeywords.Any(s => s.Equals(instance, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Determines whether the specified instance is false.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <returns></returns>
    public static bool IsFalse(this string? instance)
        => instance != null
            && _falseKeywords.Any(s => s.Equals(instance, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Indicates whether the string instance starts with any of the specified values.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public static bool StartsWithAny(this string instance, params string[] values)
        => instance.StartsWithAny((IEnumerable<string>)values);

    /// <summary>
    /// Indicates whether the string instance starts with any of the specified values.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public static bool StartsWithAny(this string instance, IEnumerable<string> values)
        => values.Any(v => instance.StartsWith(v));

    /// <summary>
    /// Indicates whether the string instance starts with any of the specified values.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="comparisonType">Type of the comparison.</param>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public static bool StartsWithAny(this string instance, StringComparison comparisonType, params string[] values)
        => instance.StartsWithAny(comparisonType, (IEnumerable<string>)values);

    /// <summary>
    /// Indicates whether the string instance starts with any of the specified values.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="comparisonType">Type of the comparison.</param>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public static bool StartsWithAny(this string instance, StringComparison comparisonType, IEnumerable<string> values)
        => values.Any(v => instance.StartsWith(v, comparisonType));

    /// <summary>
    /// Indicates whether the string instance ends with any of the specified values.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public static bool EndsWithAny(this string instance, params string[] values)
        => instance.EndsWithAny((IEnumerable<string>)values);

    /// <summary>
    /// Indicates whether the string instance ends with any of the specified values.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public static bool EndsWithAny(this string instance, IEnumerable<string> values)
        => values.Any(v => instance.EndsWith(v));

    /// <summary>
    /// Indicates whether the string instance ends any of the specified values.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="comparisonType">Type of the comparison.</param>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public static bool EndsWithAny(this string instance, StringComparison comparisonType, params string[] values)
        => instance.EndsWithAny(comparisonType, (IEnumerable<string>)values);

    /// <summary>
    /// Indicates whether the string instance ends any of the specified values.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="comparisonType">Type of the comparison.</param>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public static bool EndsWithAny(this string instance, StringComparison comparisonType, IEnumerable<string> values)
        => values.Any(v => instance.EndsWith(v, comparisonType));

    public static string[] GetLines(this string instance)
    {
        var result = new List<string>();
        using (var reader = new StringReader(instance))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                result.Add(line);
            }
        }
        return result.ToArray();
    }

    /// <summary>
    /// Replaces line endings to the right format conform the current operating system
    /// </summary>
    /// <param name="instance">String instance to get the normalized line endings for</param>
    /// <param name="matchTimeoutInMilliseconds">Time-out in milli secconds for the regular expression parsing</param>
    /// <returns></returns>
    public static string NormalizeLineEndings(this string instance, int matchTimeoutInMilliseconds = 500)
        => Regex.Replace(instance, @"\r\n|\n\r|\n|\r", Environment.NewLine, RegexOptions.None, TimeSpan.FromMilliseconds(matchTimeoutInMilliseconds));

    /// <summary>
    /// Splits the line using a custom delimiter
    /// </summary>
    /// <param name="instance">String instance to split</param>
    /// <param name="delimiter">Delimiter to use</param>
    /// <param name="textQualifier">When filled, text qualifier to use</param>
    /// <param name="leaveTextQualifier">If set to true and text qualifier is filled, then leave the text qualifiers in the string</param>
    /// <param name="trimItems">If set to true, then each will be trimmed</param>
    /// <returns></returns>
    public static string[] SplitDelimited(this string instance, char delimiter, char? textQualifier = null, bool leaveTextQualifier = false, bool trimItems = false)
        => instance.SplitDelimited(delimiter.ToString(), textQualifier, leaveTextQualifier, trimItems);

    /// <summary>
    /// Splits the line using a custom delimiter
    /// </summary>
    /// <param name="instance">String instance to split</param>
    /// <param name="delimiter">Delimiter to use (either 1 or 2 characters)</param>
    /// <param name="textQualifier">When filled, text qualifier to use</param>
    /// <param name="leaveTextQualifier">If set to true and text qualifier is filled, then leave the text qualifiers in the string</param>
    /// <param name="trimItems">If set to true, then each will be trimmed</param>
    /// <returns></returns>
    public static string[] SplitDelimited(this string instance, string delimiter, char? textQualifier = null, bool leaveTextQualifier = false, bool trimItems = false)
    {
        if (delimiter == null)
        {
            throw new ArgumentNullException(nameof(delimiter));
        }

        if (delimiter.Length != 1 && delimiter.Length != 2)
        {
            throw new ArgumentOutOfRangeException(nameof(delimiter), "Delimiter can only be 1 or 2 characters");
        }

        var result = new List<string>();
        var currentSection = new StringBuilder();
        var inText = false;
        var lastDelimiter = -1;
        var index = -1;
        char? previousCharacter = null;
        foreach (var character in instance)
        {
            index++;
            if (IsDelimiter(delimiter, character, previousCharacter) && !inText)
            {
                lastDelimiter = index;
                result.Add(currentSection.ToString());
                currentSection.Clear();
            }
            else if (textQualifier != null && character == textQualifier)
            {
                // skip this character
                inText = !inText;
                if (leaveTextQualifier)
                {
                    currentSection.Append(character);
                }
            }
            else if (!IsStartOfNextDelimiter(index, instance, delimiter, inText))
            {
                currentSection.Append(character);
            }

            previousCharacter = character;
        }

        AddRemainder(instance, result, currentSection, lastDelimiter);

        return result.Select(x => trimItems ? x.Trim() : x).ToArray();
    }

    private static bool IsStartOfNextDelimiter(int index, string instance, string delimiter, bool inText)
    {
        if (delimiter.Length == 1)
        {
            return false;
        }

        if (index + 1 == instance.Length)
        {
            return false;
        }

        if (inText)
        {
            return false;
        }

        return instance[index] == delimiter[0] && instance[index + 1] == delimiter[1];
    }

    private static void AddRemainder(string instance, List<string> result, StringBuilder currentSection, int lastDelimiter)
    {
        if (currentSection.Length > 0)
        {
            result.Add(currentSection.ToString());
        }
        else if (lastDelimiter > -1 && lastDelimiter + 1 == instance.Length)
        {
            result.Add(string.Empty);
        }
    }

    private static bool IsDelimiter(string delimiter, char character, char? previousCharacter)
    {
        if (delimiter.Length == 1)
        {
            return character == delimiter[0];
        }

        if (previousCharacter == null)
        {
            return false;
        }

        return character == delimiter[1] && previousCharacter == delimiter[0];
    }

    /// <summary>
    /// Splits the line using comma separated values (CSV) format
    /// </summary>
    /// <param name="instance">String instance to split</param>
    /// <param name="delimiter">Delimiter to use (most commonly , or ;)</param>
    /// <returns></returns>
    public static string[] SplitCsv(this string instance, char delimiter = ',')
    {
        if (string.IsNullOrEmpty(instance))
        {
            return Array.Empty<string>();
        }

        var fields = new List<string>();
        var inQuotes = false;
        var fieldStart = 0;

        for (var index = 0; index < instance.Length; index++)
        {
            if (instance[index] == '\"')
            {
                inQuotes = !inQuotes;
            }
            else if (instance[index] == delimiter && !inQuotes)
            {
                fields.Add(ParseCsvField(instance.Substring(fieldStart, index - fieldStart)));
                fieldStart = index + 1;
            }
        }

        fields.Add(ParseCsvField(instance.Substring(fieldStart)));

        return fields.ToArray();
    }

    private static string ParseCsvField(string field)
    {
        if (field.StartsWith("\"") && field.EndsWith("\""))
        {
            field = field.Substring(1, field.Length - 2);
        }

        return field.Replace("\"\"", "\"");
    }
}
