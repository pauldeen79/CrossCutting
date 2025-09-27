namespace CrossCutting.Utilities.QueryEvaluator.Core.Expressions;

public sealed record PropertyNameExpression : IExpression
{
    [Required, ValidateObject]
    public IExpression SourceExpression { get; }

    [Required]
    public string PropertyName { get; }

    public PropertyNameExpression(IExpression sourceExpression, string propertyName)
    {
        SourceExpression = sourceExpression;
        PropertyName = propertyName;

        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

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

    public IExpressionBuilder ToBuilder() => new PropertyNameExpressionBuilder(this);
}
