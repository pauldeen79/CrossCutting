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
        => t.CreateInstance(parameterType => parameterType.IsInterface
            ? Substitute.For([parameterType], [])
            : parameterType, ClassFactories, null, null);

    protected void InitializeMock<T>(T[] items)
    {
        _dataProviderMock.ResultDelegate = new Func<Query, Task<Result<IEnumerable>>>
        (
            async query =>
            {
                var results = new List<T>();
                foreach (var item in items)
                {
                    var result = await IsItemValid(query, item!).ConfigureAwait(false);

                    if (!result.IsSuccessful())
                    {
                        return Result.FromExistingResult<IEnumerable>(result);
                    }

                    if (result.Value)
                    {
                        results.Add(item);
                    }
                }

                return Result.Success<IEnumerable>(results);
            }
        );
    }
    protected async Task<Result<bool>> IsItemValid(Query query, object item)
    {
        var context = CreateContext("Dummy", item);
        if (CanEvaluateSimpleConditions(query.Filter))
        {
            return await EvaluateSimpleConditions(query.Filter, context, CancellationToken.None).ConfigureAwait(false);
        }

        return await EvaluateComplexConditions(query.Filter, context, CancellationToken.None).ConfigureAwait(false);
    }

    private static bool CanEvaluateSimpleConditions(IEnumerable<Condition> conditions)
        => !conditions.Any(x =>
            (x.Combination ?? Combination.And) == Combination.Or
            || x.StartGroup
            || x.EndGroup
        );

    private static async Task<Result<bool>> EvaluateSimpleConditions(IEnumerable<Condition> conditions, ExpressionEvaluatorContext context, CancellationToken token)
    {
        foreach (var condition in conditions)
        {
            var itemResult = await condition.EvaluateTypedAsync(context, token).ConfigureAwait(false);
            if (!itemResult.IsSuccessful())
            {
                return itemResult;
            }

            if (!itemResult.Value)
            {
                return itemResult;
            }
        }

        return Result.Success(true);
    }

    private static async Task<Result<bool>> EvaluateComplexConditions(IEnumerable<Condition> conditions, ExpressionEvaluatorContext context, CancellationToken token)
    {
        var builder = new StringBuilder();
        foreach (var condition in conditions)
        {
            if (builder.Length > 0)
            {
                builder.Append(condition.Combination == Combination.Or
                    ? "|"
                    : "&");
            }

            var prefix = condition.StartGroup ? "(" : string.Empty;
            var suffix = condition.EndGroup ? ")" : string.Empty;
            var itemResult = await condition.EvaluateTypedAsync(context, token).ConfigureAwait(false);
            if (!itemResult.IsSuccessful())
            {
                return itemResult;
            }
            builder.Append(prefix)
                   .Append(itemResult.Value ? "T" : "F")
                   .Append(suffix);
        }

        return Result.Success(EvaluateBooleanExpression(builder.ToString()));
    }

    private static bool EvaluateBooleanExpression(string expression)
    {
        var result = ProcessRecursive(ref expression);

        var @operator = "&";
        foreach (var character in expression)
        {
            bool currentResult;
            switch (character)
            {
                case '&':
                    @operator = "&";
                    break;
                case '|':
                    @operator = "|";
                    break;
                case 'T':
                case 'F':
                    currentResult = character == 'T';
                    result = @operator == "&"
                        ? result && currentResult
                        : result || currentResult;
                    break;
            }
        }

        return result;
    }

    private static bool ProcessRecursive(ref string expression)
    {
        var result = true;
        var openIndex = -1;
        int closeIndex;
        do
        {
            closeIndex = expression.IndexOf(')', StringComparison.Ordinal);
            if (closeIndex > -1)
            {
                openIndex = expression.LastIndexOf('(', closeIndex);
                if (openIndex > -1)
                {
                    result = EvaluateBooleanExpression(expression.Substring(openIndex + 1, closeIndex - openIndex - 1));
                    expression = string.Concat(GetPrefix(expression, openIndex),
                                               GetCurrent(result),
                                               GetSuffix(expression, closeIndex));
                }
            }
        } while (closeIndex > -1 && openIndex > -1);
        return result;
    }

    private static string GetPrefix(string expression, int openIndex)
        => openIndex == 0
            ? string.Empty
            : expression.Substring(0, openIndex - 2);

    private static string GetCurrent(bool result)
        => result
            ? "T"
            : "F";

    private static string GetSuffix(string expression, int closeIndex)
        => closeIndex == expression.Length
            ? string.Empty
            : expression.Substring(closeIndex + 1);
}
