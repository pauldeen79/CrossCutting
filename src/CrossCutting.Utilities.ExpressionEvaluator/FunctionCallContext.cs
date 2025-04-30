namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionCallContext
{
    public FunctionCallContext(FunctionCall functionCall, ExpressionEvaluatorContext context)
    {
        ArgumentGuard.IsNotNull(functionCall, nameof(functionCall));
        ArgumentGuard.IsNotNull(context, nameof(context));

        FunctionCall = functionCall;
        Context = context;
    }

    public FunctionCall FunctionCall { get; }
    public ExpressionEvaluatorContext Context { get; }

    public Result<object?> GetArgumentValueResult(int index, string argumentName)
        => FunctionCall.GetArgumentValueResult(index, argumentName, this);

    public Result<object?> GetArgumentValueResult(int index, string argumentName, object? defaultValue)
        => FunctionCall.GetArgumentValueResult(index, argumentName, this, defaultValue);

    public Result<T> GetArgumentValueResult<T>(int index, string argumentName)
        => FunctionCall.GetArgumentValueResult<T>(index, argumentName, this);

    public Result<T?> GetArgumentValueResult<T>(int index, string argumentName, T? defaultValue)
        => FunctionCall.GetArgumentValueResult(index, argumentName, this, defaultValue);

    public Result<string> GetArgumentStringValueResult(int index, string argumentName)
        => FunctionCall.GetArgumentStringValueResult(index, argumentName, this);

    public Result<string> GetArgumentStringValueResult(int index, string argumentName, string defaultValue)
        => FunctionCall.GetArgumentStringValueResult(index, argumentName, this, defaultValue);

    public Result<int> GetArgumentInt32ValueResult(int index, string argumentName)
        => FunctionCall.GetArgumentInt32ValueResult(index, argumentName, this);

    public Result<int> GetArgumentInt32ValueResult(int index, string argumentName, int defaultValue)
        => FunctionCall.GetArgumentInt32ValueResult(index, argumentName, this, defaultValue);

    public Result<long> GetArgumentInt64ValueResult(int index, string argumentName)
        => FunctionCall.GetArgumentInt64ValueResult(index, argumentName, this);

    public Result<long> GetArgumentInt64ValueResult(int index, string argumentName, long defaultValue)
        => FunctionCall.GetArgumentInt64ValueResult(index, argumentName, this, defaultValue);

    public Result<decimal> GetArgumentDecimalValueResult(int index, string argumentName)
        => FunctionCall.GetArgumentDecimalValueResult(index, argumentName, this);

    public Result<decimal> GetArgumentDecimalValueResult(int index, string argumentName, decimal defaultValue)
        => FunctionCall.GetArgumentDecimalValueResult(index, argumentName, this, defaultValue);

    public Result<bool> GetArgumentBooleanValueResult(int index, string argumentName)
        => FunctionCall.GetArgumentBooleanValueResult(index, argumentName, this);

    public Result<bool> GetArgumentBooleanValueResult(int index, string argumentName, bool defaultValue)
        => FunctionCall.GetArgumentBooleanValueResult(index, argumentName, this, defaultValue);

    public Result<DateTime> GetArgumentDateTimeValueResult(int index, string argumentName)
        => FunctionCall.GetArgumentDateTimeValueResult(index, argumentName, this);

    public Result<DateTime> GetArgumentDateTimeValueResult(int index, string argumentName, DateTime defaultValue)
        => FunctionCall.GetArgumentDateTimeValueResult(index, argumentName, this, defaultValue);

    public Result<object?> Evaluate(IGenericFunction genericFunction)
    {
        genericFunction = ArgumentGuard.IsNotNull(genericFunction, nameof(genericFunction));

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            var method = genericFunction
                .GetType()
                .GetMethod(nameof(IGenericFunction.EvaluateGeneric))!
                .MakeGenericMethod(FunctionCall.TypeArguments.ToArray());

            return (Result<object?>)method.Invoke(genericFunction, [this])
                ?? Result.Error<object?>("Generic evaluation result was null");
        }
        catch (ArgumentException argException)
        {
            //The type or method has 1 generic parameter(s), but 0 generic argument(s) were provided. A generic argument must be provided for each generic parameter.
            return Result.Invalid<object?>(argException.Message);
        }
        catch (Exception ex)
        {
            return Result.Error<object?>(ex, "Generic evaluation failed, see Exception property for more details");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}
