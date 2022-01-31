namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors;

internal class CloseRoundBracket : IInsertQueryParserProcessor
{
    public ProcessResult Process(char character, InsertQueryParserState state)
    {
        if (character == ')'
            && state.ValuesOpenBracketFound
            && !state.ValuesCloseBracketFound
            && !state.InValue)
        {
            state.OpenRoundBracketCount--;
            state.CurrentSection.Append(character);

            return ProcessResult.Success();
        }

        return ProcessResult.NotUnderstood();
    }
}
