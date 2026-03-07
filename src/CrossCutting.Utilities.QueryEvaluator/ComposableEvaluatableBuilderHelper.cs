namespace CrossCutting.Utilities.QueryEvaluator.Core;

public static class ComposableEvaluatableBuilderHelper
{
    public static IConditionBuilder Create(string fieldName, IConditionBuilder @operator, object value, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(new PropertyNameEvaluatableBuilder().WithOperand(new ContextEvaluatableBuilder()).WithPropertyName(fieldName))
            .WithRightExpression(new LiteralEvaluatableBuilder().WithValue(value))
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(string fieldName, IConditionBuilder @operator, IEvaluatableBuilder rightExpression, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(new PropertyNameEvaluatableBuilder().WithOperand(new ContextEvaluatableBuilder()).WithPropertyName(fieldName))
            .WithRightExpression(rightExpression)
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(string fieldName, IConditionBuilder @operator, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(new PropertyNameEvaluatableBuilder().WithOperand(new ContextEvaluatableBuilder()).WithPropertyName(fieldName))
            .WithRightExpression(new EmptyEvaluatableBuilder(), true)
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(IEvaluatableBuilder leftExpression, IConditionBuilder @operator, object value, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(leftExpression)
            .WithRightExpression(new LiteralEvaluatableBuilder().WithValue(value))
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(IEvaluatableBuilder leftExpression, IConditionBuilder @operator, IEvaluatableBuilder rightExpression, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(leftExpression)
            .WithRightExpression(rightExpression)
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(IEvaluatableBuilder leftExpression, IConditionBuilder @operator, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(leftExpression)
            .WithRightExpression(new EmptyEvaluatableBuilder(), true)
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);
}
