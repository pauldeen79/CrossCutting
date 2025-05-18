namespace CrossCutting.Utilities.ExpressionEvaluator;

public partial record FunctionCall
{
    public async Task<Result<object?>> GetArgumentValueResultAsync(int index, string argumentName, FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return index + 1 > Arguments.Count
            ? Result.Invalid<object?>($"Missing argument: {argumentName}")
            : await context.Context.EvaluateAsync(Arguments.ElementAt(index), token).ConfigureAwait(false);
    }

    public async Task<Result<object?>> GetArgumentValueResultAsync(int index, string argumentName, FunctionCallContext context, object? defaultValue, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return index + 1 > Arguments.Count
            ? Result.Success(defaultValue)
            : await context.Context.EvaluateAsync(Arguments.ElementAt(index), token).ConfigureAwait(false);
    }

    public async Task<Result<T>> GetArgumentValueResultAsync<T>(int index, string argumentName, FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return index + 1 > Arguments.Count
            ? Result.Invalid<T>($"Missing argument: {argumentName}")
            : (await context.Context.EvaluateAsync(Arguments.ElementAt(index), token).ConfigureAwait(false)).TryCast<T>();
    }

    public async Task<Result<T?>> GetArgumentValueResultAsync<T>(int index, string argumentName, FunctionCallContext context, T? defaultValue, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return index + 1 > Arguments.Count
            ? Result.Success(defaultValue)
            : (await context.Context.EvaluateAsync(Arguments.ElementAt(index), token).ConfigureAwait(false))
                .TryCastAllowNull<T>()
                .Transform(value => value is null ? defaultValue : value);
    }

    //TODO: Remove everything below this line
    public async Task<Result<string>> GetArgumentStringValueResultAsync(int index, string argumentName, FunctionCallContext context, CancellationToken token)
        => ProcessStringArgumentResult(argumentName, await GetArgumentValueResultAsync(index, argumentName, context, token).ConfigureAwait(false));

    public async Task<Result<string>> GetArgumentStringValueResultAsync(int index, string argumentName, FunctionCallContext context, string defaultValue, CancellationToken token)
        => ProcessStringArgumentResult(argumentName, await GetArgumentValueResultAsync(index, argumentName, context, (object)defaultValue, token).ConfigureAwait(false));

    public async Task<Result<int>> GetArgumentInt32ValueResultAsync(int index, string argumentName, FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return await ProcessInt32ArgumentResultAsync(argumentName, context, await GetArgumentValueResultAsync(index, argumentName, context, token).ConfigureAwait(false), token).ConfigureAwait(false);
    }

    public async Task<Result<int>> GetArgumentInt32ValueResultAsync(int index, string argumentName, FunctionCallContext context, int defaultValue, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return await ProcessInt32ArgumentResultAsync(argumentName, context, await GetArgumentValueResultAsync(index, argumentName, context, (object)defaultValue, token).ConfigureAwait(false), token).ConfigureAwait(false);
    }

    public async Task<Result<long>> GetArgumentInt64ValueResultAsync(int index, string argumentName, FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return await ProcessInt64ArgumentResultAsync(argumentName, context, await GetArgumentValueResultAsync(index, argumentName, context, token).ConfigureAwait(false), token).ConfigureAwait(false);
    }

    public async Task<Result<long>> GetArgumentInt64ValueResultAsync(int index, string argumentName, FunctionCallContext context, long defaultValue, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return await ProcessInt64ArgumentResultAsync(argumentName, context, await GetArgumentValueResultAsync(index, argumentName, context, (object)defaultValue, token).ConfigureAwait(false), token).ConfigureAwait(false);
    }

    public async Task<Result<decimal>> GetArgumentDecimalValueResultAsync(int index, string argumentName, FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return await ProcessDecimalArgumentResultAsync(argumentName, context, await GetArgumentValueResultAsync(index, argumentName, context, token).ConfigureAwait(false), token).ConfigureAwait(false);
    }

    public async Task<Result<decimal>> GetArgumentDecimalValueResultAsync(int index, string argumentName, FunctionCallContext context, decimal defaultValue, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return await ProcessDecimalArgumentResultAsync(argumentName, context, await GetArgumentValueResultAsync(index, argumentName, context, (object)defaultValue, token).ConfigureAwait(false), token).ConfigureAwait(false);
    }

    public async Task<Result<bool>> GetArgumentBooleanValueResultAsync(int index, string argumentName, FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return await ProcessBooleanArgumentResultAsync(argumentName, context, await GetArgumentValueResultAsync(index, argumentName, context, token).ConfigureAwait(false), token).ConfigureAwait(false);
    }

    public async Task<Result<bool>> GetArgumentBooleanValueResultAsync(int index, string argumentName, FunctionCallContext context, bool defaultValue, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return await ProcessBooleanArgumentResultAsync(argumentName, context, await GetArgumentValueResultAsync(index, argumentName, context, (object)defaultValue, token).ConfigureAwait(false), token).ConfigureAwait(false);
    }

    public async Task<Result<DateTime>> GetArgumentDateTimeValueResultAsync(int index, string argumentName, FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return await ProcessDateTimeArgumentResultAsync(argumentName, context, await GetArgumentValueResultAsync(index, argumentName, context, token).ConfigureAwait(false), token).ConfigureAwait(false);
    }

    public async Task<Result<DateTime>> GetArgumentDateTimeValueResultAsync(int index, string argumentName, FunctionCallContext context, DateTime defaultValue, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return await ProcessDateTimeArgumentResultAsync(argumentName, context, await GetArgumentValueResultAsync(index, argumentName, context, (object)defaultValue, token).ConfigureAwait(false), token).ConfigureAwait(false);
    }

    private static async Task<Result<int>> ProcessInt32ArgumentResultAsync(string argumentName, FunctionCallContext context, Result<object?> argumentValueResult, CancellationToken token)
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

        var parseResult = await context.Context.EvaluateAsync(s, token).ConfigureAwait(false);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<int>($"{argumentName} is not of type integer");
        }

        return parseResult.Value is int i2
            ? Result.Success(i2)
            : Result.Invalid<int>($"{argumentName} is not of type integer");
    }

    private static async Task<Result<long>> ProcessInt64ArgumentResultAsync(string argumentName, FunctionCallContext context, Result<object?> argumentValueResult, CancellationToken token)
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

        var parseResult = await context.Context.EvaluateAsync(s, token).ConfigureAwait(false);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<long>($"{argumentName} is not of type long integer");
        }

        return parseResult.Value is long l2
            ? Result.Success(l2)
            : Result.Invalid<long>($"{argumentName} is not of type long integer");
    }

    private static async Task<Result<decimal>> ProcessDecimalArgumentResultAsync(string argumentName, FunctionCallContext context, Result<object?> argumentValueResult, CancellationToken token)
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

        var parseResult = await context.Context.EvaluateAsync(s, token).ConfigureAwait(false);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<decimal>($"{argumentName} is not of type decimal");
        }

        return parseResult.Value is decimal d2
            ? Result.Success(d2)
            : Result.Invalid<decimal>($"{argumentName} is not of type decimal");
    }

    private static async Task<Result<bool>> ProcessBooleanArgumentResultAsync(string argumentName, FunctionCallContext context, Result<object?> argumentValueResult, CancellationToken token)
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

        var parseResult = await context.Context.EvaluateAsync(s, token).ConfigureAwait(false);
        if (!parseResult.IsSuccessful())
        {
            return Result.Invalid<bool>($"{argumentName} is not of type boolean");
        }

        return parseResult.Value is bool b2
            ? Result.Success(b2)
            : Result.Invalid<bool>($"{argumentName} is not of type boolean");
    }

    private static async Task<Result<DateTime>> ProcessDateTimeArgumentResultAsync(string argumentName, FunctionCallContext context, Result<object?> argumentValueResult, CancellationToken token)
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
        var parseResult = await context.Context.EvaluateAsync(s, token).ConfigureAwait(false);
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
