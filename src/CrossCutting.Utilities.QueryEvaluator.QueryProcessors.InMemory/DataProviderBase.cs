namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory;

public abstract class DataProviderBase : IDataProvider
{
    public abstract Task<Result<IEnumerable<TResult>>> GetDataAsync<TResult>(Query query)
        where TResult : class;

    protected static async Task<Result<bool>> IsItemValid(Query query, ExpressionEvaluatorContext context)
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

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
