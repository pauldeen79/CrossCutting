namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Expressions.Builders;

public partial class SqlLikeExpressionBuilder : IExpressionBuilder
{
    IExpression IBuilder<IExpression>.Build()
        => new SqlLikeExpression(SourceExpression?.Build()!, FormatString);
}
