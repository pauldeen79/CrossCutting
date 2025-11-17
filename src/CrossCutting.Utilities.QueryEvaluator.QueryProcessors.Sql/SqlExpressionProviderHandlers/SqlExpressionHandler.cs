namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public class SqlExpressionHandler : SqlExpressionProviderHandlerBase<SqlExpression>
{
    private readonly IExpressionEvaluator _expressionEvaluator;

    public SqlExpressionHandler(IExpressionEvaluator expressionEvaluator)
    {
        ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));
        _expressionEvaluator = expressionEvaluator;
    }

    protected override async Task<Result<string>> DoGetSqlExpressionAsync(IQueryContext context, SqlExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, ISqlExpressionProvider callback, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));

        if (expression.SourceExpression is PropertyNameEvaluatable propertyNameEvaluatable)
        {
            var databaseFieldName = fieldInfo.GetDatabaseFieldName(propertyNameEvaluatable.PropertyName);

            return string.IsNullOrEmpty(databaseFieldName)
                ? Result.NotFound<string>($"Expression contains unknown field [{propertyNameEvaluatable.PropertyName}]")
                : Result.Success(databaseFieldName!);
        }
        else
        {
            var expressionEvaluatorContext = new ExpressionEvaluatorContext(new ExpressionEvaluatorSettingsBuilder(), _expressionEvaluator, context.Context);

            return (await expression.SourceExpression.EvaluateAsync(expressionEvaluatorContext, token).ConfigureAwait(false))
                .OnSuccess(value => Result.Success(parameterBag.CreateQueryParameterName(value)));
        }
    }
}
