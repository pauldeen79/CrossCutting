namespace CrossCutting.Utilities.QueryEvaluator.Core;

public class ComposableEvaluatableFieldNameBuilderWrapper<T> : ComposableEvaluatableBuilderWrapperBase<T>
    where T : IQueryBuilder
{
    private readonly string _fieldName;

    public ComposableEvaluatableFieldNameBuilderWrapper(T instance, string fieldName, Combination? combination = null) : base(instance, new EmptyEvaluatableBuilder(), combination)
    {
        _fieldName = fieldName.IsNotNull(nameof(fieldName));
    }

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
            : Instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, value, Combination, StartGroup, EndGroup));

    protected override T AddFilterWithOperator<TValue>(IEvaluatableBuilder @operator, Func<TValue> valueDelegate)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, valueDelegate, Combination, StartGroup, EndGroup));

    protected override T AddFilterWithOperator(IEvaluatableBuilder @operator, IEvaluatableBuilder expression)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, expression, Combination, StartGroup, EndGroup));

    protected override T AddFilterWithOperator(IEvaluatableBuilder @operator)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, Combination, StartGroup, EndGroup));
}
