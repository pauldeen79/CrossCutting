using System.Collections.Generic;

namespace CrossCutting.Utilities.Parsers
{
    public class ParseResult<TKey, TValue>
    {
        public bool IsSuccessful { get; }
        public IEnumerable<string> ErrorMessages { get; }
        public IEnumerable<KeyValuePair<TKey, TValue>> Values { get; }

        public ParseResult(bool isSuccessful, IEnumerable<string> errorMessages, IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            IsSuccessful = isSuccessful;
            ErrorMessages = errorMessages;
            Values = values;
        }
    }
}