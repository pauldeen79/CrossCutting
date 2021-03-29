using CrossCutting.Common.Abstractions;
using System;

namespace CrossCutting.Common.Default
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetCurrentDateTime() =>
            DateTime.Now;
    }
}
