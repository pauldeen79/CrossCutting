namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Extensions;

public static class SelectCommandBuilderExtensions
{
    public static SelectCommandBuilder WithSqlExpression(this SelectCommandBuilder instance, SqlExpression sqlExpressionData)
    {   
        sqlExpressionData = ArgumentGuard.IsNotNull(sqlExpressionData, nameof(sqlExpressionData));

        return instance
            .Where(sqlExpressionData.Expression)
            .AppendParameters(sqlExpressionData.Parameters);
    }
}