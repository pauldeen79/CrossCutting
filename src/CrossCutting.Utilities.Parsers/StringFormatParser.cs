using System;
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
                return ParseResult.Error<string, object>("Format string is empty");
            }

            var state = new StringFormatParserState(args);

            foreach (var character in formatString)
            {
                if (character == '{')
                {
                    state.BeginPlaceholder();
                    if (state.OpenBracketCount > 1)
                    {
                        return ParseResult.Error<string, object>("Too many opening brackets found");
                    }
                    else
                    {
                        state.ClearCurrentSection();
                    }
                }
                else if (character == '}')
                {
                    state.EndPlaceholder();
                    if (state.OpenBracketCount < 0)
                    {
                        return ParseResult.Error<string, object>("Too many close brackets found");
                    }
                    else
                    {
                        state.ProcessCurrentSection();
                    }
                }
                else if (character != '\r' && character != '\n' && character != '\t')
                {
                    state.CurrentSection.Append(character);
                }
            }

            state.AddWarningsForMissingPlaceholders();

            //important: sort the placeholders! (i.e. Hello {1} {0} --> {0}, {1})
            state.SortPlaceholders();

            return state.GetResult();
        }

        public static ParseResult<string, object> ParseWithArgumentsString(string formatString, string argumentsString)
        {
            if (string.IsNullOrWhiteSpace(argumentsString))
            {
                return ParseResult.Error<string, object>("Arguments string is empty");
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

        private class StringFormatParserState
        {
            public StringBuilder CurrentSection { get; } = new StringBuilder();
            public int OpenBracketCount { get; private set; }
            public List<string> FormatPlaceholders { get; private set; } = new List<string>();
            public List<string> ValidationErrors { get; } = new List<string>();
            public List<object> FormatValues { get; }

            private bool FormatValueCountUnequalToFormatPlaceholderCount
                => FormatValues.Count > 0 && FormatPlaceholders.Count < FormatValues.Count;

            private bool FormatPlaceholderCountUnequalToFormatValueCount
                => FormatPlaceholders.Count > 0 && FormatValues.Count < FormatPlaceholders.Count;

            public StringFormatParserState(IEnumerable<object> args)
            {
                FormatValues = new List<object>(args.ToList());
            }

            public void BeginPlaceholder()
            {
                OpenBracketCount++;
            }

            public void EndPlaceholder()
            {
                OpenBracketCount--;
            }

            public void SortPlaceholders()
            {
                FormatPlaceholders = new List<string>(FormatPlaceholders.OrderBy(s => s).ToList());
            }

            public void ClearCurrentSection()
            {
                CurrentSection.Clear();
            }

            public void ProcessCurrentSection()
            {
                var name = CurrentSection.ToString();
                var doublePointSplit = name.Split(':');
                if (doublePointSplit.Length > 1)
                {
                    name = doublePointSplit[0];
                }
                if (!FormatPlaceholders.Contains(name))
                {
                    FormatPlaceholders.Add(name);
                }
                ClearCurrentSection();
            }

            public void AddWarningsForMissingPlaceholders()
            {
                for (int i = 0; i < FormatValues.Count; i++)
                {
                    if (!FormatPlaceholders.Contains(i.ToString()))
                    {
                        ValidationErrors.Add($"Warning: Format value {i} was not found in format placeholders");
                        FormatPlaceholders.Add(i.ToString());
                    }
                }
            }

            public ParseResult<string, object> GetResult()
            {
                if (FormatValueCountUnequalToFormatPlaceholderCount)
                {
                    var result = ParseResult.Error(ValidationErrors.Concat(new[] { $"Format values count ({FormatValues.Count}) is not equal to column placeholders count ({FormatPlaceholders.Count}), see #MISSING# in format placeholders list (keys)" }), FormatPlaceholders.Zip(FormatValues, (name, value) => new KeyValuePair<string, object>(name, value)));
                    FormatPlaceholders.AddRange(Enumerable.Range(1, FormatValues.Count - FormatPlaceholders.Count).Select(_ => "#MISSING#"));
                    return result;
                }
                else if (FormatPlaceholderCountUnequalToFormatValueCount)
                {
                    var result = ParseResult.Error(ValidationErrors.Concat(new[] { $"Format placeholders count ({FormatPlaceholders.Count}) is not equal to column values count ({FormatValues.Count}), see #MISSING# in format values list (values)" }), FormatPlaceholders.Zip(FormatValues, (name, value) => new KeyValuePair<string, object>(name, value)));
                    FormatValues.AddRange(Enumerable.Range(1, FormatPlaceholders.Count - FormatValues.Count).Select(_ => "#MISSING#"));
                    return result;
                }
                else if (FormatPlaceholders.Count == 0)
                {
                    return ParseResult.Error(ValidationErrors.Concat(new[] { "No format placeholders were found" }), Array.Empty<KeyValuePair<string, object>>());
                }
                else if (FormatValues.Count == 0)
                {
                    return ParseResult.Error(ValidationErrors.Concat(new[] { "No format values were found" }), Array.Empty<KeyValuePair<string, object>>());
                }

                return ParseResult.Create(ValidationErrors.Count == 0, FormatPlaceholders.Zip(FormatValues, (name, value) => new KeyValuePair<string, object>(name, value)), ValidationErrors);
            }
        }
    }
}
