﻿namespace CrossCutting.Utilities.Parsers;

public static class PipeDelimitedDataTableParser
{
    /// <summary>
    /// Parses the specified pipe-delimited data table entirely, without skipping lines or columns
    /// </summary>
    /// <param name="input">Data to parse. Each line contains one data item.</param>
    /// <returns>Parsed result</returns>
    public static IEnumerable<ParseResult<string, object>> Parse(string input)
        => Parse(input, 0, 0, 0, 0, null, null);

    /// <summary>
    /// Parses the specified pipe-delimited data table with advanced options
    /// </summary>
    /// <param name="input">Data to parse. Each line contains one data item.</param>
    /// <param name="skipLinesForData">Number of lines to skip from the top. When 0 is provided, no lines will be skipped.</param>
    /// <param name="skipColumnsLeft">Number of columns to skip at the left side of the data. When 0 is provided, no columns will be skipped.</param>
    /// <param name="skipColumnsRight">Number of columns to skip at the right side of the data. When 0 is provided, no columns will be skipped.</param>
    /// <param name="columnNamesLineNumber">Optional line number (one-based) in which column names are stored. When 0 is provided, no column names will be read from the input.</param>
    /// <param name="columnNames">Optional column names. When null is provided, column names will be generated. (Column 1...Column n)</param>
    /// <param name="transformFunction">Optional function for transforming values to desired output (first arument is column name, second is the value). When null is provided, no transformation will be performed, and data will probably contain leading and trailing spaces.</param>
    /// <returns>Parsed result</returns>
    public static IEnumerable<ParseResult<string, object>> Parse(
        string input,
        int skipLinesForData,
        int skipColumnsLeft,
        int skipColumnsRight,
        int columnNamesLineNumber,
        IEnumerable<string>? columnNames = null,
        Func<string, string, object>? transformFunction = null)
    {
        ArgumentGuard.IsNotNull(input, nameof(input));

        using (var sr = new StringReader(input))
        {
            string line;
            var lineNumber = 0;
            var columnNamesArray = columnNames?.ToArray();

            while ((line = sr.ReadLine()) is not null)
            {
                lineNumber++;

                if (lineNumber == columnNamesLineNumber)
                {
                    columnNamesArray = ParseLine(skipColumnsLeft, skipColumnsRight, transformFunction, line, columnNamesArray)
                        .Select((value, columnIndex) => value?.ToString() ?? $"{columnIndex + 1}")
                        .ToArray();
                }

                if (lineNumber <= skipLinesForData)
                {
                    continue;
                }

                var values = ParseLine(skipColumnsLeft, skipColumnsRight, transformFunction, line, columnNamesArray);
                var keyValuePairs = values.Select((value, index) => new KeyValuePair<string, object>(CreateColumnName(columnNamesArray, index + 1), value));

                yield return ParseResult.Success(keyValuePairs);
            }
        }
    }

    private static List<object> ParseLine(
        int skipColumnsLeft,
        int skipColumnsRight,
        Func<string, string, object>? formatFunction,
        string line,
        string[]? columnNamesArray)
    {
        var split = line.Split('|');
        var values = new List<object>();
        var columnIndex = 1;

        for (var column = skipColumnsLeft; column < split.Length - skipColumnsRight; column++)
        {
            var value = formatFunction is null
                ? split[column]
                : formatFunction(CreateColumnName(columnNamesArray, columnIndex), split[column]);

            values.Add(value);
            columnIndex++;
        }

        return values;
    }

    private static string CreateColumnName(string[]? columnNamesArray, int columnIndex)
        => columnNamesArray is null
            ? $"{columnIndex}"
            : columnNamesArray[columnIndex - 1] ?? $"{columnIndex}";
}
