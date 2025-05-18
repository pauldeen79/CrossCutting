namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionCallContext
{
    public FunctionCallContext(FunctionCall functionCall, ExpressionEvaluatorContext context)
    {
        functionCall = ArgumentGuard.IsNotNull(functionCall, nameof(functionCall));
        ArgumentGuard.IsNotNull(context, nameof(context));

        FunctionCall = functionCall;
        Context = context;
        MemberType = functionCall.MemberType;

        if (MemberType == MemberType.Unknown)
        {
            throw new ArgumentException("MemberType cannot be Unknown", nameof(functionCall));
        }
    }

    public FunctionCallContext(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        FunctionCall = state.FunctionParseResult.GetValueOrThrow();
        Context = state.Context;
        MemberType = state.Type.ToMemberType();
        InstanceValue = state.Value;
        ResultType = state.ResultType;
    }

    public FunctionCall FunctionCall { get; }
    public ExpressionEvaluatorContext Context { get; }
    public MemberType MemberType { get; }
    public object? InstanceValue { get; }
    public Type? ResultType { get; }

    public Task<Result<object?>> GetArgumentValueResultAsync(int index, string argumentName, CancellationToken token)
        => FunctionCall.GetArgumentValueResultAsync(index, argumentName, this, token);

    public Task<Result<object?>> GetArgumentValueResultAsync(int index, string argumentName, object? defaultValue, CancellationToken token)
        => FunctionCall.GetArgumentValueResultAsync(index, argumentName, this, defaultValue, token);

    public Task<Result<T>> GetArgumentValueResultAsync<T>(int index, string argumentName, CancellationToken token)
        => FunctionCall.GetArgumentValueResultAsync<T>(index, argumentName, this, token);

    public Task<Result<T?>> GetArgumentValueResultAsync<T>(int index, string argumentName, T? defaultValue, CancellationToken token)
        => FunctionCall.GetArgumentValueResultAsync(index, argumentName, this, defaultValue, token);

    //TODO: Remove custom methods for string, int32 etc. below
    public Task<Result<string>> GetArgumentStringValueResultAsync(int index, string argumentName, CancellationToken token)
        => FunctionCall.GetArgumentStringValueResultAsync(index, argumentName, this, token);

    public Task<Result<string>> GetArgumentStringValueResultAsync(int index, string argumentName, string defaultValue, CancellationToken token)
        => FunctionCall.GetArgumentStringValueResultAsync(index, argumentName, this, defaultValue, token);

    public Task<Result<int>> GetArgumentInt32ValueResultAsync(int index, string argumentName, CancellationToken token)
        => FunctionCall.GetArgumentInt32ValueResultAsync(index, argumentName, this, token);

    public Task<Result<int>> GetArgumentInt32ValueResultAsync(int index, string argumentName, int defaultValue, CancellationToken token)
        => FunctionCall.GetArgumentInt32ValueResultAsync(index, argumentName, this, defaultValue, token);

    public Task<Result<long>> GetArgumentInt64ValueResultAsync(int index, string argumentName, CancellationToken token)
        => FunctionCall.GetArgumentInt64ValueResultAsync(index, argumentName, this, token);

    public Task<Result<long>> GetArgumentInt64ValueResultAsync(int index, string argumentName, long defaultValue, CancellationToken token)
        => FunctionCall.GetArgumentInt64ValueResultAsync(index, argumentName, this, defaultValue, token);

    public Task<Result<decimal>> GetArgumentDecimalValueResultAsync(int index, string argumentName, CancellationToken token)
        => FunctionCall.GetArgumentDecimalValueResultAsync(index, argumentName, this, token);

    public Task<Result<decimal>> GetArgumentDecimalValueResultAsync(int index, string argumentName, decimal defaultValue, CancellationToken token)
        => FunctionCall.GetArgumentDecimalValueResultAsync(index, argumentName, this, defaultValue, token);

    public Task<Result<bool>> GetArgumentBooleanValueResultAsync(int index, string argumentName, CancellationToken token)
        => FunctionCall.GetArgumentBooleanValueResultAsync(index, argumentName, this, token);

    public Task<Result<bool>> GetArgumentBooleanValueResultAsync(int index, string argumentName, bool defaultValue, CancellationToken token)
        => FunctionCall.GetArgumentBooleanValueResultAsync(index, argumentName, this, defaultValue, token);

    public Task<Result<DateTime>> GetArgumentDateTimeValueResultAsync(int index, string argumentName, CancellationToken token)
        => FunctionCall.GetArgumentDateTimeValueResultAsync(index, argumentName, this, token);

    public Task<Result<DateTime>> GetArgumentDateTimeValueResultAsync(int index, string argumentName, DateTime defaultValue, CancellationToken token)
        => FunctionCall.GetArgumentDateTimeValueResultAsync(index, argumentName, this, defaultValue, token);

    public Task<Result<object?>> EvaluateAsync(IGenericFunction genericFunction, CancellationToken token)
    {
        genericFunction = ArgumentGuard.IsNotNull(genericFunction, nameof(genericFunction));

        try
        {
            var method = genericFunction
                .GetType()
                .GetMethod(nameof(IGenericFunction.EvaluateGenericAsync))!
                .MakeGenericMethod(FunctionCall.TypeArguments.ToArray());

            return (Task<Result<object?>>)method.Invoke(genericFunction, [this, token]);
        }
        catch (ArgumentException argException)
        {
            //The type or method has 1 generic parameter(s), but 0 generic argument(s) were provided. A generic argument must be provided for each generic parameter.
            return Task.FromResult(Result.Invalid<object?>(argException.Message));
        }
    }

    public Result<T> GetInstanceValueResult<T>()
        => InstanceValue is T typedValue
            ? Result.Success(typedValue)
            : Result.Error<T>(GetInstanceValueErrorMessage<T>());

    public Task<Result<T>> GetInstanceValueResultAsync<T>(CancellationToken token)
        => Task.Run(GetInstanceValueResult<T>, token);

    private string GetInstanceValueErrorMessage<T>()
        => InstanceValue is null
            ? "Instance value is null"
            : $"Instance value is not of type {typeof(T).FullName}";
}
