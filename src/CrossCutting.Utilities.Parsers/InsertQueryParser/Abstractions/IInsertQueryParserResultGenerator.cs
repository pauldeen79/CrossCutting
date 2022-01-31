namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Abstractions;

internal interface IInsertQueryParserResultGenerator
{
    ProcessResult Process(InsertQueryParserState state);
}
