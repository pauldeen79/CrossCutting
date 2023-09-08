namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors;

internal sealed class OpenRoundBracket : IInsertQueryParserProcessor
{
    public ProcessResult Process(char character, InsertQueryParserState state)
    {
        if (character == '(' &&
            (
                (state.ValuesFound || state.SelectFound) && !state.ValuesOpenBracketFound && state.OpenRoundBracketCount == 0 && state.ColumnValues.Count > 0
                || (state.ValuesOpenBracketFound && !state.ValuesCloseBracketFound && !state.InValue)
            ))
        {
            state.OpenRoundBracketCount++;
            state.CurrentSection.Append(character);

            return ProcessResult.Success();
        }

        return ProcessResult.NotUnderstood();
    }
}
