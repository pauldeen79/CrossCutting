namespace CrossCutting.Utilities.Parsers;

public partial record FunctionParseResult
{
    public Result<object?> GetArgumentValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
        => index + 1 > Arguments.Count
            ? Result.Invalid<object?>($"Missing argument: {argumentName}")
            : Arguments.ElementAt(index).GetValueResult(context, evaluator, parser, FormatProvider);

    public Result<object?> GetArgumentValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, object? defaultValue)
        => index + 1 > Arguments.Count
            ? Result.Success(defaultValue)
            : Arguments.ElementAt(index).GetValueResult(context, evaluator, parser, FormatProvider);

    public Result<string> GetArgumentStringValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
        => ProcessStringArgumentResult(argumentName, GetArgumentValueResult(index, argumentName, context, evaluator, parser));

    public Result<string> GetArgumentStringValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, string defaultValue)
        => ProcessStringArgumentResult(argumentName, GetArgumentValueResult(index, argumentName, context, evaluator, parser, defaultValue));

    public Result<int> GetArgumentInt32ValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        return ProcessInt32ArgumentResult(argumentName, parser, GetArgumentValueResult(index, argumentName, context, evaluator, parser));
    }

    public Result<int> GetArgumentInt32ValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, int defaultValue)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        return ProcessInt32ArgumentResult(argumentName, parser, GetArgumentValueResult(index, argumentName, context, evaluator, parser, defaultValue));
    }

    public Result<long> GetArgumentInt64ValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        return ProcessInt64ArgumentResult(argumentName, parser, GetArgumentValueResult(index, argumentName, context, evaluator, parser));
    }

    public Result<long> GetArgumentInt64ValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, long defaultValue)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        return ProcessInt64ArgumentResult(argumentName, parser, GetArgumentValueResult(index, argumentName, context, evaluator, parser, defaultValue));
    }

    public Result<decimal> GetArgumentDecimalValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        return ProcessDecimalArgumentResult(argumentName, parser, GetArgumentValueResult(index, argumentName, context, evaluator, parser));
    }

    public Result<decimal> GetArgumentDecimalValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, decimal defaultValue)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        return ProcessDecimalArgumentResult(argumentName, parser, GetArgumentValueResult(index, argumentName, context, evaluator, parser, defaultValue));
    }

    public Result<bool> GetArgumentBooleanValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        return ProcessBooleanArgumentResult(argumentName, parser, GetArgumentValueResult(index, argumentName, context, evaluator, parser));
    }

    public Result<bool> GetArgumentBooleanValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, bool defaultValue)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        return ProcessBooleanArgumentResult(argumentName, parser, GetArgumentValueResult(index, argumentName, context, evaluator, parser, defaultValue));
    }

    public Result<DateTime> GetArgumentDateTimeValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        return ProcessDateTimeArgumentResult(argumentName, parser, GetArgumentValueResult(index, argumentName, context, evaluator, parser));
    }

    public Result<DateTime> GetArgumentDateTimeValueResult(int index, string argumentName, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, DateTime defaultValue)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        return ProcessDateTimeArgumentResult(argumentName, parser, GetArgumentValueResult(index, argumentName, context, evaluator, parser, defaultValue));
    }

    private Result<int> ProcessInt32ArgumentResult(string argumentName, IExpressionParser parser, Result<object?> argumentValueResult)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return Result.FromExistingResult<int>(argumentValueResult);
        }

        if (argumentValueResult.Value is int i1)
        {
            return Result.Success(i1);
        }

        if (argumentValueResult.Value is not string s)
        {
            return Result.Invalid<int>($"{argumentName} is not of type integer");
        }

        var parseResult = parser.Parse(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<int>($"{argumentName} is not of type integer");
        }

        return parseResult.Value is int i2
            ? Result.Success(i2)
            : Result.Invalid<int>($"{argumentName} is not of type integer");
    }

    private Result<long> ProcessInt64ArgumentResult(string argumentName, IExpressionParser parser, Result<object?> argumentValueResult)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return Result.FromExistingResult<long>(argumentValueResult);
        }

        if (argumentValueResult.Value is long l1)
        {
            return Result.Success(l1);
        }

        if (argumentValueResult.Value is not string s)
        {
            return Result.Invalid<long>($"{argumentName} is not of type long integer");
        }

        var parseResult = parser.Parse(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<long>($"{argumentName} is not of type long integer");
        }

        return parseResult.Value is long l2
            ? Result.Success(l2)
            : Result.Invalid<long>($"{argumentName} is not of type long integer");
    }

    private Result<decimal> ProcessDecimalArgumentResult(string argumentName, IExpressionParser parser, Result<object?> argumentValueResult)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return Result.FromExistingResult<decimal>(argumentValueResult);
        }

        if (argumentValueResult.Value is decimal d1)
        {
            return Result.Success(d1);
        }

        if (argumentValueResult.Value is not string s)
        {
            return Result.Invalid<decimal>($"{argumentName} is not of type decimal");
        }

        var parseResult = parser.Parse(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<decimal>($"{argumentName} is not of type decimal");
        }

        return parseResult.Value is decimal d2
            ? Result.Success(d2)
            : Result.Invalid<decimal>($"{argumentName} is not of type decimal");
    }

    private Result<bool> ProcessBooleanArgumentResult(string argumentName, IExpressionParser parser, Result<object?> argumentValueResult)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return Result.FromExistingResult<bool>(argumentValueResult);
        }

        if (argumentValueResult.Value is bool b1)
        {
            return Result.Success(b1);
        }

        if (argumentValueResult.Value is not string s)
        {
            return Result.Invalid<bool>($"{argumentName} is not of type boolean");
        }

        var parseResult = parser.Parse(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<bool>($"{argumentName} is not of type boolean");
        }

        return parseResult.Value is bool b2
            ? Result.Success(b2)
            : Result.Invalid<bool>($"{argumentName} is not of type boolean");
    }

    private Result<DateTime> ProcessDateTimeArgumentResult(string argumentName, IExpressionParser parser, Result<object?> argumentValueResult)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return Result.FromExistingResult<DateTime>(argumentValueResult);
        }

        if (argumentValueResult.Value is DateTime dt1)
        {
            return Result.Success(dt1);
        }

        if (argumentValueResult.Value is not string s)
        {
            return Result.Invalid<DateTime>($"{argumentName} is not of type datetime");
        }
        var parseResult = parser.Parse(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<DateTime>($"{argumentName} is not of type datetime");
        }

        return parseResult.Value is DateTime dt2
            ? Result.Success(dt2)
            : Result.Invalid<DateTime>($"{argumentName} is not of type datetime");
    }

    private static Result<string> ProcessStringArgumentResult(string argumentName, Result<object?> argumentValueResult)
    {
        if (!argumentValueResult.IsSuccessful())
        {
            return Result.FromExistingResult<string>(argumentValueResult);
        }

        return argumentValueResult.Value is string stringValue
            ? Result.Success(stringValue)
            : Result.Invalid<string>($"{argumentName} is not of type string");
    }
}
