namespace CrossCutting.Utilities.Parsers.InsertQueryParser;

internal sealed class ProcessResult
{
    internal bool Understood { get; }
    internal ParseResult<string, string> Outcome { get; }

    internal ProcessResult(bool understood, ParseResult<string, string> error)
    {
        Understood = understood;
        Outcome = error;
    }

    internal static ProcessResult Success()
        => new(true, ParseResult.Success<string, string>());

    internal static ProcessResult Success(ParseResult<string, string> outcome)
        => new(true, outcome);

    internal static ProcessResult Fail(string errorMessage)
        => new(true, ParseResult.Error<string, string>(errorMessage));

    internal static ProcessResult Fail(ParseResult<string, string> result)
        => new(true, result);

    internal static ProcessResult NotUnderstood()
        => new(false, ParseResult.NotUnderstood<string, string>());
}
