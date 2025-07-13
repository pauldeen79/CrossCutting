namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory;

public class DefaultPaginator : IPaginator
{
    public async Task<IEnumerable<T>> GetPagedDataAsync<T>(IQuery query, IEnumerable<T> filteredRecords, CancellationToken token)
        where T : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        IEnumerable<T> result = filteredRecords;

        if (query.SortOrders.Count > 0)
        {
            result = await GetOrderedItems(query, result, token).ConfigureAwait(false);
        }

        if (query.Offset is not null)
        {
            result = result.Skip(query.Offset.Value);
        }

        if (query.Limit is not null)
        {
            result = result.Take(query.Limit.Value);
        }

        return result;
    }

    private static async Task<IEnumerable<T>> GetOrderedItems<T>(IQuery query, IEnumerable<T> result, CancellationToken token) where T : class
    {
        var orderByResults = await Task.WhenAll(result.Select(async x =>
        {
            var list = new List<object?>();
            var state = new AsyncResultDictionaryBuilder<object?>()
                .Add(Constants.Context, x)
                .BuildDeferred();
            var context = new ExpressionEvaluatorContext("Dummy", new ExpressionEvaluatorSettingsBuilder(), new NoopEvaluator(), state);

            foreach (var orderByField in query.SortOrders)
            {
                list.Add((await orderByField.Expression.EvaluateAsync(context, token).ConfigureAwait(false)).Value);
            }
            return list;
        })).ConfigureAwait(false);

        var zippedItems = result.Zip(orderByResults, (item, itemOrders) => new { Item = item, ItemOrders = itemOrders });
        var orderedItems = query.SortOrders.First().Order == SortOrderDirection.Ascending
            ? zippedItems.OrderBy(x => x.ItemOrders[0])
            : zippedItems.OrderByDescending(x => x.ItemOrders[0]);

        var index = 0;
        foreach (var subsequentOrder in query.SortOrders.Skip(1))
        {
            index++;
            orderedItems = subsequentOrder.Order == SortOrderDirection.Ascending
                ? orderedItems.ThenBy(x => x.ItemOrders[index])
                : orderedItems.ThenByDescending(x => x.ItemOrders[index]);
        }

        return orderedItems.Select(x => x.Item);
    }

    private sealed class NoopEvaluator : IExpressionEvaluator
    {
        private const string ErrorMessage = "It's not supported to call an expression evaluator directly from queries";

        public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
            => Task.FromResult(Result.NotSupported<object?>(ErrorMessage));

        public Task<Result<object?>> EvaluateCallbackAsync(ExpressionEvaluatorContext context, CancellationToken token)
            => Task.FromResult(Result.NotSupported<object?>(ErrorMessage));

        public Task<Result<T>> EvaluateTypedAsync<T>(ExpressionEvaluatorContext context, CancellationToken token)
            => Task.FromResult(Result.NotSupported<T>(ErrorMessage));

        public Task<Result<T>> EvaluateTypedCallbackAsync<T>(ExpressionEvaluatorContext context, CancellationToken token)
            => Task.FromResult(Result.NotSupported<T>(ErrorMessage));

        public Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
            => Task.FromResult(new ExpressionParseResultBuilder().WithStatus(ResultStatus.NotSupported).WithErrorMessage(ErrorMessage).Build());

        public Task<ExpressionParseResult> ParseCallbackAsync(ExpressionEvaluatorContext context, CancellationToken token)
            => Task.FromResult(new ExpressionParseResultBuilder().WithStatus(ResultStatus.NotSupported).WithErrorMessage(ErrorMessage).Build());
    }
}
