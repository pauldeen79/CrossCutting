namespace CrossCutting.Common.Results;

public record Result<T> : Result
{
    internal Result(T? value,
                    ResultStatus status,
                    string? errorMessage,
                    IEnumerable<ValidationError> validationErrors,
                    IEnumerable<Result> innerResults,
                    Exception? exception)
        : base(
            status,
            errorMessage,
            validationErrors,
            innerResults,
            exception)
        => Value = value;

    public override object? GetValue() => Value;

    public T? Value { get; }
    public bool HasValue => Value is not null;

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
            return FromExistingResult<TCast>(this);
        }

        if (Value is not TCast castValue)
        {
            return Invalid<TCast>(errorMessage ?? $"Could not cast {typeof(T).FullName} to {typeof(TCast).FullName}");
        }

        return new Result<TCast>(castValue, Status, ErrorMessage, ValidationErrors, InnerResults, Exception);
    }

    public Result<TTarget> TransformValue<TTarget>(Func<T, TTarget> transformDelegate)
    {
        ArgumentGuard.IsNotNull(transformDelegate, nameof(transformDelegate));

        if (!IsSuccessful())
        {
            return FromExistingResult<TTarget>(this);
        }

        return new Result<TTarget>(transformDelegate(Value!), Status, ErrorMessage, ValidationErrors, InnerResults, Exception);
    }

    public void Either(Action<Result<T>> errorDelegate, Action<Result<T>> successDelegate)
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!IsSuccessful())
        {
            errorDelegate(this);
            return;
        }

        successDelegate(this);
    }

    public Result Either(Func<Result<T>, Result> errorDelegate, Func<Result<T>, Result> successDelegate)
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!IsSuccessful())
        {
            return errorDelegate(this);
        }

        return successDelegate(this);
    }

    public Result<T> Either(Func<Result<T>, Result<T>> errorDelegate)
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));

        if (!IsSuccessful())
        {
            return errorDelegate(this);
        }

        return this;
    }

    public Result<T> Either(Func<Result<T>, Result<T>> errorDelegate, Func<Result<T>, Result<T>> successDelegate)
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!IsSuccessful())
        {
            return errorDelegate(this);
        }

        return successDelegate(this);
    }

    public Result<TResult> Either<TResult>(Func<Result<T>, Result<TResult>> errorDelegate, Func<Result<T>, Result<TResult>> successDelegate)
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!IsSuccessful())
        {
            return errorDelegate(this);
        }

        return successDelegate(this);
    }
}

public record Result
{
    protected Result(ResultStatus status,
                     string? errorMessage,
                     IEnumerable<ValidationError> validationErrors,
                     IEnumerable<Result> innerResults,
                     Exception? exception)
    {
        Status = status;
        ErrorMessage = errorMessage;
        ValidationErrors = new ValueCollection<ValidationError>(validationErrors.IsNotNull(nameof(validationErrors)));
        InnerResults = new ValueCollection<Result>(innerResults.IsNotNull(nameof(innerResults)));
        Exception = exception;
    }

    public virtual object? GetValue() => null;

    public bool IsSuccessful() => Status == ResultStatus.Ok || Status == ResultStatus.NoContent || Status == ResultStatus.Continue;
    public string? ErrorMessage { get; }
    public Exception? Exception { get; }
    public ResultStatus Status { get; }
    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }
    public IReadOnlyCollection<Result> InnerResults { get; }

    public static Result Success() => new(ResultStatus.Ok, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Success<T>(T value) => new(value, ResultStatus.Ok, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Error() => new(ResultStatus.Error, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Error(string errorMessage) => new(ResultStatus.Error, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Error(Exception exception, string errorMessage) => new(ResultStatus.Error, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), exception);
    public static Result Error(IEnumerable<Result> innerResults, string errorMessage) => new(ResultStatus.Error, errorMessage, Enumerable.Empty<ValidationError>(), innerResults, null);
    public static Result<T> Error<T>() => new(default, ResultStatus.Error, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Error<T>(string errorMessage) => new(default, ResultStatus.Error, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Error<T>(Exception exception, string errorMessage) => new(default, ResultStatus.Error, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), exception);
    public static Result<T> Error<T>(IEnumerable<Result> innerResults, string errorMessage) => new(default, ResultStatus.Error, errorMessage, Enumerable.Empty<ValidationError>(), innerResults, null);
    public static Result NotFound() => new(ResultStatus.NotFound, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result NotFound(string errorMessage) => new(ResultStatus.NotFound, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> NotFound<T>() => new(default, ResultStatus.NotFound, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> NotFound<T>(string errorMessage) => new(default, ResultStatus.NotFound, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Invalid() => new(ResultStatus.Invalid, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Invalid(IEnumerable<ValidationError> validationErrors) => new(ResultStatus.Invalid, null, validationErrors, Enumerable.Empty<Result>(), null);
    public static Result Invalid(IEnumerable<Result> innerResults) => new(ResultStatus.Invalid, null, Enumerable.Empty<ValidationError>(), innerResults, null);
    public static Result Invalid(string errorMessage) => new(ResultStatus.Invalid, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Invalid(string errorMessage, IEnumerable<ValidationError> validationErrors) => new(ResultStatus.Invalid, errorMessage, validationErrors, Enumerable.Empty<Result>(), null);
    public static Result Invalid(string errorMessage, IEnumerable<Result> innerResults) => new(ResultStatus.Invalid, errorMessage, Enumerable.Empty<ValidationError>(), innerResults, null);
    public static Result<T> Invalid<T>() => new(default, ResultStatus.Invalid, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Invalid<T>(IEnumerable<ValidationError> validationErrors) => new(default, ResultStatus.Invalid, null, validationErrors, Enumerable.Empty<Result>(), null);
    public static Result<T> Invalid<T>(string errorMessage) => new Result<T>(default, ResultStatus.Invalid, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Invalid<T>(string errorMessage, IEnumerable<ValidationError> validationErrors) => new(default, ResultStatus.Invalid, errorMessage, validationErrors, Enumerable.Empty<Result>(), null);
    public static Result<T> Invalid<T>(string errorMessage, IEnumerable<Result> innerResults) => new(default, ResultStatus.Invalid, errorMessage, Enumerable.Empty<ValidationError>(), innerResults, null);
    public static Result Unauthorized() => new(ResultStatus.Unauthorized, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Unauthorized(string errorMessage) => new(ResultStatus.Unauthorized, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Unauthorized<T>() => new(default, ResultStatus.Unauthorized, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Unauthorized<T>(string errorMessage) => new(default, ResultStatus.Unauthorized, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result NotAuthenticated() => new(ResultStatus.NotAuthenticated, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result NotAuthenticated(string errorMessage) => new(ResultStatus.NotAuthenticated, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> NotAuthenticated<T>() => new(default, ResultStatus.NotAuthenticated, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> NotAuthenticated<T>(string errorMessage) => new(default, ResultStatus.NotAuthenticated, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result NotSupported() => new(ResultStatus.NotSupported, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result NotSupported(string errorMessage) => new(ResultStatus.NotSupported, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> NotSupported<T>() => new(default, ResultStatus.NotSupported, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> NotSupported<T>(string errorMessage) => new(default, ResultStatus.NotSupported, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Unavailable() => new(ResultStatus.Unavailable, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Unavailable(string errorMessage) => new(ResultStatus.Unavailable, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Unavailable<T>() => new(default, ResultStatus.Unavailable, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Unavailable<T>(string errorMessage) => new(default, ResultStatus.Unavailable, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result NotImplemented() => new(ResultStatus.NotImplemented, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result NotImplemented(string errorMessage) => new(ResultStatus.NotImplemented, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> NotImplemented<T>() => new(default, ResultStatus.NotImplemented, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> NotImplemented<T>(string errorMessage) => new(default, ResultStatus.NotImplemented, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result NoContent() => new(ResultStatus.NoContent, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result NoContent(string errorMessage) => new(ResultStatus.NoContent, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> NoContent<T>() => new(default, ResultStatus.NoContent, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> NoContent<T>(string errorMessage) => new(default, ResultStatus.NoContent, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result ResetContent() => new(ResultStatus.ResetContent, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result ResetContent(string errorMessage) => new(ResultStatus.ResetContent, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> ResetContent<T>() => new(default, ResultStatus.ResetContent, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> ResetContent<T>(string errorMessage) => new(default, ResultStatus.ResetContent, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Continue() => new(ResultStatus.Continue, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Continue<T>() => new(default, ResultStatus.Continue, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Continue<T>(T value) => new(value, ResultStatus.Continue, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Conflict() => new(ResultStatus.Conflict, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Conflict(string errorMessage) => new(ResultStatus.Conflict, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result Conflict(IEnumerable<Result> innerResults, string errorMessage) => new(ResultStatus.Conflict, errorMessage, Enumerable.Empty<ValidationError>(), innerResults, null);
    public static Result<T> Conflict<T>() => new(default, ResultStatus.Conflict, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Conflict<T>(string errorMessage) => new(default, ResultStatus.Conflict, errorMessage, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);
    public static Result<T> Conflict<T>(IEnumerable<Result> innerResults, string errorMessage) => new(default, ResultStatus.Conflict, errorMessage, Enumerable.Empty<ValidationError>(), innerResults, null);

    public static Result<T> Redirect<T>(T value) => new(value, ResultStatus.Redirect, null, Enumerable.Empty<ValidationError>(), Enumerable.Empty<Result>(), null);

    public static Result<T> FromExistingResult<T>(Result existingResult) => new(TryGetValue<T>(ArgumentGuard.IsNotNull(existingResult, nameof(existingResult))), existingResult.Status, existingResult.ErrorMessage, existingResult.ValidationErrors, existingResult.InnerResults, null);
    public static Result<T> FromExistingResult<T>(Result existingResult, T value) => new(value, ArgumentGuard.IsNotNull(existingResult, nameof(existingResult)).Status, existingResult.ErrorMessage, existingResult.ValidationErrors, existingResult.InnerResults, null);
    public static Result<TTargetResult> FromExistingResult<TSourceResult, TTargetResult>(Result<TSourceResult> existingResult, Func<TSourceResult, TTargetResult> convertDelegate)
    {
        ArgumentGuard.IsNotNull(existingResult, nameof(existingResult));
        ArgumentGuard.IsNotNull(convertDelegate, nameof(convertDelegate));

        return existingResult.IsSuccessful()
            ? Success(convertDelegate(existingResult.Value!))
            : FromExistingResult<TTargetResult>(existingResult);
    }

    public static Result<TInstance> FromInstance<TInstance>(TInstance? instance)
        where TInstance : class
        => instance == default
            ? NotFound<TInstance>()
            : Success(instance);

    public static Result<TInstance> FromInstance<TInstance>(TInstance? instance, string errorMessage)
        where TInstance : class
        => instance == default
            ? NotFound<TInstance>(errorMessage)
            : Success(instance);

    public static Result<TInstance> FromInstance<TInstance>(TInstance? instance, string errorMessage, IEnumerable<ValidationError> validationErrors)
        where TInstance : class
        => instance == default
            ? Invalid<TInstance>(errorMessage, validationErrors)
            : Success(instance);

    public static Result<TInstance> FromInstance<TInstance>(TInstance? instance, IEnumerable<ValidationError> validationErrors)
        where TInstance : class
        => instance == default
            ? Invalid<TInstance>(validationErrors)
            : Success(instance);

    public static Result FromValidationErrors(IEnumerable<ValidationError> validationErrors)
        => validationErrors.Any()
            ? Invalid(validationErrors)
            : Success();

    public static Result FromValidationErrors(IEnumerable<ValidationError> validationErrors, string errorMessage)
        => validationErrors.Any()
            ? Invalid(errorMessage, validationErrors)
            : Success();

    public static Result Aggregate(IEnumerable<Result> innerResults, Result successResult, Func<Result[], Result> errorResultDelegate)
    {
        ArgumentGuard.IsNotNull(errorResultDelegate, nameof(errorResultDelegate));

        var errors = innerResults.Where(x => !x.IsSuccessful()).ToArray();
        if (errors.Length == 0)
        {
            return successResult;
        }

        return errorResultDelegate(errors);
    }

    public static Result<T> Aggregate<T>(IEnumerable<Result> innerResults, Result<T> successResult, Func<Result[], Result<T>> errorResultDelegate)
    {
        ArgumentGuard.IsNotNull(errorResultDelegate, nameof(errorResultDelegate));

        var errors = innerResults.Where(x => !x.IsSuccessful()).ToArray();
        if (errors.Length == 0)
        {
            return successResult;
        }

        return errorResultDelegate(errors);
    }

    private static T? TryGetValue<T>(Result existingResult) => existingResult.GetValue() is T t
        ? t
        : default;

    public void ThrowIfInvalid(string errorMessage)
    {
        if (!IsSuccessful())
        {
            throw new InvalidOperationException(errorMessage);
        }
    }

    public void ThrowIfInvalid()
        => ThrowIfInvalid(string.IsNullOrEmpty(ErrorMessage)
            ? $"Result: {Status}"
            : $"Result: {Status}, ErrorMessage: {ErrorMessage}");

    public void Either(Action<Result> errorDelegate, Action<Result> successDelegate)
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!IsSuccessful())
        {
            errorDelegate(this);
            return;
        }

        successDelegate(this);
    }
}
