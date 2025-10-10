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
            // TODO: Change ISqlExpressionProviderHandler signature so the method is async
            // TODO: Change callback argument to new ISqlExpressionProvider interface, which has an Evaluate method that takes only an expression, and builds the context itself using the context of IQueryContext.Context
            var state = context.Context is not null
                ? new AsyncResultDictionaryBuilder<object?>()
                    .Add(Constants.Context, context.Context)
                    .BuildDeferred()
                : null;
            var expressionEvaluatorContext = new ExpressionEvaluatorContext("Dummy", new ExpressionEvaluatorSettingsBuilder(), _expressionEvaluator, state);
            return Result.Success(parameterBag.CreateQueryParameterName(expression.SourceExpression.EvaluateAsync(expressionEvaluatorContext, CancellationToken.None).Result.Value));
        }
    }
}
