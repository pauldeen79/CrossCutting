namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public class SqlExpressionHandler : SqlExpressionProviderHandlerBase<SqlExpression>
{
    private readonly IExpressionEvaluator _expressionEvaluator;

    public SqlExpressionHandler(IExpressionEvaluator expressionEvaluator)
    {
        ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));
        _expressionEvaluator = expressionEvaluator;
    }

    protected override Result<string> DoGetSqlExpression(IQueryContext context, SqlExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, ISqlExpressionProvider callback)
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
            //TODO: Change ISqlExpressionProviderHandler signature so the method is async
            var expressionEvaluatorContext = new ExpressionEvaluatorContext(new ExpressionEvaluatorSettingsBuilder(), _expressionEvaluator, context.Context);
            return Result.Success(parameterBag.CreateQueryParameterName(expression.SourceExpression.EvaluateAsync(expressionEvaluatorContext, CancellationToken.None).Result.Value));
        }
    }
}
