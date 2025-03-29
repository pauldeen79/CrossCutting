namespace CrossCutting.Common.Extensions;

public static class ResultStatusExtensions
{
    public static bool IsSuccessful(this ResultStatus instance)
        => instance is ResultStatus.Ok or ResultStatus.NoContent or ResultStatus.Continue;

    public static Result ToResult(this ResultStatus instance, string? errorMessage = null, IEnumerable<ValidationError>? validationErrors = null, IEnumerable<Result>? innerResults = null, Exception? exception = null)
        => new Result(instance, errorMessage, validationErrors ?? Enumerable.Empty<ValidationError>(), innerResults ?? Enumerable.Empty<Result>(), exception);

    public static Result<T> ToTypedResult<T>(this ResultStatus instance, T? value = default, string? errorMessage = null, IEnumerable<ValidationError>? validationErrors = null, IEnumerable<Result>? innerResults = null, Exception? exception = null)
        => new Result<T>(value, instance, errorMessage, validationErrors ?? Enumerable.Empty<ValidationError>(), innerResults ?? Enumerable.Empty<Result>(), exception);
}
