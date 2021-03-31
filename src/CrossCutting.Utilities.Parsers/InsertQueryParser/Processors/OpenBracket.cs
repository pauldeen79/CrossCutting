namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors
{
    public class OpenBracket : IInsertQueryParserProcessor
    {
        public ProcessResult Process(char character, InsertQueryParserState state)
        {
            if (character == '[' && !state.ValuesFound && !state.SelectFound)
            {
                state.OpenBracketCount++;
                return ProcessResult.Success();
            }

            return ProcessResult.NotUnderstood();
        }
    }
}
