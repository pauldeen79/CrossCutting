using System.Collections.Generic;

namespace System.Data.Stub
{
    public class NextResultCalledEventArgs : EventArgs
    {
        public bool Result { get; set; }
        public int? CurrentIndex { get; set; }
        public Dictionary<int, IDictionary<string, object>> Dictionary { get; set; }
    }
}
