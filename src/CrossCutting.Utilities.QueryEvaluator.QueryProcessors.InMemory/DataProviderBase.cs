namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory;

public abstract class DataProviderBase : IDataProvider
{
    private readonly IExpressionEvaluator _expressionEvaluator;

    protected DataProviderBase(IExpressionEvaluator expressionEvaluator)
    {
        ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        _expressionEvaluator = expressionEvaluator;
    }

    public abstract IEnumerable<object> GetSourceData();

    public async Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(IQuery query)
        where TResult : class
    {
        var results = new List<TResult>();
        var sourceData = GetSourceData().OfType<TResult>().ToArray();
        if (sourceData.Length == 0)
        {
            return Result.Continue<IEnumerable<TResult>>();
        }

        foreach (var item in sourceData)
        {
            var result = await IsItemValid(query, CreateContext(item)!).ConfigureAwait(false);

            if (!result.IsSuccessful())
            {
                return Result.FromExistingResult<IEnumerable<TResult>>(result);
            }

            if (result.Value)
            {
                results.Add(item);
            }
        }

        return Result.Success<IEnumerable<TResult>>(results);
    }

    private ExpressionEvaluatorContext CreateContext(object? state)
    {
        IReadOnlyDictionary<string, Task<Result<object?>>>? dict = null;
        if (state is not null)
        {
            dict = new AsyncResultDictionaryBuilder<object?>()
                .Add(Constants.Context, state)
                .BuildDeferred();
        }

        return new ExpressionEvaluatorContext("Dummy", new ExpressionEvaluatorSettingsBuilder(), _expressionEvaluator, dict);
    }

    private static async Task<Result<bool>> IsItemValid(IQuery query, ExpressionEvaluatorContext context)
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        if (CanEvaluateSimpleConditions(query.Conditions))
        {
            return await EvaluateSimpleConditions(query.Conditions, context, CancellationToken.None).ConfigureAwait(false);
        }

        return await EvaluateComplexConditions(query.Conditions, context, CancellationToken.None).ConfigureAwait(false);
    }

    private static bool CanEvaluateSimpleConditions(IEnumerable<ICondition> conditions)
        => !conditions.Any(x =>
            (x.Combination ?? Combination.And) == Combination.Or
            || x.StartGroup
            || x.EndGroup
        );

    private static async Task<Result<bool>> EvaluateSimpleConditions(IEnumerable<ICondition> conditions, ExpressionEvaluatorContext context, CancellationToken token)
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

    private static async Task<Result<bool>> EvaluateComplexConditions(IEnumerable<ICondition> conditions, ExpressionEvaluatorContext context, CancellationToken token)
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
            closeIndex = expression.IndexOf(')');
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
