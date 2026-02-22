namespace CrossCutting.Utilities.QueryEvaluator.Core;

public class ComposableEvaluatableFieldNameBuilderWrapper<T> : ComposableEvaluatableBuilderWrapperBase<T>
    where T : IQueryBuilder
{
    private readonly string _fieldName;
    private IEvaluatableBuilder? _expression;

    public ComposableEvaluatableFieldNameBuilderWrapper(T instance, string fieldName, Combination? combination = null) : base(instance, new EmptyEvaluatableBuilder(), combination)
    {
        _fieldName = fieldName.IsNotNull(nameof(fieldName));
    }
/*
    #region Built-in functions
    /// <summary>Gets the length of this field.</summary>
    public ComposableEvaluatableFieldNameBuilderWrapper<T> Len()
    {
        _expression = new StringLengthExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Trims the value of this field.</summary>
    public ComposableEvaluatableFieldNameBuilderWrapper<T> SqlTrim()
    {
        _expression = new TrimExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the upper-cased value of this field.</summary>
    public ComposableEvaluatableFieldNameBuilderWrapper<T> Upper()
    {
        _expression = new ToUpperCaseExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the lower-cased value of this field.</summary>
    public ComposableEvaluatableFieldNameBuilderWrapper<T> Lower()
    {
        _expression = new ToLowerCaseExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the left part of this expression.</summary>
    /// <param name="length">Number of positions</param>
    public ComposableEvaluatableFieldNameBuilderWrapper<T> Left(int length)
    {
        _expression = new LeftExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName)).WithLengthExpression(new TypedConstantExpressionBuilder<int>().WithValue(length));
        return this;
    }

    /// <summary>Gets the right part of this expression.</summary>
    /// <param name="length">Number of positions</param>
    public ComposableEvaluatableFieldNameBuilderWrapper<T> Right(int length)
    {
        _expression = new RightExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName)).WithLengthExpression(new TypedConstantExpressionBuilder<int>().WithValue(length));
        return this;
    }

    /// <summary>Gets the year of this date field.</summary>
    public ComposableEvaluatableFieldNameBuilderWrapper<T> Year()
    {
        _expression = new YearExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<DateTime>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the month of this date field.</summary>
    public ComposableEvaluatableFieldNameBuilderWrapper<T> Month()
    {
        _expression = new MonthExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<DateTime>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the day of this date expression.</summary>
    public ComposableEvaluatableFieldNameBuilderWrapper<T> Day()
    {
        _expression = new DayExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<DateTime>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the count of this field.</summary>
    public ComposableEvaluatableFieldNameBuilderWrapper<T> Count()
    {
        _expression = new CountExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<IEnumerable>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the sum of this field.</summary>
    public ComposableEvaluatableFieldNameBuilderWrapper<T> Sum()
    {
        _expression = new SumExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<IEnumerable>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }
    #endregion
*/
    public ComposableEvaluatableFieldNameBuilderWrapper<T> WithStartGroup(bool startGroup = true)
    {
        StartGroup = startGroup;
        return this;
    }

    public ComposableEvaluatableFieldNameBuilderWrapper<T> WithEndGroup(bool endGroup = true)
    {
        EndGroup = endGroup;
        return this;
    }

    public ComposableEvaluatableFieldNameBuilderWrapper<T> WithCombination(Combination combination)
    {
        Combination = combination;
        return this;
    }

    protected override T AddFilterWithOperator<TValue>(IEvaluatableBuilder @operator, TValue value)
        => value is IEvaluatableBuilder expressionBuilder
            ? AddFilterWithOperator(@operator, expressionBuilder)
            : Instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, value, Combination, StartGroup, EndGroup, _expression));

    protected override T AddFilterWithOperator<TValue>(IEvaluatableBuilder @operator, Func<TValue> valueDelegate)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, valueDelegate, Combination, StartGroup, EndGroup, _expression));

    protected override T AddFilterWithOperator(IEvaluatableBuilder @operator, IEvaluatableBuilder expression)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, expression, Combination, StartGroup, EndGroup, _expression));

    protected override T AddFilterWithOperator(IEvaluatableBuilder @operator)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, Combination, StartGroup, EndGroup, _expression));
}
