namespace CrossCutting.Utilities.QueryEvaluator.Abstractions.Builders.Extensions;

public static partial class QueryBuilderExtensions
{
    public static T Where<T>(this T instance, params IConditionBuilder[] additionalConditions)
        where T : IQueryBuilder
        => instance.With(x => x.AddConditions(additionalConditions));

    public static T Where<T>(this T instance, IEnumerable<IConditionBuilder> additionalConditions)
        where T : IQueryBuilder
        => instance.Where(additionalConditions.ToArray());

    public static T Or<T>(this T instance, params IConditionBuilder[] additionalConditions)
        where T : IQueryBuilder
        => instance.Where(additionalConditions.Select(a => a.WithCombination(combination: Combination.Or)));

    public static T Or<T>(this T instance, IEnumerable<IConditionBuilder> additionalConditions)
        where T : IQueryBuilder
        => instance.Or(additionalConditions.ToArray());

    public static T And<T>(this T instance, params IConditionBuilder[] additionalConditions)
        where T : IQueryBuilder
        => instance.Where(additionalConditions.Select(a => a.WithCombination(combination: Combination.And)));

    public static T And<T>(this T instance, IEnumerable<IConditionBuilder> additionalConditions)
        where T : IQueryBuilder
        => instance.And(additionalConditions.ToArray());

    public static T AndAny<T>(this T instance, params IConditionBuilder[] additionalConditions)
        where T : IQueryBuilder
        => instance.Where(additionalConditions.Select((a, index) => a.WithStartGroup(index == 0)
                                                                     .WithEndGroup(index + 1 == additionalConditions.Length)
                                                                     .WithCombination(index == 0 ? Combination.And : Combination.Or)));

    public static T OrAll<T>(this T instance, params IConditionBuilder[] additionalConditions)
        where T : IQueryBuilder
        => instance.Where(additionalConditions.Select((a, index) => a.WithStartGroup(index == 0)
                                                                     .WithEndGroup(index + 1 == additionalConditions.Length)
                                                                     .WithCombination(index == 0 ? Combination.Or : Combination.And)));

    public static T OrderBy<T>(this T instance, params ISortOrderBuilder[] additionalOrderByFields)
        where T : IQueryBuilder
        => instance.With(x => x.SortOrders.AddRange(additionalOrderByFields));

    public static T OrderBy<T>(this T instance, IEnumerable<ISortOrderBuilder> additionalOrderByFields)
        where T : IQueryBuilder
        => instance.OrderBy(additionalOrderByFields.ToArray());

    public static T OrderBy<T>(this T instance, params string[] additionalSortOrders)
        where T : IQueryBuilder
        => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder().WithPropertyName(s)));

    public static T ThenBy<T>(this T instance, params ISortOrderBuilder[] additionalSortOrders)
        where T : IQueryBuilder
        => instance.OrderBy(additionalSortOrders);

    public static T ThenBy<T>(this T instance, params string[] additionalSortOrders)
        where T : IQueryBuilder
        => instance.OrderBy(additionalSortOrders);

    public static T Offset<T>(this T instance, int? offset)
        where T : IQueryBuilder
        => instance.With(x => x.Offset = offset);

    public static T Limit<T>(this T instance, int? limit)
        where T : IQueryBuilder
        => instance.With(x => x.Limit = limit);

    public static T Skip<T>(this T instance, int? offset)
        where T : IQueryBuilder
        => instance.Offset(offset);

    public static T Take<T>(this T instance, int? limit)
        where T : IQueryBuilder
        => instance.Limit(limit);

    public static T OrderByDescending<T>(this T instance, params ISortOrderBuilder[] additionalSortOrders)
        where T : IQueryBuilder
        => instance.OrderBy(additionalSortOrders.Select(so => new QuerySortOrderBuilder().WithExpression(so.Expression).WithOrder(SortOrderDirection.Descending)));

    public static T OrderByDescending<T>(this T instance, params string[] additionalSortOrders)
        where T : IQueryBuilder
        => instance.OrderBy(additionalSortOrders.Select(s => new QuerySortOrderBuilder().WithPropertyName(s).WithOrder(SortOrderDirection.Descending)));

    public static T ThenByDescending<T>(this T instance, params ISortOrderBuilder[] additionalSortOrders)
        where T : IQueryBuilder
        => instance.OrderByDescending(additionalSortOrders);

    public static T ThenByDescending<T>(this T instance, params string[] additionalSortOrders)
        where T : IQueryBuilder
        => instance.OrderByDescending(additionalSortOrders);

    private sealed class QuerySortOrderBuilder : ISortOrderBuilder
    {
        [Required, ValidateObject] public IEvaluatableBuilder Expression { get; set; } = default!;
        public SortOrderDirection Order { get; set; }

        public ISortOrder Build() => new SortOrder(Expression?.Build(), Order);
    }

    private sealed class SortOrder : ISortOrder
    {
        public SortOrder(IEvaluatable? expression, SortOrderDirection order)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            Order = order;
        }

        public IEvaluatable Expression { get; }
        public SortOrderDirection Order { get; }

        public ISortOrderBuilder ToBuilder()
            => new QuerySortOrderBuilder
            {
                Expression = Expression.ToBuilder(),
                Order = Order
            };
    }
}