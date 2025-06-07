namespace CrossCutting.Common.Tests;

internal sealed record MyResult : Result
{
    public MyResult(object? value) : base(ResultStatus.Ok, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null)
    {
        Value = value;
    }

    public object? Value { get; }

    public override object? GetValue() => Value;
}
