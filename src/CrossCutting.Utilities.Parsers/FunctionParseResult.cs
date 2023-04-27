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

    public Result<string> GetArgumentStringValue(int index, string argumentName, IFunctionParseResultEvaluator evaluator)
    {
        var argumentValueResult = GetArgumentValue(index, argumentName, evaluator);
        if (!argumentValueResult.IsSuccessful())
        {
            return Result<string>.FromExistingResult(argumentValueResult);
        }

        return argumentValueResult.Value is string stringValue
            ? Result<string>.Success(stringValue)
            : Result<string>.Invalid($"{argumentName} is not of type string");
    }

    public Result<int> GetArgumentInt32Value(int index, string argumentName, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
    {
        var argumentValueResult = GetArgumentValue(index, argumentName, evaluator);
        if (argumentValueResult.Value is int i1)
        {
            return Result<int>.Success(i1);
        }
        else if (argumentValueResult.Value is string s)
        {
            var parseResult = parser.Parse(s, FormatProvider, Context);
            if (!parseResult.IsSuccessful())
            {
                return Result<int>.Invalid($"{argumentName} is not of type integer");
            }
            else if (parseResult.Value is int i2)
            {
                return Result<int>.Success(i2);
            }
            else
            {
                return Result<int>.Invalid($"{argumentName} is not of type integer");
            }
        }
        else
        {
            return Result<int>.Invalid($"{argumentName} is not of type integer");
        }
    }
}
