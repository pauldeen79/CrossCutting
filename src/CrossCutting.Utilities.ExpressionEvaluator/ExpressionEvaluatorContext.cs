namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ExpressionEvaluatorContext
{
    public string Expression { get; }
    public ExpressionEvaluatorSettings Settings { get; }
    public IReadOnlyDictionary<string, Task<Result<object?>>> State { get; }
    public int CurrentRecursionLevel { get; }
    public ExpressionEvaluatorContext? ParentContext { get; }

    internal IExpressionEvaluator Evaluator { get; }
    private bool UseCallback { get; set; }

    public ExpressionEvaluatorContext(
        string? expression,
        ExpressionEvaluatorSettings settings,
        IExpressionEvaluator evaluator,
        IReadOnlyDictionary<string, Task<Result<object?>>>? state = null,
        int currentRecursionLevel = 1,
        ExpressionEvaluatorContext? parentContext = null)
    {
        ArgumentGuard.IsNotNull(settings, nameof(settings));
        ArgumentGuard.IsNotNull(evaluator, nameof(evaluator));

        Expression = expression?.Trim() ?? string.Empty;
        Settings = settings;
        State = state ?? new Dictionary<string, Task<Result<object?>>>();
        Evaluator = evaluator;
        CurrentRecursionLevel = currentRecursionLevel;
        ParentContext = parentContext;
    }

    public Task<Result<object?>> EvaluateAsync(string expression)
        => UseCallback
            ? Evaluator.EvaluateCallbackAsync(CreateChildContext(expression))
            : Evaluator.EvaluateAsync(CreateChildContext(expression));

    public async Task<Result<T>> EvaluateTypedAsync<T>(string expression)
        => UseCallback
            ? (await Evaluator.EvaluateCallbackAsync(CreateChildContext(expression)).ConfigureAwait(false)).TryCastAllowNull<T>()
            : (await Evaluator.EvaluateAsync(CreateChildContext(expression)).ConfigureAwait(false)).TryCastAllowNull<T>();

    public async Task<ExpressionParseResult> ParseAsync(string expression)
        => UseCallback
            ? await Evaluator.ParseCallbackAsync(CreateChildContext(expression)).ConfigureAwait(false)
            : await Evaluator.ParseAsync(CreateChildContext(expression)).ConfigureAwait(false);

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
