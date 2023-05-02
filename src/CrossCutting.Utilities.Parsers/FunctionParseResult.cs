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

    public Result<object?> GetArgumentValue(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator)
        => index + 1 > Arguments.Count
            ? Result<object?>.Invalid($"Missing argument: {argumentName}")
            : Arguments[index].GetValue(context, evaluator);

    public Result<object?> GetArgumentValue(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, object? defaultValue)
        => index + 1 > Arguments.Count
            ? Result<object?>.Success(defaultValue)
            : Arguments[index].GetValue(context, evaluator);

    public Result<string> GetArgumentStringValue(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator) => ProcessStringArgumentResult(argumentName, GetArgumentValue(index, argumentName, context, evaluator));

    public Result<string> GetArgumentStringValue(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, string defaultValue)
        => ProcessStringArgumentResult(argumentName, GetArgumentValue(index, argumentName, context, evaluator, defaultValue));

    public Result<int> GetArgumentInt32Value(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
        => ProcessInt32ArgumentResult(argumentName, parser, GetArgumentValue(index, argumentName, context, evaluator));

    public Result<int> GetArgumentInt32Value(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, int defaultValue)
        => ProcessInt32ArgumentResult(argumentName, parser, GetArgumentValue(index, argumentName, context, evaluator, defaultValue));

    public Result<long> GetArgumentInt64Value(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
        => ProcessInt64ArgumentResult(argumentName, parser, GetArgumentValue(index, argumentName, context, evaluator));

    public Result<long> GetArgumentInt64Value(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, long defaultValue)
        => ProcessInt64ArgumentResult(argumentName, parser, GetArgumentValue(index, argumentName, context, evaluator, defaultValue));

    public Result<decimal> GetArgumentDecimalValue(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
        => ProcessDecimalArgumentResult(argumentName, parser, GetArgumentValue(index, argumentName, context, evaluator));

    public Result<decimal> GetArgumentDecimalValue(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, decimal defaultValue)
        => ProcessDecimalArgumentResult(argumentName, parser, GetArgumentValue(index, argumentName, context, evaluator, defaultValue));

    public Result<bool> GetArgumentBooleanValue(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
        => ProcessBooleanArgumentResult(argumentName, parser, GetArgumentValue(index, argumentName, context, evaluator));

    public Result<bool> GetArgumentBooleanValue(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, bool defaultValue)
        => ProcessBooleanArgumentResult(argumentName, parser, GetArgumentValue(index, argumentName, context, evaluator, defaultValue));

    public Result<DateTime> GetArgumentDateTimeValue(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
        => ProcessDateTimeArgumentResult(argumentName, parser, GetArgumentValue(index, argumentName, context, evaluator));

    public Result<DateTime> GetArgumentDateTimeValue(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, DateTime defaultValue)
        => ProcessDateTimeArgumentResult(argumentName, parser, GetArgumentValue(index, argumentName, context, evaluator, defaultValue));

    private Result<int> ProcessInt32ArgumentResult(string argumentName, IExpressionParser parser, Result<object?> argumentValueResult)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return Result<int>.FromExistingResult(argumentValueResult);
        }

        if (argumentValueResult.Value is int i1)
        {
            return Result<int>.Success(i1);
        }

        if (argumentValueResult.Value is not string s)
        {
            return Result<int>.Invalid($"{argumentName} is not of type integer");
        }

        var parseResult = parser.Parse(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result<int>.Invalid($"{argumentName} is not of type integer");
        }

        return parseResult.Value is int i2
            ? Result<int>.Success(i2)
            : Result<int>.Invalid($"{argumentName} is not of type integer");
    }

    private Result<long> ProcessInt64ArgumentResult(string argumentName, IExpressionParser parser, Result<object?> argumentValueResult)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return Result<long>.FromExistingResult(argumentValueResult);
        }

        if (argumentValueResult.Value is long l1)
        {
            return Result<long>.Success(l1);
        }

        if (argumentValueResult.Value is not string s)
        {
            return Result<long>.Invalid($"{argumentName} is not of type long integer");
        }

        var parseResult = parser.Parse(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result<long>.Invalid($"{argumentName} is not of type long integer");
        }

        return parseResult.Value is long l2
            ? Result<long>.Success(l2)
            : Result<long>.Invalid($"{argumentName} is not of type long integer");
    }

    private Result<decimal> ProcessDecimalArgumentResult(string argumentName, IExpressionParser parser, Result<object?> argumentValueResult)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return Result<decimal>.FromExistingResult(argumentValueResult);
        }

        if (argumentValueResult.Value is decimal d1)
        {
            return Result<decimal>.Success(d1);
        }

        if (argumentValueResult.Value is not string s)
        {
            return Result<decimal>.Invalid($"{argumentName} is not of type decimal");
        }

        var parseResult = parser.Parse(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result<decimal>.Invalid($"{argumentName} is not of type decimal");
        }

        return parseResult.Value is decimal d2
            ? Result<decimal>.Success(d2)
            : Result<decimal>.Invalid($"{argumentName} is not of type decimal");
    }

    private Result<bool> ProcessBooleanArgumentResult(string argumentName, IExpressionParser parser, Result<object?> argumentValueResult)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return Result<bool>.FromExistingResult(argumentValueResult);
        }

        if (argumentValueResult.Value is bool b1)
        {
            return Result<bool>.Success(b1);
        }

        if (argumentValueResult.Value is not string s)
        {
            return Result<bool>.Invalid($"{argumentName} is not of type boolean");
        }

        var parseResult = parser.Parse(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result<bool>.Invalid($"{argumentName} is not of type boolean");
        }

        return parseResult.Value is bool b2
            ? Result<bool>.Success(b2)
            : Result<bool>.Invalid($"{argumentName} is not of type boolean");
    }

    private Result<DateTime> ProcessDateTimeArgumentResult(string argumentName, IExpressionParser parser, Result<object?> argumentValueResult)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return Result<DateTime>.FromExistingResult(argumentValueResult);
        }

        if (argumentValueResult.Value is DateTime dt1)
        {
            return Result<DateTime>.Success(dt1);
        }

        if (argumentValueResult.Value is not string s)
        {
            return Result<DateTime>.Invalid($"{argumentName} is not of type datetime");
        }
        var parseResult = parser.Parse(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result<DateTime>.Invalid($"{argumentName} is not of type datetime");
        }

        return parseResult.Value is DateTime dt2
            ? Result<DateTime>.Success(dt2)
            : Result<DateTime>.Invalid($"{argumentName} is not of type datetime");
    }

    private static Result<string> ProcessStringArgumentResult(string argumentName, Result<object?> argumentValueResult)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return Result<string>.FromExistingResult(argumentValueResult);
        }

        return argumentValueResult.Value is string stringValue
            ? Result<string>.Success(stringValue)
            : Result<string>.Invalid($"{argumentName} is not of type string");
    }
}
