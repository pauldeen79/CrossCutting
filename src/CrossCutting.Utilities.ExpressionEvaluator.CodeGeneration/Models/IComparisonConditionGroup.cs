namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IComparisonConditionGroup
{
    [Required][ValidateObject] IReadOnlyCollection<IComparisonCondition> Conditions { get; }
}
