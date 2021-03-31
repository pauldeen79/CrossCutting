namespace CrossCutting.Utilities.Parsers.InsertQueryParser
{
    public interface IInsertQueryParserProcessor
    {
        ProcessResult Process(char character, InsertQueryParserState state);
    }
}
