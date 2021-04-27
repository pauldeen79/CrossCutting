using CrossCutting.Utilities.Parsers.InsertQueryParser.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Utilities.Parsers.InsertQueryParser.ResultGenerators
{
    internal class MissingColumnNames : IInsertQueryParserResultGenerator
    {
        public ProcessResult Process(InsertQueryParserState state)
        {
            if (state.ColumnValues.Count > 0
                && state.ColumnNames.Count < state.ColumnValues.Count)
            {
                var result = ParseResult.Error
                (
                    $"Column values count ({state.ColumnValues.Count}) is not equal to column names count ({state.ColumnNames.Count}), see #MISSING# in column names list (keys)",
                    state.ColumnNames.Zip(state.ColumnValues, (name, value) => new KeyValuePair<string, string>(name.Trim(), value.Trim()))
                );
                state.ColumnNames.AddRange(Enumerable.Range(1, state.ColumnValues.Count - state.ColumnNames.Count).Select(_ => "#MISSING#"));
                return ProcessResult.Fail(result);
            }

            return ProcessResult.NotUnderstood();
        }
    }
}
