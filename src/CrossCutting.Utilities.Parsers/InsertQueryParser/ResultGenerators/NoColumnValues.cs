using CrossCutting.Utilities.Parsers.InsertQueryParser.Abstractions;

namespace CrossCutting.Utilities.Parsers.InsertQueryParser.ResultGenerators
{
    internal class NoColumnValues : IInsertQueryParserResultGenerator
    {
        public ProcessResult Process(InsertQueryParserState state)
        {
            if (state.ColumnValues.Count == 0)
            {
                return ProcessResult.Fail("No column values were found");
            }

            return ProcessResult.NotUnderstood();
        }
    }
}
