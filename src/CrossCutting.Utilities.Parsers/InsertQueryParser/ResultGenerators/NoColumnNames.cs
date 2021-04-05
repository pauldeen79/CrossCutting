using CrossCutting.Utilities.Parsers.InsertQueryParser.Abstractions;

namespace CrossCutting.Utilities.Parsers.InsertQueryParser.ResultGenerators
{
    internal class NoColumnNames : IInsertQueryParserResultGenerator
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
