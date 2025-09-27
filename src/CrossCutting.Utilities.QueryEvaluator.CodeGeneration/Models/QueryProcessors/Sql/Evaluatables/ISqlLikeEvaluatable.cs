namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.QueryProcessors.Sql.Evaluatables;

internal interface ISqlLikeEvaluatable
{
    [Required, ValidateObject]
    IEvaluatable SourceExpression { get; set; }

    [Required]
    string FormatString { get; set; }
}
