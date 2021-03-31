using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Utilities.Parsers.InsertQueryParser.ResultGenerators
{
    public class Ok : IInsertQueryParserResultGenerator
    {
        public ProcessResult Process(InsertQueryParserState state)
        {
            var values = state.ColumnNames.Zip(state.ColumnValues, (name, value) => new KeyValuePair<string, string>(name.Trim(), value.Trim()));
            return ProcessResult.Success
            (
                new ParseResult<string, string>
                (
                    isSuccessful: true,
                    errorMessages: Array.Empty<string>(),
                    values: values
                )
            );
        }
    }
}
