namespace CrossCutting.Utilities.QueryEvaluator.Core;

public abstract class ComposableEvaluatableBuilderWrapperBase<T>
    where T : IQueryBuilder
{
    private readonly IEvaluatableBuilder _leftExpression;

    protected T Instance { get; }

    protected Combination? Combination { get; set; }
    protected bool StartGroup { get; set; }
    protected bool EndGroup { get; set; }

    protected ComposableEvaluatableBuilderWrapperBase(T instance, IEvaluatableBuilder leftExpression, Combination? combination = null)
    {
        Instance = instance.IsNotNull(nameof(instance));
        _leftExpression = leftExpression.IsNotNull(nameof(leftExpression));
        Combination = combination;
    }

    #region Generated code
    public T IsEqualTo<TValue>(TValue value)
        => AddFilterWithOperator(new EqualConditionBuilder(), value);

    public T IsGreaterOrEqualThan<TValue>(TValue value)
        => AddFilterWithOperator(new GreaterThanOrEqualConditionBuilder(), value);

    public T IsGreaterThan<TValue>(TValue value)
        => AddFilterWithOperator(new GreaterThanConditionBuilder(), value);

    public T IsNotNull()
        => AddFilterWithOperator(new NotNullConditionBuilder());

    public T IsNull()
        => AddFilterWithOperator(new NullConditionBuilder());

    public T IsSmallerOrEqualThan<TValue>(TValue value)
        => AddFilterWithOperator(new SmallerThanOrEqualConditionBuilder(), value);

    public T IsSmallerThan<TValue>(TValue value)
        => AddFilterWithOperator(new SmallerThanConditionBuilder(), value);

    public T IsNotEqualTo<TValue>(TValue value)
        => AddFilterWithOperator(new NotEqualConditionBuilder(), value);
    #endregion

    protected virtual T AddFilterWithOperator<TValue>(IConditionBuilder @operator, TValue value)
        => value is IEvaluatableBuilder expressionBuilder
            ? AddFilterWithOperator(@operator, expressionBuilder)
            : Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, value, Combination, StartGroup, EndGroup));

    protected virtual T AddFilterWithOperator(IConditionBuilder @operator, IEvaluatableBuilder expression)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, expression, Combination, StartGroup, EndGroup));

    protected virtual T AddFilterWithOperator(IConditionBuilder @operator)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, Combination, StartGroup, EndGroup));
}
