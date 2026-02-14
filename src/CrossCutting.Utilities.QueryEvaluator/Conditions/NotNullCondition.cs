namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record NotNullCondition : IChildEvaluatablesContainer
{
    public async override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => ConditionHelper.EvaluateObjectConditionAsync(SourceExpression, context, first => first is not null, token);

    public IEnumerable<IEvaluatable> GetChildEvaluatables()
        => [SourceExpression];
}
