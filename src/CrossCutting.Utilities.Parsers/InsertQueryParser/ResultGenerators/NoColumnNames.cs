namespace CrossCutting.Utilities.Parsers.InsertQueryParser.ResultGenerators
{
    public class NoColumnNames : IInsertQueryParserResultGenerator
    {
        public ProcessResult Process(InsertQueryParserState state)
        {
            if (state.ColumnNames.Count == 0)
            {
                return ProcessResult.Fail("No column names were found");
            }

            return ProcessResult.NotUnderstood();
        }
    }
}
