namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviderHandlers;

public class ConditionExpressionHandlerContext<TCondition>
{
    public ConditionExpressionHandlerContext(
        StringBuilder builder,
        object? context,
        TCondition condition,
        IQueryFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag)
    {
        ArgumentGuard.IsNotNull(builder, nameof(builder));
        ArgumentGuard.IsNotNull(condition, nameof(condition));
        ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        Builder = builder;
        Context = context;
        Condition = condition;
        FieldInfo = fieldInfo;
        SqlExpressionProvider = sqlExpressionProvider;
        ParameterBag = parameterBag;
    }

    public StringBuilder Builder { get; }
    public object? Context { get; }
    public TCondition Condition { get; }
    public IQueryFieldInfo FieldInfo { get; }
    public ISqlExpressionProvider SqlExpressionProvider { get; }
    public ParameterBag ParameterBag { get; }
}