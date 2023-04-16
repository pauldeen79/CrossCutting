namespace CrossCutting.Utilities.FunctionParser;

public record FunctionParseResult
{
    public FunctionParseResult(string functionName, IEnumerable<FunctionParseResultArgument> arguments)
    {
        FunctionName = functionName;
        Arguments = new ReadOnlyValueCollection<FunctionParseResultArgument>(arguments);
    }

    public string FunctionName { get; }
    public ReadOnlyValueCollection<FunctionParseResultArgument> Arguments { get; }
}
