namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Expressions;

public sealed class SqlLikeExpression : IExpression
{
    [Required]
    [AllowedTypes([typeof(ContextExpression), typeof(DelegateExpression), typeof(LiteralExpression)])]
    public IExpression SourceExpression { get; }

    [Required]
    public string FormatString { get; }

    public SqlLikeExpression(IExpression sourceExpression, string formatString)
    {
        SourceExpression = sourceExpression;
        FormatString = formatString;

        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => SourceExpression.EvaluateAsync(context, token);

    public IExpressionBuilder ToBuilder()
        => new SqlLikeExpressionBuilder(SourceExpression, FormatString);
}
