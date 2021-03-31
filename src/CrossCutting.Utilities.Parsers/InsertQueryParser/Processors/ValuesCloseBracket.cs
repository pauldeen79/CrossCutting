namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors
{
    public class ValuesCloseBracket : IInsertQueryParserProcessor
    {
        public ProcessResult Process(char character, InsertQueryParserState state)
        {
            if (character == ')'
                && state.ValuesOpenBracketFound
                && !state.ValuesCloseBracketFound
                && state.OpenRoundBracketCount == 0)
            {
                state.ValuesCloseBracketFound = true;
                state.ColumnValues.Add(state.CurrentSection.ToString());
                state.CurrentSection.Clear();

                return ProcessResult.Success();
            }

            return ProcessResult.NotUnderstood();
        }
    }
}
