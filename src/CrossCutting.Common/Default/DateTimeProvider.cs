using CrossCutting.Common.Abstractions;

namespace CrossCutting.Common.Default;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetCurrentDateTime() =>
        DateTime.Now;
}
