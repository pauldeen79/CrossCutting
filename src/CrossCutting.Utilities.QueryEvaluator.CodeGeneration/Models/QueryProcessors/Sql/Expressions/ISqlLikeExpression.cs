namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.QueryProcessors.Sql.Expressions;

internal interface ISqlLikeExpression
{
    [Required, ValidateObject]
    IExpression SourceExpression { get; set; }

    [Required]
    string FormatString { get; set; }
}
