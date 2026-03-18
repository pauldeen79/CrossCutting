namespace CrossCutting.Utilities.QueryEvaluator.Abstractions.Builders.Extensions;

public static partial class ConditionBuilderExtensions
{
    public static IConditionBuilder WithLeftExpression(this IConditionBuilder instance, IEvaluatableBuilder expression)
    {
        if (instance is not ISourceExpressionContainerBuilder sourceExpressionContainerBuilder)
        {
            throw new ArgumentException($"Condition {instance.GetType().FullName} is not assignable to {nameof(ICompareExpressionContainerBuilder)}");
        }

        sourceExpressionContainerBuilder.WithSourceExpression(expression);
        
        return instance;
    }

    public static IConditionBuilder WithRightExpression(this IConditionBuilder instance, IEvaluatableBuilder expression, bool allowEmpty = false)
    {
        if (instance is not ICompareExpressionContainerBuilder compareExpressionContainerBuilder)
        {
            if (allowEmpty)
            {
                return instance;
            }
            
            throw new ArgumentException($"Condition {instance.GetType().FullName} is not assignable to {nameof(ICompareExpressionContainerBuilder)}");
        }
        
        compareExpressionContainerBuilder.WithCompareExpression(expression);
        
        return instance;
    }
}