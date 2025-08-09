﻿namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.InMemory;

public class DefaultPaginator : IPaginator
{
    private readonly IExpressionEvaluator _expressionEvaluator;

    public DefaultPaginator(IExpressionEvaluator expressionEvaluator)
    {
        ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        _expressionEvaluator = expressionEvaluator;
    }

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

    private async Task<IEnumerable<T>> GetOrderedItems<T>(IQuery query, IEnumerable<T> result, CancellationToken token) where T : class
    {
        var orderByResults = await Task.WhenAll(result.Select(async x =>
        {
            var list = new List<object?>();
            var state = new AsyncResultDictionaryBuilder<object?>()
                .Add(Constants.Context, x)
                .BuildDeferred();
            var context = new ExpressionEvaluatorContext("Dummy", new ExpressionEvaluatorSettingsBuilder(), _expressionEvaluator, state);

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
}
