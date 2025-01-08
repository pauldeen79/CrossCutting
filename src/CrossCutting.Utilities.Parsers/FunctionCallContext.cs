namespace CrossCutting.Utilities.Parsers;

public class FunctionCallContext
{
    public FunctionCallContext(FunctionCall functionCall, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context)
    {
        FunctionCall = functionCall ?? throw new ArgumentNullException(nameof(functionCall));
        FunctionEvaluator = functionEvaluator ?? throw new ArgumentNullException(nameof(functionEvaluator));
        ExpressionEvaluator = expressionEvaluator ?? throw new ArgumentNullException(nameof(expressionEvaluator));
        FormatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
        Context = context;
    }

    public FunctionCall FunctionCall { get; }
    public IFunctionEvaluator FunctionEvaluator { get; }
    public IExpressionEvaluator ExpressionEvaluator { get; }
    public IFormatProvider FormatProvider { get; }
    public object? Context { get; }

    public Result<object?> GetArgumentValueResult(int index, string argumentName)
        => FunctionCall.GetArgumentValueResult(index, argumentName, this);

    public Result<object?> GetArgumentValueResult(int index, string argumentName, object? defaultValue)
        => FunctionCall.GetArgumentValueResult(index, argumentName, this, defaultValue);

    public Result<T> GetArgumentValueResult<T>(int index, string argumentName)
        => FunctionCall.GetArgumentValueResult<T>(index, argumentName, this);

    public Result<T> GetArgumentValueResult<T>(int index, string argumentName, T defaultValue)
        => FunctionCall.GetArgumentValueResult<T>(index, argumentName, this, defaultValue);

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
}
