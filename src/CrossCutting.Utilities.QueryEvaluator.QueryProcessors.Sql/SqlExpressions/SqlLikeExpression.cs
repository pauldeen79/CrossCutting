namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressions;

public class SqlLikeExpression : ISqlExpression
{
    public SqlLikeExpression(IEvaluatable sourceExpression, string formatString)
    {
        ArgumentGuard.IsNotNull(sourceExpression, nameof(sourceExpression));
        ArgumentGuard.IsNotNull(formatString, nameof(formatString));

        SourceExpression = sourceExpression;
        FormatString = formatString;
    }

    public IEvaluatable SourceExpression { get; }
    public string FormatString { get; }
}
