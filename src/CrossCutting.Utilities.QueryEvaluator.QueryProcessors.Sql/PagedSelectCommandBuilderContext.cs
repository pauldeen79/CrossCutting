namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class PagedSelectCommandBuilderContext
{
    public PagedSelectCommandBuilderContext(
        IQueryContext context,
        IPagedDatabaseEntityRetrieverSettings settings,
        IQueryFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(settings, nameof(settings));
        ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        QueryContext = context;
        Settings = settings;
        FieldInfo = fieldInfo;
        SqlExpressionProvider = sqlExpressionProvider;
        ParameterBag = parameterBag;
    }

    public IQueryContext QueryContext { get; }
    public IPagedDatabaseEntityRetrieverSettings Settings { get; }
    public IQueryFieldInfo FieldInfo { get; }
    public ISqlExpressionProvider SqlExpressionProvider { get; }
    public ParameterBag ParameterBag { get; }
}