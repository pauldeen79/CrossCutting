namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Expressions;

public class EvaluatableExpressionBuilder : IEvaluatableBuilder
{
    public EvaluatableExpressionBuilder()
    {
        SourceExpression = string.Empty;
    }

    public EvaluatableExpressionBuilder(string sourceExpression)
    {
        SourceExpression = sourceExpression;
    }

    public string SourceExpression { get; set; }

    public EvaluatableExpressionBuilder WithSourceExpression(string sourceExpression)
    {
        SourceExpression = sourceExpression;
        return this;
    }

    public IEvaluatable Build()
        => BuildTyped();

    public EvaluatableExpression BuildTyped()
        => new EvaluatableExpression(SourceExpression);
}
