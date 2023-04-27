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

    public Result<object?> GetArgumentValue(int index, string argumentName, IFunctionParseResultEvaluator evaluator)
        => index + 1 > Arguments.Count
            ? Result<object?>.Invalid($"Missing argument: {argumentName}")
            : Arguments[index].GetValue(evaluator);

    public Result<int> GetArgumentValueInt32(int index, string argumentName, IFunctionParseResultEvaluator evaluator)
        => index + 1 > Arguments.Count
            ? Result<int>.Invalid($"Missing argument: {argumentName}")
            : Arguments[index].GetValueInt32(argumentName, FormatProvider, evaluator);

    public Result<string> GetArgumentValueString(int index, string argumentName, IFunctionParseResultEvaluator evaluator)
        => index + 1 > Arguments.Count
            ? Result<string>.Invalid($"Missing argument: {argumentName}")
            : Arguments[index].GetValueString(argumentName, FormatProvider, evaluator);

}
