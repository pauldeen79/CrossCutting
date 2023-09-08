namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors;

internal sealed class InsertInto : IInsertQueryParserProcessor
{
    public ProcessResult Process(char character, InsertQueryParserState state)
    {
        if (state.CurrentSection.ToString().EndsWith("INSERT INT", StringComparison.OrdinalIgnoreCase)
            && character.ToString().Equals("O", StringComparison.OrdinalIgnoreCase)
            && state.OpenBracketCount == 0)
        {
            if (state.InsertIntoFound)
            {
                return ProcessResult.Fail("INSERT INTO clause was found multiple times");
            }
            else
            {
                state.InsertIntoFound = true;
            }
            state.CurrentSection.Clear();
            return ProcessResult.Success();
        }

        return ProcessResult.NotUnderstood();
    }
}
