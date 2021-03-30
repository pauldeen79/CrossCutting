using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossCutting.Utilities.Parsers
{
    public static class StringFormatParser
    {
        public static ParseResult<string, object> Parse(string formatString, params object[] args)
            => Parse(formatString, (IEnumerable<object>)args);

        public static ParseResult<string, object> Parse(string formatString, IEnumerable<object> args)
        {
            if (string.IsNullOrWhiteSpace(formatString))
            {
                return new ParseResult<string, object>(false, new[] { "Format string is empty" }, System.Array.Empty<KeyValuePair<string, object>>());
            }

            var currentSection = new StringBuilder();
            var openBracketCount = 0;
            var formatPlaceholders = new List<string>();
            var validationErrors = new List<string>();
            var formatValues = new List<object>(args.ToList());

            foreach (var character in formatString)
            {
                if (character == '{')
                {
                    openBracketCount++;
                    if (openBracketCount > 1)
                    {
                        return new ParseResult<string, object>(false, new[] { "Too many opening brackets found" }, System.Array.Empty<KeyValuePair<string, object>>());
                    }
                    else
                    {
                        currentSection.Clear();
                    }
                }
                else if (character == '}')
                {
                    openBracketCount--;
                    if (openBracketCount < 0)
                    {
                        return new ParseResult<string, object>(false, new[] { "Too many close brackets found" }, System.Array.Empty<KeyValuePair<string, object>>());
                    }
                    else
                    {
                        var name = currentSection.ToString();
                        var doublePointSplit = name.Split(':');
                        if (doublePointSplit.Length > 1)
                        {
                            name = doublePointSplit[0];
                        }
                        if (!formatPlaceholders.Contains(name))
                        {
                            formatPlaceholders.Add(name);
                        }
                        currentSection.Clear();
                    }
                }
                else if (character != '\r' && character != '\n' && character != '\t')
                {
                    currentSection.Append(character);
                }
            }

            for (int i = 0; i < formatValues.Count; i++)
            {
                if (!formatPlaceholders.Contains(i.ToString()))
                {
                    validationErrors.Add($"Warning: Format value {i} was not found in format placeholders");
                    formatPlaceholders.Add(i.ToString());
                }
            }

            //important: sort the placeholders! (i.e. Hello {1} {0} --> {0}, {1})
            formatPlaceholders = new List<string>(formatPlaceholders.OrderBy(s => s).ToList());

            if (formatValues.Count > 0 && formatPlaceholders.Count < formatValues.Count)
            {
                var result = new ParseResult<string, object>(false, validationErrors.Concat(new[] { $"Format values count ({formatValues.Count}) is not equal to column placeholders count ({formatPlaceholders.Count}), see #MISSING# in format placeholders list (keys)" }), formatPlaceholders.Zip(formatValues, (name, value) => new KeyValuePair<string, object>(name, value)));
                formatPlaceholders.AddRange(Enumerable.Range(1, formatValues.Count - formatPlaceholders.Count).Select(_ => "#MISSING#"));
                return result;
            }
            else if (formatPlaceholders.Count > 0 && formatValues.Count < formatPlaceholders.Count)
            {
                var result = new ParseResult<string, object>(false, validationErrors.Concat(new[] { $"Format placeholders count ({formatPlaceholders.Count}) is not equal to column values count ({formatValues.Count}), see #MISSING# in format values list (values)" }), formatPlaceholders.Zip(formatValues, (name, value) => new KeyValuePair<string, object>(name, value)));
                formatValues.AddRange(Enumerable.Range(1, formatPlaceholders.Count - formatValues.Count).Select(_ => "#MISSING#"));
                return result;
            }

            if (formatPlaceholders.Count == 0)
            {
                return new ParseResult<string, object>(false, validationErrors.Concat(new[] { "No format placeholders were found" }), System.Array.Empty<KeyValuePair<string, object>>());
            }

            if (formatValues.Count == 0)
            {
                return new ParseResult<string, object>(false, validationErrors.Concat(new[] { "No format values were found" }), System.Array.Empty<KeyValuePair<string, object>>());
            }

            return new ParseResult<string, object>(validationErrors.Count == 0, validationErrors, formatPlaceholders.Zip(formatValues, (name, value) => new KeyValuePair<string, object>(name, value)));
        }

        public static ParseResult<string, object> ParseWithArgumentsString(string formatString, string argumentsString)
        {
            if (string.IsNullOrWhiteSpace(argumentsString))
            {
                return new ParseResult<string, object>(false, new[] { "Arguments string is empty" }, System.Array.Empty<KeyValuePair<string, object>>());
            }

            var currentSection = new StringBuilder();
            var inValue = false;
            var arguments = new List<string>();

            foreach (var character in argumentsString)
            {
                if (character == ',' && !inValue)
                {
                    arguments.Add(currentSection.ToString().Trim());
                    currentSection.Clear();
                }
                else if (character == '"' && !inValue)
                {
                    inValue = true;
                    currentSection.Append(character);
                }
                else if (character == '"' && inValue)
                {
                    inValue = false;
                    currentSection.Append(character);
                }
                else if (inValue || (character != '\r' && character != '\n' && character != '\t'))
                {
                    currentSection.Append(character);
                }
            }

            if (currentSection.Length > 0)
            {
                arguments.Add(currentSection.ToString().Trim());
                currentSection.Clear();
            }

            return Parse(formatString, arguments.ToArray());
        }
    }
}
