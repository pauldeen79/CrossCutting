namespace CrossCutting.Utilities.QueryEvaluator.Abstractions.Builders.Extensions;

public static partial class ConditionBuilderExtensions
{
    public static IConditionBuilder WithLeftExpression(this IConditionBuilder instance, IEvaluatableBuilder expression)
    {
        (instance as ISourceExpressionContainerBuilder)?.WithSourceExpression(expression);
        
        return instance;
    }

    public static IConditionBuilder WithRightExpression(this IConditionBuilder instance, IEvaluatableBuilder expression)
    {
        (instance as ICompareExpressionContainerBuilder)?.WithCompareExpression(expression);
        
        return instance;
    }
}