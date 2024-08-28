namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors;

internal sealed class NormalCharacter : IInsertQueryParserProcessor
{
    public ProcessResult Process(char character, InsertQueryParserState state)
    {
        state.CurrentSection.Append(character);
        return ProcessResult.Success();
    }
}
