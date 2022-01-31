namespace CrossCutting.Utilities.Parsers.InsertQueryParser.ResultGenerators;

internal class Ok : IInsertQueryParserResultGenerator
{
    public ProcessResult Process(InsertQueryParserState state)
        => ProcessResult.Success(ParseResult.Success(state.ColumnNames
            .Zip(state.ColumnValues, (name, value) => new KeyValuePair<string, string>(name.Trim(), value.Trim()))));
}
