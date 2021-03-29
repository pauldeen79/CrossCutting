using System;

namespace CrossCutting.Common.Abstractions
{
    public interface IDateTimeProvider
    {
        DateTime GetCurrentDateTime();
    }
}
