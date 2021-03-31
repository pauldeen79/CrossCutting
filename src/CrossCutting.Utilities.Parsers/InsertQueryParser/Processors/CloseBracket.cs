namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors
{
    public class CloseBracket : IInsertQueryParserProcessor
    {
        public ProcessResult Process(char character, InsertQueryParserState state)
        {
            if (character == ']' && !state.ValuesFound && !state.SelectFound)
            {
                state.OpenBracketCount--;
                if (state.OpenBracketCount < 0)
                {
                    return ProcessResult.Fail("Too many close brackets found");
                }

                return ProcessResult.Success();
            }

            return ProcessResult.NotUnderstood();
        }
    }
}
