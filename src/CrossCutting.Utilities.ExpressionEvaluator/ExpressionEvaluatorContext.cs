namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ExpressionEvaluatorContext
{
    public string Expression { get; }
    public ExpressionEvaluatorSettings Settings { get; }
    public object? Context { get; }
    public int CurrentRecursionLevel { get; }
    public ExpressionEvaluatorContext? ParentContext { get; }

    internal IExpressionEvaluator Evaluator { get; }
    private bool UseCallback { get; set; }

    public ExpressionEvaluatorContext(
        string? expression,
        ExpressionEvaluatorSettings settings,
        object? context,
        IExpressionEvaluator evaluator,
        int currentRecursionLevel = 1,
        ExpressionEvaluatorContext? parentContext = null)
    {
        ArgumentGuard.IsNotNull(settings, nameof(settings));
        ArgumentGuard.IsNotNull(evaluator, nameof(evaluator));

        Expression = expression?.Trim() ?? string.Empty;
        Settings = settings;
        Context = context;
        Evaluator = evaluator;
        CurrentRecursionLevel = currentRecursionLevel;
        ParentContext = parentContext;
    }

    public Result<object?> Evaluate(string expression)
        => UseCallback
            ? Evaluator.EvaluateCallback(CreateChildContext(expression))
            : Evaluator.Evaluate(CreateChildContext(expression));

    public Result<T> EvaluateTyped<T>(string expression)
        => UseCallback
            ? Evaluator.EvaluateTypedCallback<T>(CreateChildContext(expression))
            : Evaluator.EvaluateTyped<T>(CreateChildContext(expression));

    public ExpressionParseResult Parse(string expression)
        => UseCallback
            ? Evaluator.ParseCallback(CreateChildContext(expression))
            : Evaluator.Parse(CreateChildContext(expression));

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
        => new ExpressionEvaluatorContext(expression, Settings, Context, Evaluator, CurrentRecursionLevel + 1, this);

    internal ExpressionEvaluatorContext FromRoot()
    {
        UseCallback = true;
        return this;
    }
}
