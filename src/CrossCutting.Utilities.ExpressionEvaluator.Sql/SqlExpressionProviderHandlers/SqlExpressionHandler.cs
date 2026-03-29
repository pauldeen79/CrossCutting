namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.SqlExpressionProviderHandlers;

public class SqlExpressionHandler : SqlExpressionProviderHandlerBase<SqlExpression>
{
    private readonly IExpressionEvaluator _expressionEvaluator;

    public SqlExpressionHandler(IExpressionEvaluator expressionEvaluator)
    {
        ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));
        _expressionEvaluator = expressionEvaluator;
    }

    protected override async Task<Result<string>> HandleGetSqlExpressionAsync(object? context, SqlExpression expression, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, ISqlExpressionProvider callback, CancellationToken token)
    {
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        fieldNameProvider = ArgumentGuard.IsNotNull(fieldNameProvider, nameof(fieldNameProvider));

        if (expression.SourceExpression is PropertyNameEvaluatable propertyNameEvaluatable && propertyNameEvaluatable.Operand is ContextEvaluatable)
        {
            var databaseFieldName = fieldNameProvider.GetDatabaseFieldName(propertyNameEvaluatable.PropertyName);

            return string.IsNullOrEmpty(databaseFieldName)
                ? Result.NotFound<string>($"Expression contains unknown field [{propertyNameEvaluatable.PropertyName}]")
                : databaseFieldName!;
        }
        else
        {
            var expressionEvaluatorContext = new ExpressionEvaluatorContext(new ExpressionEvaluatorSettingsBuilder(), _expressionEvaluator, context);

            return (await expression.SourceExpression.EvaluateAsync(expressionEvaluatorContext, token).ConfigureAwait(false))
                .OnSuccess(value => parameterBag.CreateQueryParameterName(value));
        }
    }
}
