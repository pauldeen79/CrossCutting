namespace CrossCutting.Utilities.QueryEvaluator.Core;

public static class ComposableEvaluatableBuilderHelper
{
    public static IConditionBuilder Create<T>(string fieldName, IEvaluatableBuilder @operator, T value, Combination? combination = null, bool startGroup = false, bool endGroup = false, IEvaluatableBuilder? expression = null)
        => CreateCondition(@operator)
            .WithCombination(combination)
            .WithLeftExpression(expression ?? new PropertyNameEvaluatableBuilder().WithOperand(new ContextEvaluatableBuilder()).WithPropertyName(fieldName))
            //.WithOperator(@operator)
            .WithRightExpression(new LiteralEvaluatableBuilder<T>().WithValue(value))
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create<T>(string fieldName, IEvaluatableBuilder @operator, Func<T> valueDelegate, Combination? combination = null, bool startGroup = false, bool endGroup = false, IEvaluatableBuilder? expression = null)
        => CreateCondition(@operator)
            .WithCombination(combination)
            .WithLeftExpression(expression ?? new PropertyNameEvaluatableBuilder().WithOperand(new ContextEvaluatableBuilder()).WithPropertyName(fieldName))
            //.WithOperator(@operator)
            .WithRightExpression(new DelegateEvaluatableBuilder<T>().WithValue(new Func<T>(() => valueDelegate())))
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(string fieldName, IEvaluatableBuilder @operator, IEvaluatableBuilder rightExpression, Combination? combination = null, bool startGroup = false, bool endGroup = false, IEvaluatableBuilder? expression = null)
        => CreateCondition(@operator)
            .WithCombination(combination)
            .WithLeftExpression(expression ?? new PropertyNameEvaluatableBuilder().WithOperand(new ContextEvaluatableBuilder()).WithPropertyName(fieldName))
            //.WithOperator(@operator)
            .WithRightExpression(rightExpression)
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create<T>(IEvaluatableBuilder leftExpression, IEvaluatableBuilder @operator, T value, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => value is IEvaluatableBuilder expressionBuilder
            ? Create(leftExpression, @operator, expressionBuilder, combination, startGroup, endGroup)
            : @CreateCondition(@operator)
                .WithCombination(combination)
                .WithLeftExpression(leftExpression)
                //.WithOperator(@operator)
                .WithRightExpression(new LiteralEvaluatableBuilder<T>().WithValue(value))
                .WithStartGroup(startGroup)
                .WithEndGroup(endGroup);

    public static IConditionBuilder Create<T>(IEvaluatableBuilder leftExpression, IEvaluatableBuilder @operator, Func<T> valueDelegate, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => CreateCondition(@operator)
            .WithCombination(combination)
            .WithLeftExpression(leftExpression)
            //.WithOperator(@operator)
            .WithRightExpression(new DelegateEvaluatableBuilder<T>().WithValue(new Func<T>(() => valueDelegate())))
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(IEvaluatableBuilder leftExpression, IEvaluatableBuilder @operator, IEvaluatableBuilder rightExpression, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => CreateCondition(@operator)
            .WithCombination(combination)
            .WithLeftExpression(leftExpression)
            //.WithOperator(@operator)
            .WithRightExpression(rightExpression)
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(string fieldName, IEvaluatableBuilder @operator, Combination? combination = null, bool startGroup = false, bool endGroup = false, IEvaluatableBuilder? expression = null)
        => CreateCondition(@operator)
            .WithCombination(combination)
            .WithLeftExpression(expression ?? new PropertyNameEvaluatableBuilder().WithOperand(new ContextEvaluatableBuilder()).WithPropertyName(fieldName))
            //.WithOperator(@operator)
            .WithRightExpression(new EmptyEvaluatableBuilder())
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static IConditionBuilder Create(IEvaluatableBuilder leftExpression, IEvaluatableBuilder @operator, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => CreateCondition(@operator)
            .WithCombination(combination)
            .WithLeftExpression(leftExpression)
            //.WithOperator(@operator)
            .WithRightExpression(new EmptyEvaluatableBuilder())
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    //TODO: Fix open/closed principle, maybe create a new interface that you need to implement on your 'conditionable' evaluatable builders
    private static IConditionBuilder CreateCondition(IEvaluatableBuilder @operator)
        => @operator switch
        {
            EqualOperatorEvaluatableBuilder => new EqualConditionBuilder(),
            NotEqualOperatorEvaluatableBuilder => new NotEqualConditionBuilder(),
            GreaterOperatorEvaluatableBuilder => new GreaterThanConditionBuilder(),
            GreaterOrEqualOperatorEvaluatableBuilder => new GreaterThanOrEqualConditionBuilder(),
            SmallerOperatorEvaluatableBuilder => new SmallerThanConditionBuilder(),
            SmallerOrEqualOperatorEvaluatableBuilder => new SmallerThanOrEqualConditionBuilder(),
            NullOperatorEvaluatableBuilder => new NullConditionBuilder(),
            NotNullOperatorEvaluatableBuilder => new NotNullConditionBuilder(),
            _ => throw new NotSupportedException($"Operator of type {@operator.GetType().FullName} is not supported")
        };
}
