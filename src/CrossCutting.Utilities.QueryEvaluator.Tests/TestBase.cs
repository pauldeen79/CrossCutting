namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public abstract class TestBase
{
    private readonly DataProviderMock _dataProviderMock;

    protected IDictionary<Type, object?> ClassFactories { get; }
    protected IQueryProcessor QueryProcessor => ClassFactories.GetOrCreate<IQueryProcessor>(ClassFactory);
    protected IExpressionEvaluator Evaluator => ClassFactories.GetOrCreate<IExpressionEvaluator>(ClassFactory);
    protected IDateTimeProvider DateTimeProvider => ClassFactories.GetOrCreate<IDateTimeProvider>(ClassFactory);
    protected IDataProvider DataProvider => ClassFactories.GetOrCreate<IDataProvider>(ClassFactory);
    protected DateTime CurrentDateTime { get; }

    protected TestBase()
    {
        _dataProviderMock = new DataProviderMock();
        var excludedTypes = new Type[] { typeof(IDateTimeProvider) };
        ClassFactories = new ServiceCollection()
            .AddExpressionEvaluator()
            .AddQueryEvaluatorInMemory()
            .AddSingleton<IDataProvider>(_dataProviderMock)
            .Where(sd => !excludedTypes.Contains(sd.ServiceType))
            .GroupBy(sd => sd.ServiceType)
            .ToDictionary(
                g => g.Key,
                g => g.Count() == 1
                    ? g.First().ImplementationInstance ?? g.First().ImplementationType
                    : g.Select(t => t.ImplementationInstance ?? t.ImplementationType).ToArray());

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
                .Add(Constants.Context, state)
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
        => t.CreateInstance(parameterType => parameterType.IsInterface
            ? Substitute.For([parameterType], [])
            : parameterType, ClassFactories, null, null);

    protected void InitializeMock<T>(T[] items)
    {
        _dataProviderMock.SourceData = items.Cast<object>().ToArray();
        _dataProviderMock.CreateContextDelegate = item => CreateContext("Dummy", item);
    }
}

public abstract class TestBase<T> : TestBase
{
    protected StringComparison StringComparison { get; set; }

    protected T CreateSut(IDictionary<string, object?>? parameters = null) => Testing.CreateInstance<T>(ClassFactory, ClassFactories, p =>
    {
        if (p.ParameterType == typeof(StringComparison))
        {
            return StringComparison;
        }

        if (parameters is not null
            && p.Name is not null
            && parameters.TryGetValue(p.Name, out var value))
        {
            return value;
        }

        return null;
    });
}
