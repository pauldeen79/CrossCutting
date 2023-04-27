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

    public Result<FunctionParseResultArgument> GetArgument(int index, string argumentName)
        => index + 1 > Arguments.Count
            ? Result<FunctionParseResultArgument>.Invalid($"Missing argument: {argumentName}")
            : Result<FunctionParseResultArgument>.Success(Arguments[index]);
}
