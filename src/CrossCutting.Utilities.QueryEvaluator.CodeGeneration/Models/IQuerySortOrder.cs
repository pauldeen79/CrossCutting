namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models;

internal interface IQuerySortOrder
{
    [Required][ValidateObject] IExpression<string> FieldNameExpression { get; }
    QuerySortOrderDirection Order { get; }
}
