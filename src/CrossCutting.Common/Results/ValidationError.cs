﻿namespace CrossCutting.Common.Results;

public record ValidationError
{
    public string ErrorMessage { get; }
    public IReadOnlyCollection<string> MemberNames { get; }

    public ValidationError(string errorMessage)
        : this(errorMessage, [])
    {
    }

    public ValidationError(string errorMessage, IEnumerable<string> memberNames)
    {
        ErrorMessage = errorMessage;
        MemberNames = new ReadOnlyValueCollection<string>(memberNames);
    }
}
