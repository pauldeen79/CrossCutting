namespace CrossCutting.Utilities.QueryEvaluator.Core;

public class ComposableEvaluatableFieldNameBuilderWrapper<T> : ComposableEvaluatableBuilderWrapperBase<T>
    where T : IQueryBuilder
{
    private readonly string _fieldName;

    public ComposableEvaluatableFieldNameBuilderWrapper(T instance, string fieldName, Combination? combination = null)
        : base(instance, new EmptyEvaluatableBuilder(), combination)
    {
        _fieldName = fieldName.IsNotNull(nameof(fieldName));
    }

    protected override T AddFilterWithOperator(IConditionBuilder @operator, object? value)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, value, Combination, StartGroup, EndGroup));

    protected override T AddFilterWithOperator(IConditionBuilder @operator, IEvaluatableBuilder expression)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, expression, Combination, StartGroup, EndGroup));

    protected override T AddFilterWithOperator(IConditionBuilder @operator)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, Combination, StartGroup, EndGroup));
}
