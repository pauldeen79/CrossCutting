namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors
{
    public class Quote : IInsertQueryParserProcessor
    {
        public ProcessResult Process(char character, InsertQueryParserState state)
        {
            if (character == '\'' &&
                !state.InValue
                && state.OpenRoundBracketCount == 0)
            {
                state.InValue = true;
                state.CurrentSection.Append(character);

                return ProcessResult.Success();
            }
            else if (character == '\''
                && state.InValue
                && state.OpenRoundBracketCount == 0)
            {
                state.InValue = false;
                state.CurrentSection.Append(character);

                return ProcessResult.Success();
            }

            return ProcessResult.NotUnderstood();
        }
    }
}
