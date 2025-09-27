namespace CrossCutting.Utilities.QueryEvaluator.Core.Expressions;

public partial record PropertyNameExpression : IExpression
{
    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await SourceExpression.EvaluateAsync(context, token)
            .ConfigureAwait(false))
            .EnsureNotNull("Expression evaluation resulted in null")
            .OnSuccess(valueResult =>
            {
                var property = valueResult.Value!.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.Public);
                if (property is null)
                {
                    return Result.Invalid<object?>($"Type {valueResult.Value.GetType().FullName} does not contain property {PropertyName}");
                }

                return Result.WrapException<object?>(() => property.GetValue(valueResult.Value));
            });

    public Task<ExpressionParseResult> ParseAsync(CancellationToken token)
        => Task.FromResult(new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.NotSupported).Build());

    IExpressionBuilder IBuildableEntity<IExpressionBuilder>.ToBuilder() => new PropertyNameExpressionBuilder(this);
}
