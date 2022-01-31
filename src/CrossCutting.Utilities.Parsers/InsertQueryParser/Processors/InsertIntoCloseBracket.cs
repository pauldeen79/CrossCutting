namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors;

internal class InsertIntoCloseBracket : IInsertQueryParserProcessor
{
    public ProcessResult Process(char character, InsertQueryParserState state)
    {
        if (character == ')'
            && state.InsertIntoFound
            && !state.ValuesFound
            && !state.SelectFound
            && state.InsertIntoOpenBracketFound
            && !state.InsertIntoCloseBracketFound)
        {
            state.InsertIntoCloseBracketFound = true;
            state.ColumnNames.Add(state.CurrentSection.ToString());
            state.CurrentSection.Clear();

            return ProcessResult.Success();
        }

        return ProcessResult.NotUnderstood();
    }
}
