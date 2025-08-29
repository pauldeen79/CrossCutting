namespace CrossCutting.Utilities.QueryEvaluator.Abstractions.Extensions;

public static class QueryExtensions
{
    public static string GetTableName(this IQuery instance, string tableName)
        => instance is IDataObjectNameQuery dataObjectNameQuery && !string.IsNullOrEmpty(dataObjectNameQuery.DataObjectName)
            ? dataObjectNameQuery.DataObjectName
            : tableName;

    public static object? GetContext(this IQuery instance)
        => (instance as IContextualQuery)?.Context;

    public static IContextualQuery WithContext(this IQuery instance, object? context)
        => instance is IContextualQuery contextualQuery
            ? contextualQuery
            : new ContextualQueryWrapper(instance, context);

    private sealed class ContextualQueryWrapper : IContextualQuery
    {
        public ContextualQueryWrapper(IQuery instance, object? context)
        {
            Context = context;
            Limit = instance.Limit;
            Offset = instance.Offset;
            Conditions = instance.Conditions;
            SortOrders = instance.SortOrders;
        }

        public ContextualQueryWrapper(int? limit, int? offset, IEnumerable<ICondition> conditions, IEnumerable<ISortOrder> sortOrders, object? context)
        {
            Limit = limit;
            Offset = offset;
            Context = context;
            Conditions = conditions.ToList().AsReadOnly();
            SortOrders = sortOrders.ToList().AsReadOnly();
        }

        public object? Context { get; }

        public int? Limit { get; }

        public int? Offset { get; }

        public IReadOnlyCollection<ICondition> Conditions { get; }

        public IReadOnlyCollection<ISortOrder> SortOrders { get; }

        public IContextualQueryBuilder ToBuilder() => CreateBuilder();

        IQueryBuilder IQuery.ToBuilder() => CreateBuilder();

        private ContextualQueryWrapperBuilder CreateBuilder() => new ContextualQueryWrapperBuilder(this);
    }

    private sealed class ContextualQueryWrapperBuilder : IContextualQueryBuilder
    {
        public ContextualQueryWrapperBuilder(ContextualQueryWrapper entity)
        {
            Context = entity.Context;
            Limit = entity.Limit;
            Offset = entity.Offset;
            Conditions = entity.Conditions.Select(x => x.ToBuilder()).ToList();
            SortOrders = entity.SortOrders.Select(x => x.ToBuilder()).ToList();
        }

        public object? Context { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public List<IConditionBuilder> Conditions { get; set; } = new();
        public List<ISortOrderBuilder> SortOrders { get; set; } = new();

        public IContextualQuery Build() => CreateEntity();

        IQuery IQueryBuilder.Build() => CreateEntity();

        private ContextualQueryWrapper CreateEntity()
            => new ContextualQueryWrapper(Limit, Offset, Conditions.Select(x => x.Build()), SortOrders.Select(x => x.Build()), Context);
    }
}
