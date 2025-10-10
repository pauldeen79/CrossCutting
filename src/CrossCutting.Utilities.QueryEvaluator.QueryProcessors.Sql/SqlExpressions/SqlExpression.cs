namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressions;

public class SqlExpression : ISqlExpression
{
    public SqlExpression(IEvaluatable sourceExpression)
    {
        ArgumentGuard.IsNotNull(sourceExpression, nameof(sourceExpression));

        SourceExpression = sourceExpression;
    }

    public IEvaluatable SourceExpression { get; }
}
