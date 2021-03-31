using System;
using System.Collections.Generic;

namespace CrossCutting.Utilities.Parsers.InsertQueryParser
{
    public class ProcessResult
    {
        public bool Understood { get; }
        public ParseResult<string, string> Outcome { get; }

        public ProcessResult(bool understood, ParseResult<string, string> error)
        {
            Understood = understood;
            Outcome = error;
        }

        public static ProcessResult Success(ParseResult<string, string> outcome = null) => new ProcessResult(true, outcome);

        public static ProcessResult Fail(string errorMessage) => new ProcessResult(true, new ParseResult<string, string>(false, new[] { errorMessage }, Array.Empty<KeyValuePair<string, string>>()));

        public static ProcessResult Fail(ParseResult<string, string> result) => new ProcessResult(true, result);

        public static ProcessResult NotUnderstood() => new ProcessResult(false, null);
    }
}
