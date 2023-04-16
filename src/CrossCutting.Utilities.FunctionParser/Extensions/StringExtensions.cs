namespace CrossCutting.Utilities.FunctionParser.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Splits a string with a separator, with possibility to escape the separator if it's between open and close tokens.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="noSplitOpenToken">The no split open token.</param>
    /// <param name="noSplitCloseToken">The no split close token.</param>
    /// <returns>
    /// The split string.
    /// </returns>
    public static string[] SafeSplit(this string value, char separator, char noSplitOpenToken, char noSplitCloseToken)
    {
        var lst = new List<string>();
        var lastSeparatorPostition = -1;
        var nestedCount = 0;
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == separator && nestedCount == 0)
            {
                lst.Add(value.Substring(lastSeparatorPostition + 1, i - lastSeparatorPostition - 1));
                lastSeparatorPostition = i;
            }

            if (noSplitOpenToken == noSplitCloseToken)
            {
                if (value[i] == noSplitOpenToken)
                {
                    nestedCount = GetNextNestedCountForEqualSplitOpenAndCloseToken(nestedCount);
                }
            }
            else
            {
                nestedCount = GetNextNestedCountForUnequalSplitOpenAndCloseToken(nestedCount, value[i], noSplitOpenToken, noSplitCloseToken);
            }
        }

        AddRemainder(value, lst, lastSeparatorPostition);

        return lst.Select(x => RemoveQuotes(x, noSplitOpenToken, noSplitCloseToken)).ToArray();
    }

    private static int GetNextNestedCountForUnequalSplitOpenAndCloseToken(int nestedCount, char current, char noSplitOpenToken, char noSplitCloseToken)
        => current switch
        {
            var value when value == noSplitOpenToken => nestedCount + 1,
            var value when value == noSplitCloseToken => nestedCount - 1,
            _ => nestedCount
        };

    private static int GetNextNestedCountForEqualSplitOpenAndCloseToken(int nestedCount)
        => nestedCount == 0
            ? 1
            : 0;

    private static void AddRemainder(string value, List<string> lst, int lastSeparatorPostition)
    {
        if (lastSeparatorPostition == value.Length - 1 && value.Length > 0)
        {
            lst.Add(string.Empty);
        }
        else if (value.Length > 0)
        {
            lst.Add(value.Substring(lastSeparatorPostition + 1));
        }
    }

    private static string RemoveQuotes(string arg, char noSplitOpenToken, char noSplitCloseToken)
        => arg.StartsWith(noSplitOpenToken.ToString()) && arg.EndsWith(noSplitCloseToken.ToString())
            ? arg.Substring(1, arg.Length - 2)
            : arg;
}
