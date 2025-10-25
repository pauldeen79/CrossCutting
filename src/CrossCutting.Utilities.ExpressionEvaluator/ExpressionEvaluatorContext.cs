namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ExpressionEvaluatorContext
{
    public string Expression { get; }
    public ExpressionEvaluatorSettings Settings { get; }
    public IReadOnlyDictionary<string, Func<Task<Result<object?>>>> State { get; }
    public int CurrentRecursionLevel { get; }
    public ExpressionEvaluatorContext? ParentContext { get; }

    internal IExpressionEvaluator Evaluator { get; }
    private bool UseCallback { get; set; }

    public ExpressionEvaluatorContext(
        ExpressionEvaluatorSettings settings,
        IExpressionEvaluator evaluator,
        object? context)
    {
        ArgumentGuard.IsNotNull(settings, nameof(settings));
        ArgumentGuard.IsNotNull(evaluator, nameof(evaluator));

        var state = new AsyncResultDictionaryBuilder<object?>()
            .Add(Constants.Context, context)
            .BuildDeferred();

        Expression = string.Empty;
        Settings = settings;
        State = state;
        Evaluator = evaluator;
    }

    public ExpressionEvaluatorContext(
        string? expression,
        ExpressionEvaluatorSettings settings,
        IExpressionEvaluator evaluator,
        IReadOnlyDictionary<string, Func<Task<Result<object?>>>>? state = null,
        int currentRecursionLevel = 1,
        ExpressionEvaluatorContext? parentContext = null)
    {
        ArgumentGuard.IsNotNull(settings, nameof(settings));
        ArgumentGuard.IsNotNull(evaluator, nameof(evaluator));

        Expression = expression?.Trim() ?? string.Empty;
        Settings = settings;
        State = state ?? new Dictionary<string, Func<Task<Result<object?>>>>();
        Evaluator = evaluator;
        CurrentRecursionLevel = currentRecursionLevel;
        ParentContext = parentContext;
    }

    public Task<Result<object?>> EvaluateAsync(string expression, CancellationToken token)
        => UseCallback
            ? Evaluator.EvaluateCallbackAsync(CreateChildContext(expression), token)
            : Evaluator.EvaluateAsync(CreateChildContext(expression), token);

    public async Task<Result<T>> EvaluateTypedAsync<T>(string expression, CancellationToken token)
        => UseCallback
            ? await Evaluator.EvaluateTypedCallbackAsync<T>(CreateChildContext(expression), token).ConfigureAwait(false)
            : await Evaluator.EvaluateTypedAsync<T>(CreateChildContext(expression), token).ConfigureAwait(false);

    public async Task<ExpressionParseResult> ParseAsync(string expression, CancellationToken token)
        => UseCallback
            ? await Evaluator.ParseCallbackAsync(CreateChildContext(expression), token).ConfigureAwait(false)
            : await Evaluator.ParseAsync(CreateChildContext(expression), token).ConfigureAwait(false);

    public Result<T> Validate<T>()
    {
        if (string.IsNullOrEmpty(Expression))
        {
            return Result.Invalid<T>("Value is required");
        }

        if (CurrentRecursionLevel > Settings.MaximumRecursion)
        {
            return Result.Invalid<T>("Maximum recursion level has been reached");
        }

        return Result.NoContent<T>();
    }

    internal ExpressionEvaluatorContext CreateChildContext(string expression)
        => new ExpressionEvaluatorContext(expression, Settings, Evaluator, State, CurrentRecursionLevel + 1, this);

    internal ExpressionEvaluatorContext FromRoot()
    {
        UseCallback = true;
        return this;
    }
}
