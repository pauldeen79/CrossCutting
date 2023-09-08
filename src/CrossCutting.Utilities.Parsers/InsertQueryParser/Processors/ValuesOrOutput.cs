namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors;

internal sealed class ValuesOrOutput : IInsertQueryParserProcessor
{
    public ProcessResult Process(char character, InsertQueryParserState state)
    {
        if ((state.CurrentSection.ToString().EndsWith(" VALUE", StringComparison.OrdinalIgnoreCase) && character.ToString().Equals("S", StringComparison.OrdinalIgnoreCase) && state.InsertIntoFound && !state.ValuesFound && state.OpenBracketCount == 0)
         || (state.CurrentSection.ToString().EndsWith(" OUTPU", StringComparison.OrdinalIgnoreCase) && character.ToString().Equals("T", StringComparison.OrdinalIgnoreCase) && state.InsertIntoFound && !state.ValuesFound && state.OpenBracketCount == 0))
        {
            state.ValuesFound = state.CurrentSection.ToString().EndsWith(" VALUE", StringComparison.OrdinalIgnoreCase) && character.ToString().Equals("S", StringComparison.OrdinalIgnoreCase) && state.InsertIntoFound && !state.ValuesFound;
            state.StopInsertInto = state.CurrentSection.ToString().EndsWith(" OUTPU", StringComparison.OrdinalIgnoreCase) && character.ToString().Equals("T", StringComparison.OrdinalIgnoreCase) && state.InsertIntoFound && !state.ValuesFound;
            state.CurrentSection.Clear();

            return ProcessResult.Success();
        }

        return ProcessResult.NotUnderstood();
    }
}
