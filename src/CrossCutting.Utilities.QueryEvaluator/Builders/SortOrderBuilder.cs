namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders;

public partial class SortOrderBuilder
{
    public SortOrderBuilder(IEvaluatableBuilder expression)
    {
        _expression = expression;
    }

    public SortOrderBuilder(IEvaluatableBuilder expression, SortOrderDirection order)
    {
        _expression = expression;
        _order = order;
    }
}
