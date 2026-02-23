namespace CrossCutting.Utilities.ExpressionEvaluator.Sql;

public class EvaluatableSqlExpressionProvider : IEvaluatableSqlExpressionProvider
{
    private readonly ISqlExpressionProvider _sqlExpressionProvider;
    private readonly IExpressionEvaluator _expressionEvaluator;

    public EvaluatableSqlExpressionProvider(ISqlExpressionProvider sqlExpressionProvider, IExpressionEvaluator expressionEvaluator)
    {
        ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));
        _sqlExpressionProvider = sqlExpressionProvider;
        _expressionEvaluator = expressionEvaluator;
    }

    public async Task<Result> GetConditionExpressionAsync(SelectCommandBuilder selectCommandBuilder, object? context, IEvaluatable<bool> condition, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, CancellationToken token)
    {
        selectCommandBuilder = ArgumentGuard.IsNotNull(selectCommandBuilder, nameof(selectCommandBuilder));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));
        fieldNameProvider = ArgumentGuard.IsNotNull(fieldNameProvider, nameof(fieldNameProvider));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        if (condition is PropertyNameEvaluatable propertyNameEvaluatable && propertyNameEvaluatable.Operand is ContextEvaluatable)
        {
            var databaseFieldName = fieldNameProvider.GetDatabaseFieldName(propertyNameEvaluatable.PropertyName);

            return string.IsNullOrEmpty(databaseFieldName)
                ? Result.NotFound<string>($"Expression contains unknown field [{propertyNameEvaluatable.PropertyName}]")
                : Result.Success(databaseFieldName!);
        }
        else
        {
            var expressionEvaluatorContext = new ExpressionEvaluatorContext(new ExpressionEvaluatorSettingsBuilder(), _expressionEvaluator, context);

            return (await condition.EvaluateAsync(expressionEvaluatorContext, token).ConfigureAwait(false))
                .OnSuccess(value => Result.Success(parameterBag.CreateQueryParameterName(value)));
        }

        //await _sqlExpressionProvider.GetSqlExpressionAsync(context, )
    }
}