namespace CrossCutting.Utilities.QueryEvaluator.Core;

public static class ComposableEvaluatableBuilderHelper
{
    public static IConditionBuilder Create<T>(string fieldName, IConditionBuilder @operator, T value, Combination? combination = null, bool startGroup = false, bool endGroup = false, IEvaluatableBuilder? expression = null)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(expression ?? new PropertyNameEvaluatableBuilder().WithOperand(new ContextEvaluatableBuilder()).WithPropertyName(fieldName))
            .WithRightExpression(new LiteralEvaluatableBuilder<T>().WithValue(value))
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create<T>(string fieldName, IConditionBuilder @operator, Func<T> valueDelegate, Combination? combination = null, bool startGroup = false, bool endGroup = false, IEvaluatableBuilder? expression = null)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(expression ?? new PropertyNameEvaluatableBuilder().WithOperand(new ContextEvaluatableBuilder()).WithPropertyName(fieldName))
            .WithRightExpression(new DelegateEvaluatableBuilder<T>().WithValue(new Func<T>(() => valueDelegate())))
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(string fieldName, IConditionBuilder @operator, IEvaluatableBuilder rightExpression, Combination? combination = null, bool startGroup = false, bool endGroup = false, IEvaluatableBuilder? expression = null)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(expression ?? new PropertyNameEvaluatableBuilder().WithOperand(new ContextEvaluatableBuilder()).WithPropertyName(fieldName))
            .WithRightExpression(rightExpression)
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create<T>(IEvaluatableBuilder leftExpression, IConditionBuilder @operator, T value, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => value is IEvaluatableBuilder expressionBuilder
            ? Create(leftExpression, @operator, expressionBuilder, combination, startGroup, endGroup)
            : @operator
                .WithCombination(combination)
                .WithLeftExpression(leftExpression)
                .WithRightExpression(new LiteralEvaluatableBuilder<T>().WithValue(value))
                .WithStartGroup(startGroup)
                .WithEndGroup(endGroup);

    public static IConditionBuilder Create<T>(IEvaluatableBuilder leftExpression, IConditionBuilder @operator, Func<T> valueDelegate, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(leftExpression)
            .WithRightExpression(new DelegateEvaluatableBuilder<T>().WithValue(new Func<T>(() => valueDelegate())))
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(IEvaluatableBuilder leftExpression, IConditionBuilder @operator, IEvaluatableBuilder rightExpression, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(leftExpression)
            .WithRightExpression(rightExpression)
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(string fieldName, IConditionBuilder @operator, Combination? combination = null, bool startGroup = false, bool endGroup = false, IEvaluatableBuilder? expression = null)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(expression ?? new PropertyNameEvaluatableBuilder().WithOperand(new ContextEvaluatableBuilder()).WithPropertyName(fieldName))
            .WithRightExpression(new EmptyEvaluatableBuilder())
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(IEvaluatableBuilder leftExpression, IConditionBuilder @operator, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => @operator
            .WithCombination(combination)
            .WithLeftExpression(leftExpression)
            .WithRightExpression(new EmptyEvaluatableBuilder())
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);
}
