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

    public Result<TTarget> Transform<TTarget>(Func<T, TTarget> transformDelegate)
    {
        ArgumentGuard.IsNotNull(transformDelegate, nameof(transformDelegate));

        if (!IsSuccessful())
        {
            return FromExistingResult<TTarget>(this);
        }

        return Success(transformDelegate(Value!));
    }

    public Result<TTarget> Transform<TTarget>(Func<T, Result<TTarget>> transformDelegate)
    {
        ArgumentGuard.IsNotNull(transformDelegate, nameof(transformDelegate));

        if (!IsSuccessful())
        {
            return FromExistingResult<TTarget>(this);
        }

        return transformDelegate(Value!);
    }

    public Result Transform(Func<T, Result> transformDelegate)
    {
        ArgumentGuard.IsNotNull(transformDelegate, nameof(transformDelegate));

        if (!IsSuccessful())
        {
            return this;
        }

        return transformDelegate(Value!);
    }

    public Result<T> Either(Func<Result<T>, Result<T>> errorDelegate)
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        if (!IsSuccessful())
        {
            return errorDelegate(this);
        }

        return this;
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
        ArgumentGuard.IsNotNull(validationErrors, nameof(validationErrors));
        ArgumentGuard.IsNotNull(innerResults, nameof(innerResults));

        Status = status;
        ErrorMessage = errorMessage;
        ValidationErrors = new ValueCollection<ValidationError>(validationErrors);
        InnerResults = new ValueCollection<Result>(innerResults);
        Exception = exception;
    }

    public virtual object? GetValue() => null;

    public Result<TCast> TryCast<TCast>(string? errorMessage = null)
    {
        if (!IsSuccessful())
        {
            return FromExistingResult<TCast>(this);
        }

        var value = GetValue();
        if (value is null)
        {
            return new Result<TCast>(default, Status, ErrorMessage, ValidationErrors, InnerResults, Exception);
        }

        if (value is not TCast castValue)
        {
            return Invalid<TCast>(errorMessage ?? $"Could not cast {value.GetType().FullName} to {typeof(TCast).FullName}");
        }

        return new Result<TCast>(castValue, Status, ErrorMessage, ValidationErrors, InnerResults, Exception);
    }

    public T? TryCastValueAs<T>()
    {
        var value = GetValue();
        if (value is T t)
        {
            return t;
        }

        return default;
    }

    public T? TryCastValueAs<T>(T? defaultValue)
    {
        var value = GetValue();
        if (value is T t)
        {
            return t;
        }

        return defaultValue;
    }

    public T CastValueAs<T>()
    {
        var value = GetValue();
        if (value is null && Nullable.GetUnderlyingType(typeof(T)) is null)
        {
            throw new InvalidOperationException("Value is null");
        }

        return (T)value!;
    }

    public bool IsSuccessful() => Status is ResultStatus.Ok or ResultStatus.NoContent or ResultStatus.Continue;
    public string? ErrorMessage { get; }
    public Exception? Exception { get; }
    public ResultStatus Status { get; }
    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }
    public IReadOnlyCollection<Result> InnerResults { get; }

    public static Result Success() => new(ResultStatus.Ok, null, [], [], null);
    public static Result<T> Success<T>(T value) => new(value, ResultStatus.Ok, null, [], [], null);
    public static Result Error() => new(ResultStatus.Error, null, [], [], null);
    public static Result Error(string errorMessage) => new(ResultStatus.Error, errorMessage, [], [], null);
    public static Result Error(Exception exception, string errorMessage) => new(ResultStatus.Error, errorMessage, [], [], exception);
    public static Result Error(IEnumerable<Result> innerResults, string errorMessage) => new(ResultStatus.Error, errorMessage, [], innerResults, null);
    public static Result<T> Error<T>() => new(default, ResultStatus.Error, null, [], [], null);
    public static Result<T> Error<T>(string errorMessage) => new(default, ResultStatus.Error, errorMessage, [], [], null);
    public static Result<T> Error<T>(Exception exception, string errorMessage) => new(default, ResultStatus.Error, errorMessage, [], [], exception);
    public static Result<T> Error<T>(IEnumerable<Result> innerResults, string errorMessage) => new(default, ResultStatus.Error, errorMessage, [], innerResults, null);
    public static Result NotFound() => new(ResultStatus.NotFound, null, [], [], null);
    public static Result NotFound(string errorMessage) => new(ResultStatus.NotFound, errorMessage, [], [], null);
    public static Result<T> NotFound<T>() => new(default, ResultStatus.NotFound, null, [], [], null);
    public static Result<T> NotFound<T>(string errorMessage) => new(default, ResultStatus.NotFound, errorMessage, [], [], null);
    public static Result Invalid() => new(ResultStatus.Invalid, null, [], [], null);
    public static Result Invalid(IEnumerable<ValidationError> validationErrors) => new(ResultStatus.Invalid, null, validationErrors, [], null);
    public static Result Invalid(IEnumerable<Result> innerResults) => new(ResultStatus.Invalid, null, [], innerResults, null);
    public static Result Invalid(string errorMessage) => new(ResultStatus.Invalid, errorMessage, [], [], null);
    public static Result Invalid(string errorMessage, IEnumerable<ValidationError> validationErrors) => new(ResultStatus.Invalid, errorMessage, validationErrors, [], null);
    public static Result Invalid(string errorMessage, IEnumerable<Result> innerResults) => new(ResultStatus.Invalid, errorMessage, [], innerResults, null);
    public static Result<T> Invalid<T>() => new(default, ResultStatus.Invalid, null, [], [], null);
    public static Result<T> Invalid<T>(IEnumerable<ValidationError> validationErrors) => new(default, ResultStatus.Invalid, null, validationErrors, [], null);
    public static Result<T> Invalid<T>(string errorMessage) => new(default, ResultStatus.Invalid, errorMessage, [], [], null);
    public static Result<T> Invalid<T>(string errorMessage, IEnumerable<ValidationError> validationErrors) => new(default, ResultStatus.Invalid, errorMessage, validationErrors, [], null);
    public static Result<T> Invalid<T>(string errorMessage, IEnumerable<Result> innerResults) => new(default, ResultStatus.Invalid, errorMessage, [], innerResults, null);
    public static Result Unauthorized() => new(ResultStatus.Unauthorized, null, [], [], null);
    public static Result Unauthorized(string errorMessage) => new(ResultStatus.Unauthorized, errorMessage, [], [], null);
    public static Result<T> Unauthorized<T>() => new(default, ResultStatus.Unauthorized, null, [], [], null);
    public static Result<T> Unauthorized<T>(string errorMessage) => new(default, ResultStatus.Unauthorized, errorMessage, [], [], null);
    public static Result NotAuthenticated() => new(ResultStatus.NotAuthenticated, null, [], [], null);
    public static Result NotAuthenticated(string errorMessage) => new(ResultStatus.NotAuthenticated, errorMessage, [], [], null);
    public static Result<T> NotAuthenticated<T>() => new(default, ResultStatus.NotAuthenticated, null, [], [], null);
    public static Result<T> NotAuthenticated<T>(string errorMessage) => new(default, ResultStatus.NotAuthenticated, errorMessage, [], [], null);
    public static Result NotSupported() => new(ResultStatus.NotSupported, null, [], [], null);
    public static Result NotSupported(string errorMessage) => new(ResultStatus.NotSupported, errorMessage, [], [], null);
    public static Result<T> NotSupported<T>() => new(default, ResultStatus.NotSupported, null, [], [], null);
    public static Result<T> NotSupported<T>(string errorMessage) => new(default, ResultStatus.NotSupported, errorMessage, [], [], null);
    public static Result Unavailable() => new(ResultStatus.Unavailable, null, [], [], null);
    public static Result Unavailable(string errorMessage) => new(ResultStatus.Unavailable, errorMessage, [], [], null);
    public static Result<T> Unavailable<T>() => new(default, ResultStatus.Unavailable, null, [], [], null);
    public static Result<T> Unavailable<T>(string errorMessage) => new(default, ResultStatus.Unavailable, errorMessage, [], [], null);
    public static Result NotImplemented() => new(ResultStatus.NotImplemented, null, [], [], null);
    public static Result NotImplemented(string errorMessage) => new(ResultStatus.NotImplemented, errorMessage, [], [], null);
    public static Result<T> NotImplemented<T>() => new(default, ResultStatus.NotImplemented, null, [], [], null);
    public static Result<T> NotImplemented<T>(string errorMessage) => new(default, ResultStatus.NotImplemented, errorMessage, [], [], null);
    public static Result NoContent() => new(ResultStatus.NoContent, null, [], [], null);
    public static Result NoContent(string errorMessage) => new(ResultStatus.NoContent, errorMessage, [], [], null);
    public static Result<T> NoContent<T>() => new(default, ResultStatus.NoContent, null, [], [], null);
    public static Result<T> NoContent<T>(string errorMessage) => new(default, ResultStatus.NoContent, errorMessage, [], [], null);
    public static Result ResetContent() => new(ResultStatus.ResetContent, null, [], [], null);
    public static Result ResetContent(string errorMessage) => new(ResultStatus.ResetContent, errorMessage, [], [], null);
    public static Result<T> ResetContent<T>() => new(default, ResultStatus.ResetContent, null, [], [], null);
    public static Result<T> ResetContent<T>(string errorMessage) => new(default, ResultStatus.ResetContent, errorMessage, [], [], null);
    public static Result Continue() => new(ResultStatus.Continue, null, [], [], null);
    public static Result<T> Continue<T>() => new(default, ResultStatus.Continue, null, [], [], null);
    public static Result<T> Continue<T>(T value) => new(value, ResultStatus.Continue, null, [], [], null);
    public static Result Conflict() => new(ResultStatus.Conflict, null, [], [], null);
    public static Result Conflict(string errorMessage) => new(ResultStatus.Conflict, errorMessage, [], [], null);
    public static Result Conflict(IEnumerable<Result> innerResults, string errorMessage) => new(ResultStatus.Conflict, errorMessage, [], innerResults, null);
    public static Result<T> Conflict<T>() => new(default, ResultStatus.Conflict, null, [], [], null);
    public static Result<T> Conflict<T>(string errorMessage) => new(default, ResultStatus.Conflict, errorMessage, [], [], null);
    public static Result<T> Conflict<T>(IEnumerable<Result> innerResults, string errorMessage) => new(default, ResultStatus.Conflict, errorMessage, [], innerResults, null);
    public static Result Created() => new(ResultStatus.Created, null, [], [], null);
    public static Result<T> Created<T>(T value) => new(value, ResultStatus.Created, null, [], [], null);
    public static Result Accepted() => new(ResultStatus.Accepted, null, [], [], null);
    public static Result<T> Accepted<T>(T value) => new(value, ResultStatus.Accepted, null, [], [], null);
    public static Result AlreadyReported() => new(ResultStatus.AlreadyReported, null, [], [], null);
    public static Result<T> AlreadyReported<T>(T value) => new(value, ResultStatus.AlreadyReported, null, [], [], null);
    public static Result Found() => new(ResultStatus.Found, null, [], [], null);
    public static Result Found(string errorMessage) => new(ResultStatus.Found, errorMessage, [], [], null);
    public static Result<T> Found<T>() => new(default, ResultStatus.Found, null, [], [], null);
    public static Result<T> Found<T>(string errorMessage) => new(default, ResultStatus.Found, errorMessage, [], [], null);
    public static Result MovedPermanently() => new(ResultStatus.MovedPermanently, null, [], [], null);
    public static Result MovedPermanently(string errorMessage) => new(ResultStatus.MovedPermanently, errorMessage, [], [], null);
    public static Result<T> MovedPermanently<T>() => new(default, ResultStatus.MovedPermanently, null, [], [], null);
    public static Result<T> MovedPermanently<T>(string errorMessage) => new(default, ResultStatus.MovedPermanently, errorMessage, [], [], null);
    public static Result Gone() => new(ResultStatus.Gone, null, [], [], null);
    public static Result Gone(string errorMessage) => new(ResultStatus.Gone, errorMessage, [], [], null);
    public static Result<T> Gone<T>() => new(default, ResultStatus.Gone, null, [], [], null);
    public static Result<T> Gone<T>(string errorMessage) => new(default, ResultStatus.Gone, errorMessage, [], [], null);
    public static Result NotModified() => new(ResultStatus.NotModified, null, [], [], null);
    public static Result NotModified(string errorMessage) => new(ResultStatus.NotModified, errorMessage, [], [], null);
    public static Result<T> NotModified<T>() => new(default, ResultStatus.NotModified, null, [], [], null);
    public static Result<T> NotModified<T>(string errorMessage) => new(default, ResultStatus.NotModified, errorMessage, [], [], null);
    public static Result TemporaryRedirect() => new(ResultStatus.TemporaryRedirect, null, [], [], null);
    public static Result TemporaryRedirect(string errorMessage) => new(ResultStatus.TemporaryRedirect, errorMessage, [], [], null);
    public static Result<T> TemporaryRedirect<T>() => new(default, ResultStatus.TemporaryRedirect, null, [], [], null);
    public static Result<T> TemporaryRedirect<T>(string errorMessage) => new(default, ResultStatus.TemporaryRedirect, errorMessage, [], [], null);
    public static Result PermanentRedirect() => new(ResultStatus.PermanentRedirect, null, [], [], null);
    public static Result PermanentRedirect(string errorMessage) => new(ResultStatus.PermanentRedirect, errorMessage, [], [], null);
    public static Result<T> PermanentRedirect<T>() => new(default, ResultStatus.PermanentRedirect, null, [], [], null);
    public static Result<T> PermanentRedirect<T>(string errorMessage) => new(default, ResultStatus.PermanentRedirect, errorMessage, [], [], null);
    public static Result Forbidden() => new(ResultStatus.Forbidden, null, [], [], null);
    public static Result Forbidden(string errorMessage) => new(ResultStatus.Forbidden, errorMessage, [], [], null);
    public static Result<T> Forbidden<T>() => new(default, ResultStatus.Forbidden, null, [], [], null);
    public static Result<T> Forbidden<T>(string errorMessage) => new(default, ResultStatus.Forbidden, errorMessage, [], [], null);
    public static Result NotAcceptable() => new(ResultStatus.NotAcceptable, null, [], [], null);
    public static Result NotAcceptable(string errorMessage) => new(ResultStatus.NotAcceptable, errorMessage, [], [], null);
    public static Result<T> NotAcceptable<T>() => new(default, ResultStatus.NotAcceptable, null, [], [], null);
    public static Result<T> NotAcceptable<T>(string errorMessage) => new(default, ResultStatus.NotAcceptable, errorMessage, [], [], null);
    public static Result TimeOut() => new(ResultStatus.TimeOut, null, [], [], null);
    public static Result TimeOut(string errorMessage) => new(ResultStatus.TimeOut, errorMessage, [], [], null);
    public static Result<T> TimeOut<T>() => new(default, ResultStatus.TimeOut, null, [], [], null);
    public static Result<T> TimeOut<T>(string errorMessage) => new(default, ResultStatus.TimeOut, errorMessage, [], [], null);
    public static Result Locked() => new(ResultStatus.Locked, null, [], [], null);
    public static Result Locked(string errorMessage) => new(ResultStatus.Locked, errorMessage, [], [], null);
    public static Result<T> Locked<T>() => new(default, ResultStatus.Locked, null, [], [], null);
    public static Result<T> Locked<T>(string errorMessage) => new(default, ResultStatus.Locked, errorMessage, [], [], null);
    public static Result ServiceUnavailable() => new(ResultStatus.ServiceUnavailable, null, [], [], null);
    public static Result ServiceUnavailable(string errorMessage) => new(ResultStatus.ServiceUnavailable, errorMessage, [], [], null);
    public static Result<T> ServiceUnavailable<T>() => new(default, ResultStatus.ServiceUnavailable, null, [], [], null);
    public static Result<T> ServiceUnavailable<T>(string errorMessage) => new(default, ResultStatus.ServiceUnavailable, errorMessage, [], [], null);
    public static Result GatewayTimeout() => new(ResultStatus.GatewayTimeout, null, [], [], null);
    public static Result GatewayTimeout(string errorMessage) => new(ResultStatus.GatewayTimeout, errorMessage, [], [], null);
    public static Result<T> GatewayTimeout<T>() => new(default, ResultStatus.GatewayTimeout, null, [], [], null);
    public static Result<T> GatewayTimeout<T>(string errorMessage) => new(default, ResultStatus.GatewayTimeout, errorMessage, [], [], null);
    public static Result BadGateway() => new(ResultStatus.BadGateway, null, [], [], null);
    public static Result BadGateway(string errorMessage) => new(ResultStatus.BadGateway, errorMessage, [], [], null);
    public static Result<T> BadGateway<T>() => new(default, ResultStatus.BadGateway, null, [], [], null);
    public static Result<T> BadGateway<T>(string errorMessage) => new(default, ResultStatus.BadGateway, errorMessage, [], [], null);
    public static Result InsuficientStorage() => new(ResultStatus.InsuficientStorage, null, [], [], null);
    public static Result InsuficientStorage(string errorMessage) => new(ResultStatus.InsuficientStorage, errorMessage, [], [], null);
    public static Result<T> InsuficientStorage<T>() => new(default, ResultStatus.InsuficientStorage, null, [], [], null);
    public static Result<T> InsuficientStorage<T>(string errorMessage) => new(default, ResultStatus.InsuficientStorage, errorMessage, [], [], null);
    public static Result UnprocessableContent() => new(ResultStatus.UnprocessableContent, null, [], [], null);
    public static Result UnprocessableContent(string errorMessage) => new(ResultStatus.UnprocessableContent, errorMessage, [], [], null);
    public static Result<T> UnprocessableContent<T>() => new(default, ResultStatus.UnprocessableContent, null, [], [], null);
    public static Result<T> UnprocessableContent<T>(string errorMessage) => new(default, ResultStatus.UnprocessableContent, errorMessage, [], [], null);
    public static Result FailedDependency() => new(ResultStatus.FailedDependency, null, [], [], null);
    public static Result FailedDependency(string errorMessage) => new(ResultStatus.FailedDependency, errorMessage, [], [], null);
    public static Result<T> FailedDependency<T>() => new(default, ResultStatus.FailedDependency, null, [], [], null);
    public static Result<T> FailedDependency<T>(string errorMessage) => new(default, ResultStatus.FailedDependency, errorMessage, [], [], null);
    public static Result PreconditionRequired() => new(ResultStatus.PreconditionRequired, null, [], [], null);
    public static Result PreconditionRequired(string errorMessage) => new(ResultStatus.PreconditionRequired, errorMessage, [], [], null);
    public static Result<T> PreconditionRequired<T>() => new(default, ResultStatus.PreconditionRequired, null, [], [], null);
    public static Result<T> PreconditionRequired<T>(string errorMessage) => new(default, ResultStatus.PreconditionRequired, errorMessage, [], [], null);
    public static Result PreconditionFailed() => new(ResultStatus.PreconditionFailed, null, [], [], null);
    public static Result PreconditionFailed(string errorMessage) => new(ResultStatus.PreconditionFailed, errorMessage, [], [], null);
    public static Result<T> PreconditionFailed<T>() => new(default, ResultStatus.PreconditionFailed, null, [], [], null);
    public static Result<T> PreconditionFailed<T>(string errorMessage) => new(default, ResultStatus.PreconditionFailed, errorMessage, [], [], null);
    public static Result TooManyRequests() => new(ResultStatus.TooManyRequests, null, [], [], null);
    public static Result TooManyRequests(string errorMessage) => new(ResultStatus.TooManyRequests, errorMessage, [], [], null);
    public static Result<T> TooManyRequests<T>() => new(default, ResultStatus.TooManyRequests, null, [], [], null);
    public static Result<T> TooManyRequests<T>(string errorMessage) => new(default, ResultStatus.TooManyRequests, errorMessage, [], [], null);

    public static Result<T> Redirect<T>(T value) => new(value, ResultStatus.Redirect, null, [], [], null);

    public static Result<T> FromExistingResult<T>(Result existingResult)
    {
        ArgumentGuard.IsNotNull(existingResult, nameof(existingResult));

        return new(TryGetValue<T>(existingResult), existingResult.Status, existingResult.ErrorMessage, existingResult.ValidationErrors, existingResult.InnerResults, null);
    }

    public static Result<T> FromExistingResult<T>(Result existingResult, T value)
    {
        ArgumentGuard.IsNotNull(existingResult, nameof(existingResult));

        return new(value, existingResult.Status, existingResult.ErrorMessage, existingResult.ValidationErrors, existingResult.InnerResults, null);
    }

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

    public Result Either(Func<Result, Result> errorDelegate)
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        if (!IsSuccessful())
        {
            return errorDelegate(this);
        }

        return this;
    }
}
