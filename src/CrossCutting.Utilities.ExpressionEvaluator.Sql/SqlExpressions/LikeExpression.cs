namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.SqlExpressions;

public class LikeExpression : ISqlExpression
{
    public LikeExpression(IEvaluatable sourceExpression, string formatString)
    {
        ArgumentGuard.IsNotNull(sourceExpression, nameof(sourceExpression));
        ArgumentGuard.IsNotNull(formatString, nameof(formatString));

        SourceExpression = sourceExpression;
        FormatString = formatString;
    }

    public IEvaluatable SourceExpression { get; }
    public string FormatString { get; }
}
