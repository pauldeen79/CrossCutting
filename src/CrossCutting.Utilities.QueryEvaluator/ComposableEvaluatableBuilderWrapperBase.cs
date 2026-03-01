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
        => AddFilterWithOperator(new EqualOperatorEvaluatableBuilder(), value);

    public T IsEqualTo<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new EqualOperatorEvaluatableBuilder(), valueDelegate);

    // public T IsEqualToParameter(string parameterName)
    //     => AddFilterWithOperator(new EqualOperatorEvaluatableBuilder(), new QueryParameterExpressionBuilder().WithParameterName(parameterName));

    public T IsGreaterOrEqualThan<TValue>(TValue value)
        => AddFilterWithOperator(new GreaterOrEqualOperatorEvaluatableBuilder(), value);

    public T IsGreaterOrEqualThan<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new GreaterOrEqualOperatorEvaluatableBuilder(), valueDelegate);

    public T IsGreaterThan<TValue>(TValue value)
        => AddFilterWithOperator(new GreaterOperatorEvaluatableBuilder(), value);

    public T IsGreaterThan<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new GreaterOperatorEvaluatableBuilder(), valueDelegate);

    public T IsNotNull()
        => AddFilterWithOperator(new NotNullOperatorEvaluatableBuilder());

    public T IsNull()
        => AddFilterWithOperator(new NullOperatorEvaluatableBuilder());

    public T IsSmallerOrEqualThan<TValue>(TValue value)
        => AddFilterWithOperator(new SmallerOrEqualOperatorEvaluatableBuilder(), value);

    public T IsSmallerOrEqualThan<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new SmallerOrEqualOperatorEvaluatableBuilder(), valueDelegate);

    public T IsSmallerThan<TValue>(TValue value)
        => AddFilterWithOperator(new SmallerOperatorEvaluatableBuilder(), value);

    public T IsSmallerThan<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new SmallerOperatorEvaluatableBuilder(), valueDelegate);

    public T IsNotEqualTo<TValue>(TValue value)
        => AddFilterWithOperator(new NotEqualOperatorEvaluatableBuilder(), value);

    public T IsNotEqualTo<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new NotEqualOperatorEvaluatableBuilder(), valueDelegate);
    #endregion

    protected virtual T AddFilterWithOperator<TValue>(IEvaluatableBuilder @operator, TValue value)
        => value is IEvaluatableBuilder expressionBuilder
            ? AddFilterWithOperator(@operator, expressionBuilder)
            : Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, value, Combination, StartGroup, EndGroup));

    protected virtual T AddFilterWithOperator<TValue>(IEvaluatableBuilder @operator, Func<TValue> valueDelegate)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, valueDelegate, Combination, StartGroup, EndGroup));

    protected virtual T AddFilterWithOperator(IEvaluatableBuilder @operator, IEvaluatableBuilder expression)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, expression, Combination, StartGroup, EndGroup));

    protected virtual T AddFilterWithOperator(IEvaluatableBuilder @operator)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, Combination, StartGroup, EndGroup));
}
