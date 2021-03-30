using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossCutting.Utilities.Parsers
{
    public static class InsertQueryParser
    {
        public static ParseResult<string, string> Parse(string insertQuery)
        {
            if (string.IsNullOrWhiteSpace(insertQuery))
            {
                return new ParseResult<string, string>(false, new[] { "Insert query is empty" }, Array.Empty<KeyValuePair<string, string>>());
            }

            var currentSection = new StringBuilder();
            var insertIntoFound = false;
            var valuesFound = false;
            var stopInsertInto = false;
            var valuesOpenBracketFound = false;
            var valuesCloseBracketFound = false;
            var selectFound = false;
            var fromFound = false;
            var insertIntoOpenBracketFound = false;
            var insertIntoCloseBracketFound = false;
            var inValue = false;
            var openBracketCount = 0;
            var openRoundBracketCount = 0;
            var columnNames = new List<string>();
            var columnValues = new List<string>();

            foreach (var character in insertQuery)
            {
                if (character == '[' && !valuesFound && !selectFound)
                {
                    openBracketCount++;
                }
                else if (character == ']' && !valuesFound && !selectFound)
                {
                    openBracketCount--;
                    if (openBracketCount < 0)
                    {
                        return new ParseResult<string, string>(false, new[] { "Too many close brackets found" }, Array.Empty<KeyValuePair<string, string>>());
                    }
                }
                else if (currentSection.ToString().EndsWith("INSERT INT", StringComparison.OrdinalIgnoreCase) && character.ToString().Equals("O", StringComparison.OrdinalIgnoreCase) && openBracketCount == 0)
                {
                    if (insertIntoFound)
                    {
                        return new ParseResult<string, string>(false, new[] { "INSERT INTO clause was found multiple times" }, Array.Empty<KeyValuePair<string, string>>());
                    }
                    else
                    {
                        insertIntoFound = true;
                    }
                    currentSection.Clear();
                }
                else if ((currentSection.ToString().EndsWith(" VALUE", StringComparison.OrdinalIgnoreCase) && character.ToString().Equals("S", StringComparison.OrdinalIgnoreCase) && insertIntoFound && !valuesFound && openBracketCount == 0)
                      || (currentSection.ToString().EndsWith(" OUTPU", StringComparison.OrdinalIgnoreCase) && character.ToString().Equals("T", StringComparison.OrdinalIgnoreCase) && insertIntoFound && !valuesFound && openBracketCount == 0))
                {
                    valuesFound = currentSection.ToString().EndsWith(" VALUE", StringComparison.OrdinalIgnoreCase) && character.ToString().Equals("S", StringComparison.OrdinalIgnoreCase) && insertIntoFound && !valuesFound && openBracketCount == 0;
                    stopInsertInto = currentSection.ToString().EndsWith(" OUTPU", StringComparison.OrdinalIgnoreCase) && character.ToString().Equals("T", StringComparison.OrdinalIgnoreCase) && insertIntoFound && !valuesFound && openBracketCount == 0;
                    currentSection.Clear();
                }
                else if (currentSection.ToString().EndsWith(" SELEC", StringComparison.OrdinalIgnoreCase) && character.ToString().Equals("T", StringComparison.OrdinalIgnoreCase) && insertIntoFound && !selectFound && openBracketCount == 0)
                {
                    selectFound = true;
                    currentSection.Clear();
                }
                else if (character == '(' && insertIntoFound && !valuesFound && !selectFound && !insertIntoOpenBracketFound)
                {
                    insertIntoOpenBracketFound = true;
                    currentSection.Clear();
                }
                else if (character == ')' && insertIntoFound && !valuesFound && !selectFound && insertIntoOpenBracketFound && !insertIntoCloseBracketFound)
                {
                    insertIntoCloseBracketFound = true;
                    columnNames.Add(currentSection.ToString());
                    currentSection.Clear();
                }
                else if (character == '(' && (valuesFound || selectFound) && !valuesOpenBracketFound && openRoundBracketCount == 0 && columnValues.Count == 0 /*&& new[] { "VALUES", "OUTPUT" }.Any(x => currentSection.ToString().Trim().Equals(x, StringComparison.InvariantCultureIgnoreCase))*/)
                {
                    valuesOpenBracketFound = true;
                    currentSection.Clear();
                }
                else if (character == '(' && (valuesFound || selectFound) && !valuesOpenBracketFound && openRoundBracketCount == 0 && columnValues.Count > 0)
                {
                    openRoundBracketCount++;
                    currentSection.Append(character);
                }
                else if (character == ')' && valuesOpenBracketFound && !valuesCloseBracketFound && openRoundBracketCount == 0)
                {
                    valuesCloseBracketFound = true;
                    columnValues.Add(currentSection.ToString());
                    currentSection.Clear();
                }
                else if (character == '(' && valuesOpenBracketFound && !valuesCloseBracketFound && !inValue)
                {
                    openRoundBracketCount++;
                    currentSection.Append(character);
                }
                else if (character == ')' && valuesOpenBracketFound && !valuesCloseBracketFound && !inValue)
                {
                    openRoundBracketCount--;
                    currentSection.Append(character);
                }
                else if (currentSection.ToString().EndsWith(" FRO", StringComparison.OrdinalIgnoreCase) && character.ToString().Equals("M", StringComparison.OrdinalIgnoreCase) && selectFound)
                {
                    fromFound = true;
                    var val = currentSection.ToString().Substring(0, currentSection.ToString().Length - 4);
                    columnValues.Add(val);
                    currentSection.Clear();
                }
                else if (character == ',' && insertIntoFound && !inValue && !stopInsertInto && openRoundBracketCount == 0)
                {
                    //part of INSERT INTO or VALUES
                    if (valuesOpenBracketFound && !valuesCloseBracketFound)
                    {
                        AddValue(currentSection, columnValues);
                    }
                    else if (insertIntoOpenBracketFound && !insertIntoCloseBracketFound)
                    {
                        AddColumnName(currentSection, columnNames);
                    }
                    else if (!fromFound)
                    {
                        //value
                        AddValue(currentSection, columnValues);
                    }
                }
                else if (character == '\'' && !inValue && openRoundBracketCount == 0)
                {
                    inValue = true;
                    currentSection.Append(character);
                }
                else if (character == '\'' && inValue && openRoundBracketCount == 0)
                {
                    inValue = false;
                    currentSection.Append(character);
                }
                else if (character != '\r' && character != '\n' && character != '\t')
                {
                    currentSection.Append(character);
                }
            }

            if (!insertIntoFound)
            {
                return new ParseResult<string, string>(false, new[] { "INSERT INTO clause was not found" }, Array.Empty<KeyValuePair<string, string>>());
            }

            if (!valuesFound && !selectFound)
            {
                return new ParseResult<string, string>(false, new[] { "VALUES or SELECT clause was not found" }, Array.Empty<KeyValuePair<string, string>>());
            }

            if (columnValues.Count > 0 && columnNames.Count < columnValues.Count)
            {
                var result = new ParseResult<string, string>(false, new[] { $"Column values count ({columnValues.Count}) is not equal to column names count ({columnNames.Count}), see #MISSING# in column names list (keys)" }, columnNames.Zip(columnValues, (name, value) => new KeyValuePair<string, string>(name.Trim(), value.Trim())));
                columnNames.AddRange(Enumerable.Range(1, columnValues.Count - columnNames.Count).Select(_ => "#MISSING#"));
                return result;
            }
            else if (columnNames.Count > 0 && columnValues.Count < columnNames.Count)
            {
                var result = new ParseResult<string, string>(false, new[] { $"Column names count ({columnNames.Count}) is not equal to column values count ({columnValues.Count}), see #MISSING# in column values list (values)" }, columnNames.Zip(columnValues, (name, value) => new KeyValuePair<string, string>(name.Trim(), value.Trim())));
                columnValues.AddRange(Enumerable.Range(1, columnNames.Count - columnValues.Count).Select(_ => "#MISSING#"));
                return result;
            }

            if (columnNames.Count == 0)
            {
                return new ParseResult<string, string>(false, new[] { "No column names were found" }, Array.Empty<KeyValuePair<string, string>>());
            }

            if (columnValues.Count == 0)
            {
                return new ParseResult<string, string>(false, new[] { "No column values were found" }, Array.Empty<KeyValuePair<string, string>>());
            }

            return new ParseResult<string, string>(true, Array.Empty<string>(), columnNames.Zip(columnValues, (name, value) => new KeyValuePair<string, string>(name.Trim(), value.Trim())));
        }

        private static void AddColumnName(StringBuilder currentSection, List<string> columnNames)
        {
            columnNames.Add(currentSection.ToString());
            currentSection.Clear();
        }

        private static void AddValue(StringBuilder currentSection, List<string> columnValues)
        {
            columnValues.Add(currentSection.ToString());
            currentSection.Clear();
        }

        public static string ToInsertIntoString(ParseResult<string, string> parseResult, string tableName)
            => parseResult.IsSuccessful
                ? string.Format
                (
                    "INSERT INTO [{0}]({1}) VALUES({2})",
                    tableName,
                    string.Join(", ", parseResult.Values.Select(kvp => kvp.Key.Contains(" ") ? $"[{kvp.Key}]" : kvp.Key)),
                    string.Join(", ", parseResult.Values.Select(kvp => kvp.Value))
                )
                : "Error: Parse result was not successful. Error messages: " + string.Join(Environment.NewLine, parseResult.ErrorMessages);
    }
}
