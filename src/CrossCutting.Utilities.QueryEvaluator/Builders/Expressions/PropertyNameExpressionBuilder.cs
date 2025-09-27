namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Expressions;

public partial class PropertyNameExpressionBuilder : IExpressionBuilder
{
    IExpression IBuilder<IExpression>.Build()
        => new PropertyNameExpression(SourceExpression?.Build()!, PropertyName);

    partial void SetDefaultValues()
    {
        SourceExpression = new ContextExpressionBuilder();
    }
}
