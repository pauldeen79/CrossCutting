namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Expressions;

public partial class DynamicExpressionBuilder
{
    public DynamicExpressionBuilder(IExpressionBuilder expression)
    {
        ArgumentGuard.IsNotNull(expression, nameof(expression));

        _expression = expression;
    }

    partial void SetDefaultValues()
    {
        _expression = new EmptyExpressionBuilder();
    }
}
