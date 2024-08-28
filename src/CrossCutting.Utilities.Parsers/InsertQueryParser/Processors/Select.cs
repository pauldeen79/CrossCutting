namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors;

internal sealed class Select : IInsertQueryParserProcessor
{
    public ProcessResult Process(char character, InsertQueryParserState state)
    {
        if (state.CurrentSection.ToString().EndsWithAny(StringComparison.OrdinalIgnoreCase, " SELEC", "\tSELEC", "\nSELEC")
            && character.ToString().Equals("T", StringComparison.OrdinalIgnoreCase)
            && state.InsertIntoFound
            && !state.SelectFound
            && state.OpenBracketCount == 0)
        {
            state.SelectFound = true;
            state.CurrentSection.Clear();
            return ProcessResult.Success();
        }

        return ProcessResult.NotUnderstood();
    }
}
