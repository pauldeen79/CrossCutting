﻿namespace CrossCutting.Utilities.Parsers.InsertQueryParser.ResultGenerators;

internal sealed class InsertIntoNotFound : IInsertQueryParserResultGenerator
{
    public ProcessResult Process(InsertQueryParserState state)
    {
        if (!state.InsertIntoFound)
        {
            return ProcessResult.Fail("INSERT INTO clause was not found");
        }

        return ProcessResult.NotUnderstood();
    }
}
