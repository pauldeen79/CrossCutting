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
        => index + 1 > FunctionCall.Arguments.Count
            ? Result.Invalid<object?>($"Missing argument: {argumentName}")
            : FunctionCall.Arguments.ElementAt(index).GetValueResult(this);

    public Result<object?> GetArgumentValueResult(int index, string argumentName, object? defaultValue)
        => index + 1 > FunctionCall.Arguments.Count
            ? Result.Success(defaultValue)
            : FunctionCall.Arguments.ElementAt(index).GetValueResult(this);

    public Result<T> GetTypedArgumentValueResult<T>(int index, string argumentName)
        => index + 1 > FunctionCall.Arguments.Count
            ? Result.Invalid<T>($"Missing argument: {argumentName}")
            : FunctionCall.Arguments.ElementAt(index).GetValueResult(this).TryCast<T>();

    public Result<T> GetTypedArgumentValueResult<T>(int index, string argumentName, T defaultValue)
        => index + 1 > FunctionCall.Arguments.Count
            ? Result.Success(defaultValue)
            : FunctionCall.Arguments.ElementAt(index).GetValueResult(this).TryCast<T>();

    public Result<string> GetArgumentStringValueResult(int index, string argumentName)
        => FunctionCall.ProcessStringArgumentResult(argumentName, FunctionCall.GetArgumentValueResult(index, argumentName, this));

    public Result<string> GetArgumentStringValueResult(int index, string argumentName, string defaultValue)
        => FunctionCall.ProcessStringArgumentResult(argumentName, FunctionCall.GetArgumentValueResult(index, argumentName, this, defaultValue));

    public Result<int> GetArgumentInt32ValueResult(int index, string argumentName)
        => FunctionCall.ProcessInt32ArgumentResult(argumentName, this, FunctionCall.GetArgumentValueResult(index, argumentName, this));

    public Result<int> GetArgumentInt32ValueResult(int index, string argumentName, int defaultValue)
        => FunctionCall.ProcessInt32ArgumentResult(argumentName, this, FunctionCall.GetArgumentValueResult(index, argumentName, this, defaultValue));

    public Result<long> GetArgumentInt64ValueResult(int index, string argumentName)
        => FunctionCall.ProcessInt64ArgumentResult(argumentName, this, FunctionCall.GetArgumentValueResult(index, argumentName, this));

    public Result<long> GetArgumentInt64ValueResult(int index, string argumentName, long defaultValue)
        => FunctionCall.ProcessInt64ArgumentResult(argumentName, this, FunctionCall.GetArgumentValueResult(index, argumentName, this, defaultValue));

    public Result<decimal> GetArgumentDecimalValueResult(int index, string argumentName)
        => FunctionCall.ProcessDecimalArgumentResult(argumentName, this, FunctionCall.GetArgumentValueResult(index, argumentName, this));

    public Result<decimal> GetArgumentDecimalValueResult(int index, string argumentName, decimal defaultValue)
        => FunctionCall.ProcessDecimalArgumentResult(argumentName, this, FunctionCall.GetArgumentValueResult(index, argumentName, this, defaultValue));

    public Result<bool> GetArgumentBooleanValueResult(int index, string argumentName)
        => FunctionCall.ProcessBooleanArgumentResult(argumentName, this, FunctionCall.GetArgumentValueResult(index, argumentName, this));

    public Result<bool> GetArgumentBooleanValueResult(int index, string argumentName, bool defaultValue)
        => FunctionCall.ProcessBooleanArgumentResult(argumentName, this, FunctionCall.GetArgumentValueResult(index, argumentName, this, defaultValue));

    public Result<DateTime> GetArgumentDateTimeValueResult(int index, string argumentName)
        => FunctionCall.ProcessDateTimeArgumentResult(argumentName, this, FunctionCall.GetArgumentValueResult(index, argumentName, this));

    public Result<DateTime> GetArgumentDateTimeValueResult(int index, string argumentName, DateTime defaultValue)
        => FunctionCall.ProcessDateTimeArgumentResult(argumentName, this, FunctionCall.GetArgumentValueResult(index, argumentName, this, defaultValue));
}
