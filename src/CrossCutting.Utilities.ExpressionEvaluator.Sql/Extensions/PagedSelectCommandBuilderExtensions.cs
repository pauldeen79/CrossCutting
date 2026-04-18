namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Extensions;

public static class PagedSelectCommandBuilderExtensions
{
    public static PagedSelectCommandBuilder WithSqlExpression(this PagedSelectCommandBuilder instance, SqlExpression sqlExpressionData)
    {   
        sqlExpressionData = ArgumentGuard.IsNotNull(sqlExpressionData, nameof(sqlExpressionData));

        return instance
            .Where(sqlExpressionData.Expression)
            .AppendParameters(sqlExpressionData.Parameters);
    }
}