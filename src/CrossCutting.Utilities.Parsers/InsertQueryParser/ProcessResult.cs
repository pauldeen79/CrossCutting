using System;
using System.Collections.Generic;

namespace CrossCutting.Utilities.Parsers.InsertQueryParser
{
    internal class ProcessResult
    {
        internal bool Understood { get; }
        internal ParseResult<string, string> Outcome { get; }

        internal ProcessResult(bool understood, ParseResult<string, string> error)
        {
            Understood = understood;
            Outcome = error;
        }

        internal static ProcessResult Success(ParseResult<string, string> outcome = null) => new ProcessResult(true, outcome);

        internal static ProcessResult Fail(string errorMessage) => new ProcessResult(true, new ParseResult<string, string>(false, new[] { errorMessage }, Array.Empty<KeyValuePair<string, string>>()));

        internal static ProcessResult Fail(ParseResult<string, string> result) => new ProcessResult(true, result);

        internal static ProcessResult NotUnderstood() => new ProcessResult(false, null);
    }
}
