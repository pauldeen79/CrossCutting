namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors;

internal class NormalCharacter : IInsertQueryParserProcessor
{
    public ProcessResult Process(char character, InsertQueryParserState state)
    {
        if (character != '\r'
            && character != '\n'
            && character != '\t')
        {
            state.CurrentSection.Append(character);
            return ProcessResult.Success();
        }

        return ProcessResult.NotUnderstood();
    }
}
