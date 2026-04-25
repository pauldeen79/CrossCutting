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
    public T IsEqualTo(object value)
        => AddFilterWithOperator(new EqualConditionBuilder(), value);

    public T IsEqualTo(IEvaluatableBuilder expression)
        => AddFilterWithOperator(new EqualConditionBuilder(), expression);

    public T IsGreaterOrEqualThan(object value)
        => AddFilterWithOperator(new GreaterThanOrEqualConditionBuilder(), value);

    public T IsGreaterOrEqualThan(IEvaluatableBuilder expression)
        => AddFilterWithOperator(new GreaterThanOrEqualConditionBuilder(), expression);

    public T IsGreaterThan(object value)
        => AddFilterWithOperator(new GreaterThanConditionBuilder(), value);

    public T IsGreaterThan(IEvaluatableBuilder expression)
        => AddFilterWithOperator(new GreaterThanConditionBuilder(), expression);

    public T IsSmallerOrEqualThan(object value)
        => AddFilterWithOperator(new SmallerThanOrEqualConditionBuilder(), value);

    public T IsSmallerOrEqualThan(IEvaluatableBuilder expression)
        => AddFilterWithOperator(new SmallerThanOrEqualConditionBuilder(), expression);

    public T IsSmallerThan(object value)
        => AddFilterWithOperator(new SmallerThanConditionBuilder(), value);

    public T IsSmallerThan(IEvaluatableBuilder expression)
        => AddFilterWithOperator(new SmallerThanConditionBuilder(), expression);

    public T IsNotEqualTo(object value)
        => AddFilterWithOperator(new NotEqualConditionBuilder(), value);

    public T IsNotEqualTo(IEvaluatableBuilder expression)
        => AddFilterWithOperator(new NotEqualConditionBuilder(), expression);

    public T IsNotNull()
        => AddFilterWithOperator(new NotNullConditionBuilder());

    public T IsNull()
        => AddFilterWithOperator(new NullConditionBuilder());

    public T StartsWith(string value)
        => AddFilterWithOperator(new StringStartsWithConditionBuilder(), value);

    public T StartsWith(IEvaluatableBuilder expression)
        => AddFilterWithOperator(new StringStartsWithConditionBuilder(), expression);

    public T DoesNotStartWith(string value)
        => AddFilterWithOperator(new StringNotStartsWithConditionBuilder(), value);

    public T DoesNotStartWith(IEvaluatableBuilder expression)
        => AddFilterWithOperator(new StringNotStartsWithConditionBuilder(), expression);

    public T EndsWith(string value)
        => AddFilterWithOperator(new StringEndsWithConditionBuilder(), value);

    public T EndsWith(IEvaluatableBuilder expression)
        => AddFilterWithOperator(new StringEndsWithConditionBuilder(), expression);

    public T DoesNotEndWith(string value)
        => AddFilterWithOperator(new StringNotEndsWithConditionBuilder(), value);

    public T DoesNotEndWith(IEvaluatableBuilder expression)
        => AddFilterWithOperator(new StringNotEndsWithConditionBuilder(), expression);

    public T Contains(string value)
        => AddFilterWithOperator(new StringContainsConditionBuilder(), value);

    public T Contains(IEvaluatableBuilder expression)
        => AddFilterWithOperator(new StringContainsConditionBuilder(), expression);

    public T DoesNotContain(string value)
        => AddFilterWithOperator(new StringNotContainsConditionBuilder(), value);

    public T DoesNotContain(IEvaluatableBuilder expression)
        => AddFilterWithOperator(new StringNotContainsConditionBuilder(), expression);
    #endregion

    protected virtual T AddFilterWithOperator(IConditionBuilder @operator, object? value)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, value, Combination, StartGroup, EndGroup));

    protected virtual T AddFilterWithOperator(IConditionBuilder @operator, IEvaluatableBuilder expression)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, expression, Combination, StartGroup, EndGroup));

    protected virtual T AddFilterWithOperator(IConditionBuilder @operator)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, Combination, StartGroup, EndGroup));
}
