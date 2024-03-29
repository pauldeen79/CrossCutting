﻿namespace CrossCutting.Utilities.Parsers.InsertQueryParser.Processors;

internal sealed class ValuesOpenBracket : IInsertQueryParserProcessor
{
    public ProcessResult Process(char character, InsertQueryParserState state)
    {
        if (character == '('
            && (state.ValuesFound || state.SelectFound)
            && !state.ValuesOpenBracketFound
            && state.OpenRoundBracketCount == 0
            && state.ColumnValues.Count == 0)
        {
            state.ValuesOpenBracketFound = true;
            state.CurrentSection.Clear();

            return ProcessResult.Success();
        }

        return ProcessResult.NotUnderstood();
    }
}
