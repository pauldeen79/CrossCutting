namespace CrossCutting.Common.Extensions;

public static class StringExtensions
{
    private static readonly string[] _trueKeywords = ["true", "t", "1", "y", "yes", "ja", "j"];
    private static readonly string[] _falseKeywords = ["false", "f", "0", "n", "no", "nee"];

    /// <summary>
    /// Performs a is null or empty check, and returns another value when this evaluates to true.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="whenNullOrEmpty">The when null or empty.</param>
    /// <returns></returns>
    public static string WhenNullOrEmpty(this string? instance, string whenNullOrEmpty)
    {
        if (instance is null || string.IsNullOrEmpty(instance))
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
        ArgumentGuard.IsNotNull(whenNullOrEmptyDelegate, nameof(whenNullOrEmptyDelegate));

        if (instance is null || string.IsNullOrEmpty(instance))
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
        if (instance is null || string.IsNullOrWhiteSpace(instance))
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
        ArgumentGuard.IsNotNull(whenNullOrWhiteSpaceDelegate, nameof(whenNullOrWhiteSpaceDelegate));

        if (instance is null || string.IsNullOrWhiteSpace(instance))
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
        => instance is not null
            && Array.Exists(_trueKeywords, s => s.Equals(instance, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Determines whether the specified instance is false.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <returns></returns>
    public static bool IsFalse(this string? instance)
        => instance is not null
            && Array.Exists(_falseKeywords, s => s.Equals(instance, StringComparison.OrdinalIgnoreCase));

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
            while ((line = reader.ReadLine()) is not null)
            {
                result.Add(line);
            }
        }
        return [.. result];
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
        ArgumentGuard.IsNotNull(delimiter, nameof(delimiter));

        if (delimiter.Length is not 1 and not 2)
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
            else if (textQualifier is not null && character == textQualifier)
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

    public static string[] SplitDelimited(this string instance, string[] delimiters, char? textQualifier = null, bool leaveTextQualifier = false, bool trimItems = false, bool addDelimiters = false, StringComparison comparisonType = StringComparison.CurrentCulture)
    {
        ArgumentGuard.IsNotNull(delimiters, nameof(delimiters));

        if (string.IsNullOrEmpty(instance))
        {
            return Array.Empty<string>();
        }

        var results = new List<string>();
        var currentSegment = new List<char>();
        bool insideQualifier = false;
        char qualifierChar = textQualifier ?? '\0';

        for (int i = 0; i < instance.Length; i++)
        {
            char c = instance[i];

            // Toggle insideQualifier when encountering a text qualifier
            if (textQualifier.HasValue && c == qualifierChar)
            {
                insideQualifier = !insideQualifier;
                if (!leaveTextQualifier)
                {
                    continue; // Skip the qualifier if removal is enabled
                }
            }

            // If outside qualifier, check for multi-character delimiters
            if (!insideQualifier)
            {
                foreach (var delimiter in from string delimiter in delimiters
                                          where instance.Substring(i).StartsWith(delimiter, comparisonType)
                                          select delimiter)
                {
                    AddCurrentSegmentToResults(trimItems, addDelimiters, results, currentSegment, delimiter);

#pragma warning disable S127 // "for" loop stop conditions should be invariant
                    i += delimiter.Length - 1;// Skip the delimiter
#pragma warning restore S127 // "for" loop stop conditions should be invariant
#pragma warning disable S907 // "goto" statement should not be used
                    goto NextChar;// Move to next character
#pragma warning restore S907 // "goto" statement should not be used
                }
            }

            // Add character to current segment
            currentSegment.Add(c);

#pragma warning disable S1116 // Empty statements should be removed
NextChar:;
#pragma warning restore S1116 // Empty statements should be removed
        }

        AddLastSegmentIfNotEmpty(trimItems, results, currentSegment);

        return results.ToArray();
    }

    private static void AddLastSegmentIfNotEmpty(bool trimItems, List<string> results, List<char> currentSegment)
    {
        if (currentSegment.Count <= 0)
        {
            return;
        }
        
        results.Add(GetResultItem(trimItems, currentSegment));
    }

    private static void AddCurrentSegmentToResults(bool trimItems, bool addDelimiters, List<string> results, List<char> currentSegment, string delimiter)
    {
        if (currentSegment.Count <= 0)
        {
            return;
        }

        results.Add(GetResultItem(trimItems, currentSegment));

        if (addDelimiters)
        {
            results.Add(delimiter);
        }

        currentSegment.Clear();
    }

    private static string GetResultItem(bool trimItems, List<char> currentSegment)
        => trimItems
            ? new string([.. currentSegment]).Trim()
            : new string([.. currentSegment]);

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

        if (previousCharacter is null)
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
            return [];
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

        return [.. fields];
    }

    private static string ParseCsvField(string field)
    {
        if (field.StartsWith("\"") && field.EndsWith("\""))
        {
            field = field.Substring(1, field.Length - 2);
        }

        return field.Replace("\"\"", "\"");
    }

    public static string ToPascalCase(this string value, CultureInfo cultureInfo)
        => string.IsNullOrEmpty(value)
            ? value
            : string.Concat(value.Substring(0, 1).ToUpper(cultureInfo), value.Substring(1));

    public static string ToCamelCase(this string value, CultureInfo cultureInfo)
        => string.IsNullOrEmpty(value)
            ? value
            : string.Concat(value.Substring(0, 1).ToLower(cultureInfo), value.Substring(1));

    public static string ReplaceSuffix(this string instance, string find, string replace, StringComparison comparisonType)
    {
        find = find.IsNotNull(nameof(find));

        var index = instance.LastIndexOf(find, comparisonType);
        if (index == -1 || index < instance.Length - find.Length)
        {
            return instance;
        }

        return instance.Substring(0, instance.Length - find.Length) + replace;
    }

    public static IEnumerable<int> FindAllOccurences(this string instance, char characterToFind)
    {
        int index = -1;

        do
        {
            if (index + 1 >= instance.Length)
            {
                break;
            }

            index = instance.IndexOf(characterToFind, index + 1);
            
            if (index == -1)
            {
                break;
            }
            
            yield return index;

        } while (true);
    }

    public static IEnumerable<int> FindAllOccurences(this string instance, string stringToFind, StringComparison comparisonType)
    {
        int index = -1;

        do
        {
            if (index + 1 >= instance.Length)
            {
                break;
            }

            index = instance.IndexOf(stringToFind, index + 1, comparisonType);

            if (index == -1)
            {
                break;
            }

            yield return index;

        } while (true);
    }
}
