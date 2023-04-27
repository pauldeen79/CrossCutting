namespace CrossCutting.Utilities.Parsers;

public record FunctionParseResult
{
    public FunctionParseResult(string functionName, IEnumerable<FunctionParseResultArgument> arguments, IFormatProvider formatProvider, object? context)
    {
        FunctionName = functionName;
        Arguments = new ReadOnlyValueCollection<FunctionParseResultArgument>(arguments);
        FormatProvider = formatProvider;
        Context = context;
    }

    public string FunctionName { get; }
    public ReadOnlyValueCollection<FunctionParseResultArgument> Arguments { get; }
    public IFormatProvider FormatProvider { get; }
    public object? Context { get; }
}
