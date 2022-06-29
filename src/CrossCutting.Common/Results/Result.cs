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
    public static Result<T> Redirect(T value) => new(value, ResultStatus.Redirect, null, Enumerable.Empty<ValidationError>());
    public static Result<T> FromExistingResult(Result existingResult) => new(default, existingResult.Status, existingResult.ErrorMessage, existingResult.ValidationErrors);

    public static Result<T> Chain<TCommand>(TCommand command, params Func<TCommand, Result<T>>[] steps)
        => steps.Length == 0
            ? Error("Could not determine result because there are no steps defined")
            : steps.Select(step => step.Invoke(command))
                   .TakeWhileWithFirstNonMatching(result => result.IsSuccessful())
                   .Last();

    public static Result<T> Chain<TCommand>(TCommand command, params Func<TCommand, Result<T>, Result<T>>[] steps)
        => steps.Aggregate
        (
            Error("Could not determine result because there are no steps defined"),
            (seed, step) => step.Invoke(command, seed)
        );

    public T GetValueOrThrow(string errorMessage)
    {
        if (Value == null)
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

    public static Result Chain<TCommand>(TCommand command, params Func<TCommand, Result>[] steps)
        => steps.Length == 0
            ? Error("Could not determine result because there are no steps defined")
            : steps.Select(step => step.Invoke(command))
                   .TakeWhileWithFirstNonMatching(result => result.IsSuccessful())
                   .Last();

    public static Result Chain<TCommand>(TCommand command, params Func<TCommand, Result, Result>[] steps)
        => steps.Aggregate
        (
            Error("Could not determine result because there are no steps defined"),
            (seed, step) => step.Invoke(command, seed)
        );
}
