namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders;

public partial class SortOrderBuilder
{
    public SortOrderBuilder(IExpressionBuilder expression)
    {
        ArgumentGuard.IsNotNull(expression, nameof(expression));

        _expression = expression;
    }

    public SortOrderBuilder(IExpressionBuilder expression, SortOrderDirection order)
    {
        ArgumentGuard.IsNotNull(expression, nameof(expression));
        ArgumentGuard.IsNotNull(order, nameof(order));

        _expression = expression;
        _order = order;
    }
}
