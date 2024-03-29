﻿namespace CrossCutting.Utilities.Parsers.InsertQueryParser.ResultGenerators;

internal sealed class MissingColumnValues : IInsertQueryParserResultGenerator
{
    public ProcessResult Process(InsertQueryParserState state)
    {
        if (state.ColumnNames.Count > 0
            && state.ColumnValues.Count < state.ColumnNames.Count)
        {
            var result = ParseResult.Error
            (
                $"Column names count ({state.ColumnNames.Count}) is not equal to column values count ({state.ColumnValues.Count}), see #MISSING# in column values list (values)",
                state.ColumnNames.Zip(state.ColumnValues, (name, value) => new KeyValuePair<string, string>(name.Trim(), value.Trim()))
            );
            state.ColumnValues.AddRange(Enumerable.Range(1, state.ColumnNames.Count - state.ColumnValues.Count).Select(_ => "#MISSING#"));
            return ProcessResult.Fail(result);
        }

        return ProcessResult.NotUnderstood();
    }
}
