namespace CrossCutting.Utilities.Parsers.InsertQueryParser;

public static class InsertQueryParser
{
    public static ParseResult<string, string> Parse(string insertQuery)
    {
        if (string.IsNullOrWhiteSpace(insertQuery))
        {
            return ParseResult.Error<string, string>("Insert query is empty");
        }

        return insertQuery.Aggregate(CreateInsertQueryParserState(), (x, character) => { x.Process(character); return x; })
                          .GetResult();
    }

    private static InsertQueryParserState CreateInsertQueryParserState()
        => new InsertQueryParserState(ComponentConfiguration.GetProcessors(), ComponentConfiguration.GetResultGenerators());

    public static string ToInsertIntoString(ParseResult<string, string> parseResult, string tableName)
        => parseResult.IsSuccessful
            ? $"INSERT INTO [{tableName}]({GetColumnNamees(parseResult)}) VALUES({GetValues(parseResult)})"
            : "Error: Parse result was not successful. Error messages: " + string.Join(Environment.NewLine, parseResult.ErrorMessages);

    private static string GetColumnNamees(ParseResult<string, string> parseResult)
        => string.Join(", ", parseResult.Values.Select(kvp => GetSqlName(kvp.Key)));

    private static string GetValues(ParseResult<string, string> parseResult)
        => string.Join(", ", parseResult.Values.Select(kvp => kvp.Value));

    private static string GetSqlName(string key)
        => key.Contains(" ")
            ? $"[{key}]"
            : key;

}
