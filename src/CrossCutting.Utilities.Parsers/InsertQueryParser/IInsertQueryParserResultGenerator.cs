namespace CrossCutting.Utilities.Parsers.InsertQueryParser
{
    public interface IInsertQueryParserResultGenerator
    {
        ProcessResult Process(InsertQueryParserState state);
    }
}
