namespace CrossCutting.Utilities.Parsers;

public static class ParseResult
{
    public static ParseResult<TKey, TValue> NotUnderstood<TKey, TValue>()
        => new(false, [], []);
    public static ParseResult<TKey, TValue> Error<TKey, TValue>(IEnumerable<string> errorMessages)
        => new(false, errorMessages, []);
    public static ParseResult<TKey, TValue> Error<TKey, TValue>(IEnumerable<string> errorMessages, IEnumerable<KeyValuePair<TKey, TValue>> values)
        => new(false, errorMessages, values);
    public static ParseResult<TKey, TValue> Error<TKey, TValue>(string errorMessage)
        => new(false, [errorMessage], []);
    public static ParseResult<TKey, TValue> Error<TKey, TValue>(string errorMessage, IEnumerable<KeyValuePair<TKey, TValue>> values)
        => new(false, [errorMessage], values);
    public static ParseResult<TKey, TValue> Success<TKey, TValue>()
        => new(true, [], []);
    public static ParseResult<TKey, TValue> Success<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> values)
        => new(true, [], values);
    public static ParseResult<TKey, TValue> Create<TKey, TValue>(bool isSuccessful, IEnumerable<KeyValuePair<TKey, TValue>> values, IEnumerable<string> errorMessages)
        => new(isSuccessful, errorMessages, values);
}

public class ParseResult<TKey, TValue>
{
    public bool IsSuccessful { get; }
    public IEnumerable<string> ErrorMessages { get; }
    public IEnumerable<KeyValuePair<TKey, TValue>> Values { get; }

    internal ParseResult(bool isSuccessful, IEnumerable<string> errorMessages, IEnumerable<KeyValuePair<TKey, TValue>> values)
    {
        ArgumentGuard.IsNotNull(errorMessages, nameof(errorMessages));
        ArgumentGuard.IsNotNull(values, nameof(values));

        IsSuccessful = isSuccessful;
        ErrorMessages = errorMessages;
        Values = values;
    }
}
