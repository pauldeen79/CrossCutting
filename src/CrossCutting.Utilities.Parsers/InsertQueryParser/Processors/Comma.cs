namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors;

internal class Comma : IInsertQueryParserProcessor
{
    public ProcessResult Process(char character, InsertQueryParserState state)
    {
        if (character == ','
            && state.InsertIntoFound
            && !state.InValue
            && !state.StopInsertInto
            && state.OpenRoundBracketCount == 0)
        {
            //part of INSERT INTO or VALUES
            if (state.ValuesOpenBracketFound && !state.ValuesCloseBracketFound)
            {
                state.AddValue();
            }
            else if (state.InsertIntoOpenBracketFound && !state.InsertIntoCloseBracketFound)
            {
                state.AddColumnName();
            }
            else if (!state.FromFound)
            {
                //value
                state.AddValue();
            }

            return ProcessResult.Success();
        }

        return ProcessResult.NotUnderstood();
    }
}
