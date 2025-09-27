namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Builders.Expressions;

public sealed class SqlLikeExpressionBuilder : IExpressionBuilder
{
    [Required, ValidateObject]
    public IExpressionBuilder SourceExpression { get; set; }

    [Required]
    public string FormatString { get; set; }

    public SqlLikeExpressionBuilder()
    {
        SourceExpression = new EmptyExpressionBuilder();
        FormatString = "{0}";
    }

    public SqlLikeExpressionBuilder(SqlLikeExpression sqlLikeExpression)
    {
        sqlLikeExpression = ArgumentGuard.IsNotNull(sqlLikeExpression, nameof(sqlLikeExpression));

        SourceExpression = sqlLikeExpression.SourceExpression.ToBuilder();
        FormatString = sqlLikeExpression.FormatString;
    }

    public SqlLikeExpressionBuilder WithSourceExpression(IExpressionBuilder sourceExpression)
        => this.With(x => x.SourceExpression = sourceExpression);

    public SqlLikeExpressionBuilder WithFormatString(string formatString)
        => this.With(x => x.FormatString = formatString);

    public IExpression Build()
        => new SqlLikeExpression(SourceExpression?.Build()!, FormatString);
}
