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
    public static Result<T> Success(T value) => new Result<T>(value, ResultStatus.Ok, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> Error() => new Result<T>(default, ResultStatus.Error, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> Error(string errorMessage) => new Result<T>(default, ResultStatus.Error, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> NotFound() => new Result<T>(default, ResultStatus.NotFound, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> NotFound(string errorMessage) => new Result<T>(default, ResultStatus.NotFound, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> Invalid() => new Result<T>(default, ResultStatus.Invalid, null, Enumerable.Empty<ValidationError>());
    public static new Result<T> Invalid(IEnumerable<ValidationError> validationErrors) => new Result<T>(default, ResultStatus.Invalid, null, validationErrors);
    public static new Result<T> Invalid(string errorMessage) => new Result<T>(default, ResultStatus.Invalid, errorMessage, Enumerable.Empty<ValidationError>());
    public static new Result<T> Invalid(string errorMessage, IEnumerable<ValidationError> validationErrors) => new Result<T>(default, ResultStatus.Invalid, errorMessage, validationErrors);
    public static Result<T> FromExistingResult(Result existingResult) => new Result<T>(default, existingResult.Status, existingResult.ErrorMessage, existingResult.ValidationErrors);

    public T GetValueOrThrow(string errorMessage)
    {
        if (!IsSuccessful())
        {
            throw new InvalidOperationException(errorMessage);
        }
        return Value!;
    }
    public T GetValueOrThrow() => GetValueOrThrow($"Result: {Status}");
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

    public bool IsSuccessful() => Status == ResultStatus.Ok;
    public string? ErrorMessage { get; }
    public ResultStatus Status { get; }
    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

    public static Result Success() => new Result(ResultStatus.Ok, null, Enumerable.Empty<ValidationError>());
    public static Result Error() => new Result(ResultStatus.Error, null, Enumerable.Empty<ValidationError>());
    public static Result Error(string errorMessage) => new Result(ResultStatus.Error, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result NotFound() => new Result(ResultStatus.NotFound, null, Enumerable.Empty<ValidationError>());
    public static Result NotFound(string errorMessage) => new Result(ResultStatus.NotFound, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result Invalid() => new Result(ResultStatus.Invalid, null, Enumerable.Empty<ValidationError>());
    public static Result Invalid(IEnumerable<ValidationError> validationErrors) => new Result(ResultStatus.Invalid, null, validationErrors);
    public static Result Invalid(string errorMessage) => new Result(ResultStatus.Invalid, errorMessage, Enumerable.Empty<ValidationError>());
    public static Result Invalid(string errorMessage, IEnumerable<ValidationError> validationErrors) => new Result(ResultStatus.Invalid, errorMessage, validationErrors);

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

}
