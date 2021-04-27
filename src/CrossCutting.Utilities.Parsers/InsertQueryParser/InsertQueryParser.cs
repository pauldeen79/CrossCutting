using System;
using System.Linq;

namespace CrossCutting.Utilities.Parsers.InsertQueryParser
{
    public static class InsertQueryParser
    {
        public static ParseResult<string, string> Parse(string insertQuery)
        {
            if (string.IsNullOrWhiteSpace(insertQuery))
            {
                return ParseResult.Error<string, string>("Insert query is empty");
            }

            var processors = ComponentConfiguration.GetProcessors();
            var resultGenerators = ComponentConfiguration.GetResultGenerators();
            var state = new InsertQueryParserState(processors, resultGenerators);

            foreach (var character in insertQuery)
            {
                state.Process(character);
            }

            return state.GetResult();
        }

        public static string ToInsertIntoString(ParseResult<string, string> parseResult, string tableName)
            => parseResult.IsSuccessful
                ? string.Format
                (
                    "INSERT INTO [{0}]({1}) VALUES({2})",
                    tableName,
                    string.Join(", ", parseResult.Values.Select(kvp => GetSqlName(kvp.Key))),
                    string.Join(", ", parseResult.Values.Select(kvp => kvp.Value))
                )
                : "Error: Parse result was not successful. Error messages: " + string.Join(Environment.NewLine, parseResult.ErrorMessages);

        private static string GetSqlName(string key)
            => key.Contains(" ")
                ? $"[{key}]"
                : key;

    }
}
