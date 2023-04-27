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

    public Result<long> GetArgumentInt64Value(int index, string argumentName, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
    {
        var argumentValueResult = GetArgumentValue(index, argumentName, evaluator);
        if (argumentValueResult.Value is long l1)
        {
            return Result<long>.Success(l1);
        }
        else if (argumentValueResult.Value is string s)
        {
            var parseResult = parser.Parse(s, FormatProvider, Context);
            if (!parseResult.IsSuccessful())
            {
                return Result<long>.Invalid($"{argumentName} is not of type long integer");
            }
            else if (parseResult.Value is long l2)
            {
                return Result<long>.Success(l2);
            }
            else
            {
                return Result<long>.Invalid($"{argumentName} is not of type long integer");
            }
        }
        else
        {
            return Result<long>.Invalid($"{argumentName} is not of type long integer");
        }
    }

    public Result<decimal> GetArgumentDecimalValue(int index, string argumentName, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
    {
        var argumentValueResult = GetArgumentValue(index, argumentName, evaluator);
        if (argumentValueResult.Value is decimal d1)
        {
            return Result<decimal>.Success(d1);
        }
        else if (argumentValueResult.Value is string s)
        {
            var parseResult = parser.Parse(s, FormatProvider, Context);
            if (!parseResult.IsSuccessful())
            {
                return Result<decimal>.Invalid($"{argumentName} is not of type decimal");
            }
            else if (parseResult.Value is decimal d2)
            {
                return Result<decimal>.Success(d2);
            }
            else
            {
                return Result<decimal>.Invalid($"{argumentName} is not of type decimal");
            }
        }
        else
        {
            return Result<decimal>.Invalid($"{argumentName} is not of type decimal");
        }
    }

    public Result<bool> GetArgumentBooleanValue(int index, string argumentName, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
    {
        var argumentValueResult = GetArgumentValue(index, argumentName, evaluator);
        if (argumentValueResult.Value is bool b1)
        {
            return Result<bool>.Success(b1);
        }
        else if (argumentValueResult.Value is string s)
        {
            var parseResult = parser.Parse(s, FormatProvider, Context);
            if (!parseResult.IsSuccessful())
            {
                return Result<bool>.Invalid($"{argumentName} is not of type boolean");
            }
            else if (parseResult.Value is bool b2)
            {
                return Result<bool>.Success(b2);
            }
            else
            {
                return Result<bool>.Invalid($"{argumentName} is not of type boolean");
            }
        }
        else
        {
            return Result<bool>.Invalid($"{argumentName} is not of type boolean");
        }
    }

    public Result<DateTime> GetArgumentDateTimeValue(int index, string argumentName, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
    {
        var argumentValueResult = GetArgumentValue(index, argumentName, evaluator);
        if (argumentValueResult.Value is DateTime dt1)
        {
            return Result<DateTime>.Success(dt1);
        }
        else if (argumentValueResult.Value is string s)
        {
            var parseResult = parser.Parse(s, FormatProvider, Context);
            if (!parseResult.IsSuccessful())
            {
                return Result<DateTime>.Invalid($"{argumentName} is not of type DateTime");
            }
            else if (parseResult.Value is DateTime dt2)
            {
                return Result<DateTime>.Success(dt2);
            }
            else
            {
                return Result<DateTime>.Invalid($"{argumentName} is not of type DateTime");
            }
        }
        else
        {
            return Result<DateTime>.Invalid($"{argumentName} is not of type DateTime");
        }
    }
}
