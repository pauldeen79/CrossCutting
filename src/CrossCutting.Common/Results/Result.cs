using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Common.Results;

public record Result<T> : Result
{
    private Result(T? value,
                   ResultStatus status,
                   string? errorMessage,
                   IEnumerable<ValidationError> validationErrors)
        : base(
            status,
            errorMessage,
            validationErrors)
        => Value = value;

    public T? Value { get; }
    public bool HasValue => Value != null;
    public static Result<T> Success(T value) => new(value, ResultStatus.Ok, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> Error() => new(default, ResultStatus.Error, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> Error(string errorMessage) => new(default, ResultStatus.Error, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> NotFound() => new(default, ResultStatus.NotFound, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> NotFound(string errorMessage) => new(default, ResultStatus.NotFound, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> Invalid() => new(default, ResultStatus.Invalid, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> Invalid(IEnumerable<ValidationError> validationErrors) => new(default, ResultStatus.Invalid, null, validationErrors);
    public static new Result<T> Invalid(string errorMessage) => new Result<T>(default, ResultStatus.Invalid, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> Invalid(string errorMessage, IEnumerable<ValidationError> validationErrors) => new(default, ResultStatus.Invalid, errorMessage, validationErrors);
    public static new Result<T> Unauthorized() => new(default, ResultStatus.Unauthorized, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> Unauthorized(string errorMessage) => new(default, ResultStatus.Unauthorized, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> NotAuthenticated() => new(default, ResultStatus.NotAuthenticated, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> NotAuthenticated(string errorMessage) => new(default, ResultStatus.NotAuthenticated, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> NotSupported() => new(default, ResultStatus.NotSupported, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> NotSupported(string errorMessage) => new(default, ResultStatus.NotSupported, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> Unavailable() => new(default, ResultStatus.Unavailable, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> Unavailable(string errorMessage) => new(default, ResultStatus.Unavailable, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> NotImplemented() => new(default, ResultStatus.NotImplemented, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> NotImplemented(string errorMessage) => new(default, ResultStatus.NotImplemented, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> NoContent() => new(default, ResultStatus.NoContent, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> NoContent(string errorMessage) => new(default, ResultStatus.NoContent, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> ResetContent() => new(default, ResultStatus.ResetContent, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> ResetContent(string errorMessage) => new(default, ResultStatus.ResetContent, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> Continue() => new(default, ResultStatus.Continue, null, Enumerable.Empty<ValidationError>());
    public static Result<T> Redirect(T value) => new(value, ResultStatus.Redirect, null, Enumerable.Empty<ValidationError>());
    public static Result<T> FromExistingResult(Result existingResult) => new(default, existingResult.Status, existingResult.ErrorMessage, existingResult.ValidationErrors);
    public static Result<T> FromExistingResult(Result existingResult, T value) => new(value, existingResult.Status, existingResult.ErrorMessage, existingResult.ValidationErrors);
    public static Result<T> FromExistingResult<TSourceResult>(Result<TSourceResult> existingResult, Func<TSourceResult, T> convertDelegate)
        => existingResult.IsSuccessful()
            ? Result<T>.Success(convertDelegate(existingResult.Value!))
            : Result<T>.FromExistingResult(existingResult);

    public T GetValueOrThrow(string errorMessage)
        => !IsSuccessful() || Value is null
            ? throw new InvalidOperationException(errorMessage)
            : Value;

    public T GetValueOrThrow()
        => GetValueOrThrow(string.IsNullOrEmpty(ErrorMessage)
            ? $"Result: {Status}"
            : $"Result: {Status}, ErrorMessage: {ErrorMessage}");

    public Result<TCast> TryCast<TCast>(string? errorMessage = null)
    {
        if (!IsSuccessful())
        {
            return Result<TCast>.FromExistingResult(this);
        }

        if (Value is not TCast castValue)
        {
            return errorMessage == null
                ? Result<TCast>.Invalid()
                : Result<TCast>.Invalid(errorMessage);
        }

        return Result<TCast>.Success(castValue);
    }
}

public record Result
{
    protected Result(ResultStatus status,
                     string? errorMessage,
                     IEnumerable<ValidationError> validationErrors)
    {
        Status = status;
        ErrorMessage = errorMessage;
        ValidationErrors = new ValueCollection<ValidationError>(validationErrors);
    }

    public bool IsSuccessful() => Status == ResultStatus.Ok || Status == ResultStatus.NoContent || Status == ResultStatus.Continue;
    public string? ErrorMessage { get; }
    public ResultStatus Status { get; }
    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

    public static Result Success() => new(ResultStatus.Ok, null, Enumerable.Empty<ValidationError>());
    public static Result Error() => new(ResultStatus.Error, null, Enumerable.Empty<ValidationError>());
    public static Result Error(string errorMessage) => new(ResultStatus.Error, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result NotFound() => new(ResultStatus.NotFound, null, Enumerable.Empty<ValidationError>());
    public static Result NotFound(string errorMessage) => new(ResultStatus.NotFound, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result Invalid() => new(ResultStatus.Invalid, null, Enumerable.Empty<ValidationError>());
    public static Result Invalid(IEnumerable<ValidationError> validationErrors) => new(ResultStatus.Invalid, null, validationErrors);
    public static Result Invalid(string errorMessage) => new(ResultStatus.Invalid, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result Invalid(string errorMessage, IEnumerable<ValidationError> validationErrors) => new(ResultStatus.Invalid, errorMessage, validationErrors);
    public static Result Unauthorized() => new(ResultStatus.Unauthorized, null, Enumerable.Empty<ValidationError>());
    public static Result Unauthorized(string errorMessage) => new(ResultStatus.Unauthorized, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result NotAuthenticated() => new(ResultStatus.NotAuthenticated, null, Enumerable.Empty<ValidationError>());
    public static Result NotAuthenticated(string errorMessage) => new(ResultStatus.NotAuthenticated, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result NotSupported() => new(ResultStatus.NotSupported, null, Enumerable.Empty<ValidationError>());
    public static Result NotSupported(string errorMessage) => new(ResultStatus.NotSupported, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result Unavailable() => new(ResultStatus.Unavailable, null, Enumerable.Empty<ValidationError>());
    public static Result Unavailable(string errorMessage) => new(ResultStatus.Unavailable, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result NotImplemented() => new(ResultStatus.NotImplemented, null, Enumerable.Empty<ValidationError>());
    public static Result NotImplemented(string errorMessage) => new(ResultStatus.NotImplemented, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result NoContent() => new(ResultStatus.NoContent, null, Enumerable.Empty<ValidationError>());
    public static Result NoContent(string errorMessage) => new(ResultStatus.NoContent, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result ResetContent() => new(ResultStatus.ResetContent, null, Enumerable.Empty<ValidationError>());
    public static Result ResetContent(string errorMessage) => new(ResultStatus.ResetContent, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result Continue() => new(ResultStatus.Continue, null, Enumerable.Empty<ValidationError>());

    public static Result<TInstance> FromInstance<TInstance>(TInstance? instance)
        where TInstance : class
        => instance == default
            ? Result<TInstance>.NotFound()
            : Result<TInstance>.Success(instance);

    public static Result<TInstance> FromInstance<TInstance>(TInstance? instance, string errorMessage)
        where TInstance : class
        => instance == default
            ? Result<TInstance>.NotFound(errorMessage)
            : Result<TInstance>.Success(instance);

    public static Result<TInstance> FromInstance<TInstance>(TInstance? instance, string errorMessage, IEnumerable<ValidationError> validationErrors)
        where TInstance : class
        => instance == default
            ? Result<TInstance>.Invalid(errorMessage, validationErrors)
            : Result<TInstance>.Success(instance);

    public static Result<TInstance> FromInstance<TInstance>(TInstance? instance, IEnumerable<ValidationError> validationErrors)
        where TInstance : class
        => instance == default
            ? Result<TInstance>.Invalid(validationErrors)
            : Result<TInstance>.Success(instance);

    public static Result FromValidationErrors(IEnumerable<ValidationError> validationErrors)
        => validationErrors.Any()
            ? Invalid(validationErrors)
            : Success();

    public static Result FromValidationErrors(IEnumerable<ValidationError> validationErrors, string errorMessage)
        => validationErrors.Any()
            ? Invalid(errorMessage, validationErrors)
            : Success();
}
