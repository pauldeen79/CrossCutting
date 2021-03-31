using System;

namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors
{
    public class From : IInsertQueryParserProcessor
    {
        public ProcessResult Process(char character, InsertQueryParserState state)
        {
            if (state.CurrentSection.ToString().EndsWith(" FRO", StringComparison.OrdinalIgnoreCase)
                && character.ToString().Equals("M", StringComparison.OrdinalIgnoreCase)
                && state.SelectFound)
            {
                state.FromFound = true;
                state.ColumnValues.Add(state.CurrentSection.ToString().Substring(0, state.CurrentSection.ToString().Length - 4));
                state.CurrentSection.Clear();

                return ProcessResult.Success();
            }

            return ProcessResult.NotUnderstood();
        }
    }
}
