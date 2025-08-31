namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Builders.Expressions;

public sealed class SqlLikeExpressionBuilder : IExpressionBuilder
{
    [Required]
    public IExpression SourceExpression { get; set; }
    [Required]
    public string FormatString { get; set; }

    public SqlLikeExpressionBuilder()
    {
        SourceExpression = default!;
        FormatString = "{0}";
    }

    public SqlLikeExpressionBuilder(IExpression sourceExpression, string formatString)
    {
        SourceExpression = sourceExpression;
        FormatString = formatString ?? "{0}";
    }

    public IExpression Build() => new SqlLikeExpression(SourceExpression, FormatString);

    public SqlLikeExpressionBuilder WithSourceExpression(IExpression sourceExpression)
    {
        SourceExpression = sourceExpression;
        return this;
    }

    public SqlLikeExpressionBuilder WithFormatString(string formatString)
    {
        FormatString = formatString;
        return this;
    }
}
