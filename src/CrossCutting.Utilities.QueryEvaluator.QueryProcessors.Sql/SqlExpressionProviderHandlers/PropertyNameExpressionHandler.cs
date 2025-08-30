namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public class PropertyNameExpressionHandler : SqlExpressionProviderHandlerBase<PropertyNameExpression>
{
    protected override Result<string> DoGetSqlExpression(IQuery query, PropertyNameExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag)
    {
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));

        var databaseFieldName = fieldInfo.GetDatabaseFieldName(expression.PropertyName);

        return string.IsNullOrEmpty(databaseFieldName)
            ? Result.NotFound<string>($"Expression contains unknown field [{expression.PropertyName}]")
            : Result.Success(databaseFieldName!);
    }
}
