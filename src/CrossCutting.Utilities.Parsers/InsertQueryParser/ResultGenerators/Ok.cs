using System.Collections.Generic;
using System.Linq;
using CrossCutting.Utilities.Parsers.InsertQueryParser.Abstractions;

namespace CrossCutting.Utilities.Parsers.InsertQueryParser.ResultGenerators
{
    internal class Ok : IInsertQueryParserResultGenerator
    {
        public ProcessResult Process(InsertQueryParserState state)
        {
            var values = state.ColumnNames.Zip(state.ColumnValues, (name, value) => new KeyValuePair<string, string>(name.Trim(), value.Trim()));
            return ProcessResult.Success(ParseResult.Success(values));
        }
    }
}
