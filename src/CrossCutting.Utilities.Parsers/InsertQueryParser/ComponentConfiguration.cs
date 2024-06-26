﻿namespace CrossCutting.Utilities.Parsers.InsertQueryParser
{
    internal static class ComponentConfiguration
    {
        internal static IInsertQueryParserResultGenerator[] GetResultGenerators()
            =>
            [
                new InsertIntoNotFound(),
                new ValuesOrSelectClauseNotFound(),
                new MissingColumnNames(),
                new MissingColumnValues(),
                new NoColumnNames(),
                new NoColumnValues(),
                new Ok()
            ];

        internal static IInsertQueryParserProcessor[] GetProcessors()
            =>
            [
                new OpenBracket(),
                new CloseBracket(),
                new InsertInto(),
                new ValuesOrOutput(),
                new Select(),
                new InsertIntoOpenBracket(),
                new InsertIntoCloseBracket(),
                new ValuesOpenBracket(),
                new OpenRoundBracket(),
                new ValuesCloseBracket(),
                new CloseRoundBracket(),
                new From(),
                new Comma(),
                new Quote(),
                new NormalCharacter()
            ];
    }
}
