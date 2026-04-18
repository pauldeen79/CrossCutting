namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.SqlExpressions;

public class PlainExpression : ISqlExpression
{
    public PlainExpression(IEvaluatable sourceExpression)
    {
        ArgumentGuard.IsNotNull(sourceExpression, nameof(sourceExpression));

        SourceExpression = sourceExpression;
    }

    public IEvaluatable SourceExpression { get; }
}
