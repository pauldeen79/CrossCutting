namespace CrossCutting.Utilities.Parsers.InsertQueryParser.ResultGenerators;

internal sealed class ValuesOrSelectClauseNotFound : IInsertQueryParserResultGenerator
{
    public ProcessResult Process(InsertQueryParserState state)
    {
        if (!state.ValuesFound
            && !state.SelectFound)
        {
            return ProcessResult.Fail("VALUES or SELECT clause was not found");
        }

        return ProcessResult.NotUnderstood();
    }
}
