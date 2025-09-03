namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public abstract class TestBase
{
    private readonly DataProviderMock _dataProviderMock;
    private IEnumerable<object> _sourceData = Enumerable.Empty<object>();

    protected IDictionary<Type, object?> ClassFactoryDictionary { get; }
    protected IQueryProcessor InMemoryQueryProcessor => ClassFactoryDictionary.GetOrCreateMultiple<IQueryProcessor>(ClassFactory).OfType<QueryEvaluator.QueryProcessors.InMemory.QueryProcessor>().First();
    protected IQueryProcessor SqlQueryProcessor => ClassFactoryDictionary.GetOrCreateMultiple<IQueryProcessor>(ClassFactory).OfType<QueryEvaluator.QueryProcessors.Sql.QueryProcessor>().First();
    protected IDatabaseEntityRetrieverProvider DatabaseEntityRetrieverProvider => ClassFactoryDictionary.GetOrCreate<IDatabaseEntityRetrieverProvider>(ClassFactory);
    protected IDatabaseEntityRetriever<MyEntity> DatabaseEntityRetriever => ClassFactoryDictionary.GetOrCreate<IDatabaseEntityRetriever<MyEntity>>(ClassFactory);
    protected IPagedResult<MyEntity> PagedResult => ClassFactoryDictionary.GetOrCreate<IPagedResult<MyEntity>>(ClassFactory);
    protected IPagedDatabaseCommandProvider<IQuery> PagedDatabaseCommandProvider => ClassFactoryDictionary.GetOrCreate<IPagedDatabaseCommandProvider<IQuery>>(ClassFactory);
    protected IExpressionEvaluator Evaluator => ClassFactoryDictionary.GetOrCreate<IExpressionEvaluator>(ClassFactory);
    protected IDateTimeProvider DateTimeProvider => ClassFactoryDictionary.GetOrCreate<IDateTimeProvider>(ClassFactory);
    protected IPagedDatabaseEntityRetrieverSettingsProvider DatabaseEntityRetrieverSettingsProvider => ClassFactoryDictionary.GetOrCreate<IPagedDatabaseEntityRetrieverSettingsProvider>(ClassFactory);
    protected IPagedDatabaseEntityRetrieverSettings DatabaseEntityRetrieverSettings => ClassFactoryDictionary.GetOrCreate<IPagedDatabaseEntityRetrieverSettings>(ClassFactory);
    protected IQueryFieldInfoProviderHandler QueryFieldInfoProviderHandler => ClassFactoryDictionary.GetOrCreate<IQueryFieldInfoProviderHandler>(ClassFactory);
    protected IQueryFieldInfo QueryFieldInfo => ClassFactoryDictionary.GetOrCreate<IQueryFieldInfo>(ClassFactory);
    protected DateTime CurrentDateTime { get; }

    protected TestBase()
    {
        var excludedTypes = new Type[] { typeof(IDateTimeProvider) };
        ClassFactoryDictionary = new ServiceCollection()
            .AddExpressionEvaluator()
            .AddQueryEvaluatorInMemory()
            .AddQueryEvaluatorSql()
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

        // Initialize the expression evaluator on the DataProvider
        // This needs to be done after initializing class factories, because we need the expression evaluator
        _dataProviderMock = new DataProviderMock(Evaluator, () => _sourceData);
        ClassFactoryDictionary.Add(typeof(IDataProvider), _dataProviderMock);

        // Initialize entity retriever
        DatabaseEntityRetrieverProvider
            .Create<MyEntity>(Arg.Any<IQuery>())
            .Returns(Result.Success(DatabaseEntityRetriever));

        // Initialize entity retriever settings provider
#pragma warning disable CS8601 // Possible null reference assignment.
        DatabaseEntityRetrieverSettingsProvider
            .TryGet<IQuery>(out Arg.Any<IPagedDatabaseEntityRetrieverSettings>())
            .Returns(x =>
            {
                x[0] = DatabaseEntityRetrieverSettings;
                return true;
            });
#pragma warning restore CS8601 // Possible null reference assignment.
        DatabaseEntityRetrieverSettings
            .TableName
            .Returns("MyEntity");

        // Initialize query field info provider
        QueryFieldInfoProviderHandler
            .Create(Arg.Any<IQuery>())
            .Returns(_ => Result.Success(QueryFieldInfo));
        QueryFieldInfo
            .GetDatabaseFieldName(Arg.Any<string>())
            .Returns(x => x.ArgAt<string>(0));
    }

    protected static MyEntity[] CreateData() =>
    [
        new MyEntity("B", "C"),
        new MyEntity("A", "Z"),
        new MyEntity("B", "D"),
        new MyEntity("A", "A"),
        new MyEntity("B", "E"),
    ];

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
            : parameterType, ClassFactoryDictionary, null, null);

    protected void InitializeMock<T>(IEnumerable<T> items)
    {
        _sourceData = items.Cast<object>().ToArray();
        DatabaseEntityRetriever.FindOneAsync(Arg.Any<IDatabaseCommand>(), Arg.Any<CancellationToken>()).Returns(_ => Task.FromResult(_sourceData.OfType<MyEntity>().FirstOrDefault()));
        DatabaseEntityRetriever.FindManyAsync(Arg.Any<IDatabaseCommand>(), Arg.Any<CancellationToken>()).Returns(_ => Task.FromResult<IReadOnlyCollection<MyEntity>>(_sourceData.OfType<MyEntity>().ToList()));
        DatabaseEntityRetriever.FindPagedAsync(Arg.Any<IPagedDatabaseCommand>(), Arg.Any<CancellationToken>()).Returns(_ => Task.FromResult(PagedResult));
        PagedResult.Count.Returns(_sourceData.OfType<T>().Count());
        PagedResult.TotalRecordCount.Returns(_sourceData.OfType<T>().Count());
        PagedResult.PageSize.Returns(_sourceData.OfType<T>().Count());
        PagedResult.GetEnumerator().Returns(_sourceData.OfType<MyEntity>().GetEnumerator());
    }
}

public abstract class TestBase<T> : TestBase
{
    protected StringComparison StringComparison { get; set; }

    protected T CreateSut(IDictionary<string, object?>? parameters = null)
        => Testing.CreateInstance<T>(ClassFactory, ClassFactoryDictionary, p =>
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
