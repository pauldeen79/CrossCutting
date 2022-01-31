namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors;

internal class InsertIntoOpenBracket : IInsertQueryParserProcessor
{
    public ProcessResult Process(char character, InsertQueryParserState state)
    {
        if (character == '('
            && state.InsertIntoFound
            && !state.ValuesFound
            && !state.SelectFound
            && !state.InsertIntoOpenBracketFound)
        {
            state.InsertIntoOpenBracketFound = true;
            state.CurrentSection.Clear();

            return ProcessResult.Success();
        }

        return ProcessResult.NotUnderstood();
    }
}
