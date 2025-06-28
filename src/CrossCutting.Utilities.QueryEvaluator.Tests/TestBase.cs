namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public abstract class TestBase
{
    protected IDictionary<Type, object?> Mocks { get; }
    protected IExpressionEvaluator Evaluator { get; }
    protected IDateTimeProvider DateTimeProvider => Mocks.GetOrCreate<IDateTimeProvider>(ClassFactory);
    protected DateTime CurrentDateTime { get; }

    protected TestBase()
    {
        Mocks = new Dictionary<Type, object?>();
        Evaluator = Testing.CreateInstance<IExpressionEvaluator>(ClassFactory, Mocks);

        // Freeze DateTime.Now to a predicatable value
        CurrentDateTime = new DateTime(2025, 2, 1, 5, 30, 0, DateTimeKind.Utc);
        DateTimeProvider
            .GetCurrentDateTime()
            .Returns(CurrentDateTime);
    }

    protected ExpressionEvaluatorContext CreateContext(
        string? expression,
        object? state,
        int currentRecursionLevel = 1,
        ExpressionEvaluatorContext? parentContext = null,
        IExpressionEvaluator? evaluator = null,
        ExpressionEvaluatorSettingsBuilder? settings = null)
    {
        IReadOnlyDictionary<string, Task<Result<object?>>>? dict = null;
        if (state is not null)
        {
            dict = new AsyncResultDictionaryBuilder<object?>()
                .Add(Constants.State, state)
                .BuildDeferred();
        }

        return CreateContext(expression, dict, currentRecursionLevel, parentContext, evaluator, settings);
    }

    protected ExpressionEvaluatorContext CreateContext(
        string? expression,
        IReadOnlyDictionary<string, Task<Result<object?>>>? state = null,
        int currentRecursionLevel = 1,
        ExpressionEvaluatorContext? parentContext = null,
        IExpressionEvaluator? evaluator = null,
        ExpressionEvaluatorSettingsBuilder? settings = null)
            => new ExpressionEvaluatorContext(expression, settings ?? new ExpressionEvaluatorSettingsBuilder(), evaluator ?? Evaluator, state, currentRecursionLevel, parentContext);

    // Class factory for NSubstitute, see Readme.md
    protected object? ClassFactory(Type t)
        => t.CreateInstance(parameterType => Substitute.For([parameterType], []), Mocks, null, null);

}
