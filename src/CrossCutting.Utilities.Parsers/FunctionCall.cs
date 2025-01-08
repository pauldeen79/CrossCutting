namespace CrossCutting.Utilities.Parsers;

public partial record FunctionCall
{
    public Result<object?> GetArgumentValueResult(int index, string argumentName, FunctionCallContext context)
        => index + 1 > Arguments.Count
            ? Result.Invalid<object?>($"Missing argument: {argumentName}")
            : Arguments.ElementAt(index).GetValueResult(context);

    public Result<object?> GetArgumentValueResult(int index, string argumentName, FunctionCallContext context, object? defaultValue)
        => index + 1 > Arguments.Count
            ? Result.Success(defaultValue)
            : Arguments.ElementAt(index).GetValueResult(context);

    public Result<T> GetTypedArgumentValueResult<T>(int index, string argumentName, FunctionCallContext context)
        => index + 1 > Arguments.Count
            ? Result.Invalid<T>($"Missing argument: {argumentName}")
            : Arguments.ElementAt(index).GetValueResult(context).TryCast<T>();

    public Result<T> GetTypedArgumentValueResult<T>(int index, string argumentName, FunctionCallContext context, T defaultValue)
        => index + 1 > Arguments.Count
            ? Result.Success(defaultValue)
            : Arguments.ElementAt(index).GetValueResult(context).TryCast<T>();

    public Result<string> GetArgumentStringValueResult(int index, string argumentName, FunctionCallContext context)
        => ProcessStringArgumentResult(argumentName, GetArgumentValueResult(index, argumentName, context));

    public Result<string> GetArgumentStringValueResult(int index, string argumentName, FunctionCallContext context, string defaultValue)
        => ProcessStringArgumentResult(argumentName, GetArgumentValueResult(index, argumentName, context, defaultValue));

    public Result<int> GetArgumentInt32ValueResult(int index, string argumentName, FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return ProcessInt32ArgumentResult(argumentName, context , GetArgumentValueResult(index, argumentName, context));
    }

    public Result<int> GetArgumentInt32ValueResult(int index, string argumentName, FunctionCallContext context, int defaultValue)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return ProcessInt32ArgumentResult(argumentName, context , GetArgumentValueResult(index, argumentName, context, defaultValue));
    }

    public Result<long> GetArgumentInt64ValueResult(int index, string argumentName, FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return ProcessInt64ArgumentResult(argumentName, context , GetArgumentValueResult(index, argumentName, context));
    }

    public Result<long> GetArgumentInt64ValueResult(int index, string argumentName, FunctionCallContext context, long defaultValue)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return ProcessInt64ArgumentResult(argumentName, context , GetArgumentValueResult(index, argumentName, context, defaultValue));
    }

    public Result<decimal> GetArgumentDecimalValueResult(int index, string argumentName, FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return ProcessDecimalArgumentResult(argumentName, context , GetArgumentValueResult(index, argumentName, context));
    }

    public Result<decimal> GetArgumentDecimalValueResult(int index, string argumentName, FunctionCallContext context, decimal defaultValue)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return ProcessDecimalArgumentResult(argumentName, context , GetArgumentValueResult(index, argumentName, context, defaultValue));
    }

    public Result<bool> GetArgumentBooleanValueResult(int index, string argumentName, FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return ProcessBooleanArgumentResult(argumentName, context , GetArgumentValueResult(index, argumentName, context));
    }

    public Result<bool> GetArgumentBooleanValueResult(int index, string argumentName, FunctionCallContext context, bool defaultValue)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return ProcessBooleanArgumentResult(argumentName, context , GetArgumentValueResult(index, argumentName, context, defaultValue));
    }

    public Result<DateTime> GetArgumentDateTimeValueResult(int index, string argumentName, FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return ProcessDateTimeArgumentResult(argumentName, context , GetArgumentValueResult(index, argumentName, context));
    }

    public Result<DateTime> GetArgumentDateTimeValueResult(int index, string argumentName, FunctionCallContext context, DateTime defaultValue)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return ProcessDateTimeArgumentResult(argumentName, context , GetArgumentValueResult(index, argumentName, context, defaultValue));
    }

    public static Result<int> ProcessInt32ArgumentResult(string argumentName, FunctionCallContext context, Result<object?> argumentValueResult)
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

        var parseResult = context.ExpressionEvaluator.Evaluate(s, context.FormatProvider, context.Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<int>($"{argumentName} is not of type integer");
        }

        return parseResult.Value is int i2
            ? Result.Success(i2)
            : Result.Invalid<int>($"{argumentName} is not of type integer");
    }

    public static Result<long> ProcessInt64ArgumentResult(string argumentName, FunctionCallContext context, Result<object?> argumentValueResult)
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

        var parseResult = context.ExpressionEvaluator.Evaluate(s, context.FormatProvider, context.Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<long>($"{argumentName} is not of type long integer");
        }

        return parseResult.Value is long l2
            ? Result.Success(l2)
            : Result.Invalid<long>($"{argumentName} is not of type long integer");
    }

    public static Result<decimal> ProcessDecimalArgumentResult(string argumentName, FunctionCallContext context, Result<object?> argumentValueResult)
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

        var parseResult = context.ExpressionEvaluator.Evaluate(s, context.FormatProvider, context.Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<decimal>($"{argumentName} is not of type decimal");
        }

        return parseResult.Value is decimal d2
            ? Result.Success(d2)
            : Result.Invalid<decimal>($"{argumentName} is not of type decimal");
    }

    public static Result<bool> ProcessBooleanArgumentResult(string argumentName, FunctionCallContext context, Result<object?> argumentValueResult)
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

        var parseResult = context.ExpressionEvaluator.Evaluate(s, context.FormatProvider, context.Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<bool>($"{argumentName} is not of type boolean");
        }

        return parseResult.Value is bool b2
            ? Result.Success(b2)
            : Result.Invalid<bool>($"{argumentName} is not of type boolean");
    }

    public static Result<DateTime> ProcessDateTimeArgumentResult(string argumentName, FunctionCallContext context, Result<object?> argumentValueResult)
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
        var parseResult = context.ExpressionEvaluator.Evaluate(s, context.FormatProvider, context.Context);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<DateTime>($"{argumentName} is not of type datetime");
        }

        return parseResult.Value is DateTime dt2
            ? Result.Success(dt2)
            : Result.Invalid<DateTime>($"{argumentName} is not of type datetime");
    }

    public static Result<string> ProcessStringArgumentResult(string argumentName, Result<object?> argumentValueResult)
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
