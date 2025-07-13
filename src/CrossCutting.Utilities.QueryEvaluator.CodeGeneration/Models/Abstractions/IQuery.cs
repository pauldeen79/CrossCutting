namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface IQuery
{
    int? Limit { get; }
    int? Offset { get; }
    [Required][ValidateObject][ValidGroups] IReadOnlyCollection<ICondition> Conditions { get; }
    [Required][ValidateObject] IReadOnlyCollection<ISortOrder> SortOrders { get; }
}
