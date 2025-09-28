namespace CrossCutting.Utilities.QueryEvaluator.Core.Evaluatables;

public partial record PropertyNameEvaluatable : IEvaluatable
{
    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await SourceExpression.EvaluateAsync(context, token)
            .ConfigureAwait(false))
            .EnsureNotNull("Expression evaluation resulted in null")
            .EnsureValue("Expression evaluation result value is null")
            .OnSuccess(valueResult =>
            {
                var property = valueResult.Value!.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.Public);
                if (property is null)
                {
                    return Result.Error<object?>($"Type {valueResult.Value.GetType().FullName} does not contain property {PropertyName}");
                }

                return Result.WrapException<object?>(() => property.GetValue(valueResult.Value));
            });

    IEvaluatableBuilder IBuildableEntity<IEvaluatableBuilder>.ToBuilder() => new PropertyNameEvaluatableBuilder(this);
}
