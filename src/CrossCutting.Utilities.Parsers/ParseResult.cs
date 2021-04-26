using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Utilities.Parsers
{
    public class ParseResult<TKey, TValue>
    {
        public bool Understood { get; }

        public bool IsSuccessful { get; }
        public IEnumerable<string> ErrorMessages { get; }
        public IEnumerable<KeyValuePair<TKey, TValue>> Values { get; }

        public ParseResult(bool isSuccessful, IEnumerable<string> errorMessages, IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            IsSuccessful = isSuccessful;
            ErrorMessages = errorMessages;
            Values = values;
        }

        public ParseResult()
        {
            Understood = false;
            IsSuccessful = false;
            ErrorMessages = Enumerable.Empty<string>();
            Values = Enumerable.Empty<KeyValuePair<TKey, TValue>>();
        }
    }
}
