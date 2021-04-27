using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Utilities.Parsers
{
    public static class ParseResult
    {
        public static ParseResult<TKey, TValue> NotUnderstood<TKey, TValue>() => new ParseResult<TKey, TValue>(false, false, Enumerable.Empty<string>(), Enumerable.Empty<KeyValuePair<TKey, TValue>>());
        public static ParseResult<TKey, TValue> Error<TKey, TValue>(IEnumerable<string> errorMessages) => new ParseResult<TKey, TValue>(true, false, errorMessages, Enumerable.Empty<KeyValuePair<TKey, TValue>>());
        public static ParseResult<TKey, TValue> Error<TKey, TValue>(IEnumerable<string> errorMessages, IEnumerable<KeyValuePair<TKey, TValue>> values) => new ParseResult<TKey, TValue>(true, false, errorMessages, values);
        public static ParseResult<TKey, TValue> Error<TKey, TValue>(string errorMessage) => new ParseResult<TKey, TValue>(true, false, new[] { errorMessage }, Enumerable.Empty<KeyValuePair<TKey, TValue>>());
        public static ParseResult<TKey, TValue> Error<TKey, TValue>(string errorMessage, IEnumerable<KeyValuePair<TKey, TValue>> values) => new ParseResult<TKey, TValue>(true, false, new[] { errorMessage }, values);
        public static ParseResult<TKey, TValue> Success<TKey, TValue>() => new ParseResult<TKey, TValue>(true, true, Enumerable.Empty<string>(), Enumerable.Empty<KeyValuePair<TKey, TValue>>());
        public static ParseResult<TKey, TValue> Success<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> values) => new ParseResult<TKey, TValue>(true, true, Enumerable.Empty<string>(), values);
        public static ParseResult<TKey, TValue> Create<TKey, TValue>(bool isSuccessful, IEnumerable<KeyValuePair<TKey, TValue>> values, IEnumerable<string> errorMessages) => new ParseResult<TKey, TValue>(true, isSuccessful, errorMessages, values);
    }

    public class ParseResult<TKey, TValue>
    {
        public bool Understood { get; }
        public bool IsSuccessful { get; }
        public IEnumerable<string> ErrorMessages { get; }
        public IEnumerable<KeyValuePair<TKey, TValue>> Values { get; }

        internal ParseResult(bool understood, bool isSuccessful, IEnumerable<string> errorMessages, IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            Understood = understood;
            IsSuccessful = isSuccessful;
            ErrorMessages = errorMessages;
            Values = values;
        }
    }
}
