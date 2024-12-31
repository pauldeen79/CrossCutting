namespace CrossCutting.Utilities.Parsers;

public partial record FunctionCall
{
    public Result<object?> GetArgumentValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator)
        => index + 1 > Arguments.Count
            ? Result.Invalid<object?>($"Missing argument: {argumentName}")
            : Arguments.ElementAt(index).GetValueResult(context, functionEvaluator, expressionEvaluator, FormatProvider);

    public Result<object?> GetArgumentValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, object? defaultValue)
        => index + 1 > Arguments.Count
            ? Result.Success(defaultValue)
            : Arguments.ElementAt(index).GetValueResult(context, functionEvaluator, expressionEvaluator, FormatProvider);

    public Result<string> GetArgumentStringValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator)
        => ProcessStringArgumentResult(argumentName, GetArgumentValueResult(index, argumentName, context, functionEvaluator, expressionEvaluator));

    public Result<string> GetArgumentStringValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, string defaultValue)
        => ProcessStringArgumentResult(argumentName, GetArgumentValueResult(index, argumentName, context, functionEvaluator, expressionEvaluator, defaultValue));

    public Result<int> GetArgumentInt32ValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator)
    {
        expressionEvaluator = ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        return ProcessInt32ArgumentResult(argumentName, expressionEvaluator, GetArgumentValueResult(index, argumentName, context, functionEvaluator, expressionEvaluator));
    }

    public Result<int> GetArgumentInt32ValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, int defaultValue)
    {
        expressionEvaluator = ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        return ProcessInt32ArgumentResult(argumentName, expressionEvaluator, GetArgumentValueResult(index, argumentName, context, functionEvaluator, expressionEvaluator, defaultValue));
    }

    public Result<long> GetArgumentInt64ValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator)
    {
        expressionEvaluator = ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        return ProcessInt64ArgumentResult(argumentName, expressionEvaluator, GetArgumentValueResult(index, argumentName, context, functionEvaluator, expressionEvaluator));
    }

    public Result<long> GetArgumentInt64ValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, long defaultValue)
    {
        expressionEvaluator = ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        return ProcessInt64ArgumentResult(argumentName, expressionEvaluator, GetArgumentValueResult(index, argumentName, context, functionEvaluator, expressionEvaluator, defaultValue));
    }

    public Result<decimal> GetArgumentDecimalValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator)
    {
        expressionEvaluator = ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        return ProcessDecimalArgumentResult(argumentName, expressionEvaluator, GetArgumentValueResult(index, argumentName, context, functionEvaluator, expressionEvaluator));
    }

    public Result<decimal> GetArgumentDecimalValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, decimal defaultValue)
    {
        expressionEvaluator = ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        return ProcessDecimalArgumentResult(argumentName, expressionEvaluator, GetArgumentValueResult(index, argumentName, context, functionEvaluator, expressionEvaluator, defaultValue));
    }

    public Result<bool> GetArgumentBooleanValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator)
    {
        expressionEvaluator = ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        return ProcessBooleanArgumentResult(argumentName, expressionEvaluator, GetArgumentValueResult(index, argumentName, context, functionEvaluator, expressionEvaluator));
    }

    public Result<bool> GetArgumentBooleanValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, bool defaultValue)
    {
        expressionEvaluator = ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        return ProcessBooleanArgumentResult(argumentName, expressionEvaluator, GetArgumentValueResult(index, argumentName, context, functionEvaluator, expressionEvaluator, defaultValue));
    }

    public Result<DateTime> GetArgumentDateTimeValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator)
    {
        expressionEvaluator = ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        return ProcessDateTimeArgumentResult(argumentName, expressionEvaluator, GetArgumentValueResult(index, argumentName, context, functionEvaluator, expressionEvaluator));
    }

    public Result<DateTime> GetArgumentDateTimeValueResult(int index, string argumentName, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, DateTime defaultValue)
    {
        expressionEvaluator = ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        return ProcessDateTimeArgumentResult(argumentName, expressionEvaluator, GetArgumentValueResult(index, argumentName, context, functionEvaluator, expressionEvaluator, defaultValue));
    }

    private Result<int> ProcessInt32ArgumentResult(string argumentName, IExpressionEvaluator expressionEvaluator, Result<object?> argumentValueResult)
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

        var parseResult = expressionEvaluator.Evaluate(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<int>($"{argumentName} is not of type integer");
        }

        return parseResult.Value is int i2
            ? Result.Success(i2)
            : Result.Invalid<int>($"{argumentName} is not of type integer");
    }

    private Result<long> ProcessInt64ArgumentResult(string argumentName, IExpressionEvaluator expressionEvaluator, Result<object?> argumentValueResult)
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

        var parseResult = expressionEvaluator.Evaluate(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<long>($"{argumentName} is not of type long integer");
        }

        return parseResult.Value is long l2
            ? Result.Success(l2)
            : Result.Invalid<long>($"{argumentName} is not of type long integer");
    }

    private Result<decimal> ProcessDecimalArgumentResult(string argumentName, IExpressionEvaluator expressionEvaluator, Result<object?> argumentValueResult)
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

        var parseResult = expressionEvaluator.Evaluate(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<decimal>($"{argumentName} is not of type decimal");
        }

        return parseResult.Value is decimal d2
            ? Result.Success(d2)
            : Result.Invalid<decimal>($"{argumentName} is not of type decimal");
    }

    private Result<bool> ProcessBooleanArgumentResult(string argumentName, IExpressionEvaluator expressionEvaluator, Result<object?> argumentValueResult)
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

        var parseResult = expressionEvaluator.Evaluate(s, FormatProvider, Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<bool>($"{argumentName} is not of type boolean");
        }

        return parseResult.Value is bool b2
            ? Result.Success(b2)
            : Result.Invalid<bool>($"{argumentName} is not of type boolean");
    }

    private Result<DateTime> ProcessDateTimeArgumentResult(string argumentName, IExpressionEvaluator expressionEvaluator, Result<object?> argumentValueResult)
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
        var parseResult = expressionEvaluator.Evaluate(s, FormatProvider, Context);
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
