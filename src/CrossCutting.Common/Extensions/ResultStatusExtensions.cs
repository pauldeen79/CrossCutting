namespace CrossCutting.Common.Extensions;

public static class ResultStatusExtensions
{
    public static bool IsSuccessful(this ResultStatus instance)
        => instance is ResultStatus.Ok or ResultStatus.NoContent or ResultStatus.Continue;
}
