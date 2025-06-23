namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface IQuery
{
    int? Limit { get; }
    int? Offset { get; }
    [Required][ValidateObject][ValidGroups] IReadOnlyCollection<ICondition> Filter { get; }
    [Required][ValidateObject] IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
}
