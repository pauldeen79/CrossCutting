namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Abstractions;

internal interface IInsertQueryParserProcessor
{
    ProcessResult Process(char character, InsertQueryParserState state);
}
