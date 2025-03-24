namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IComparison
{
    [Required][ValidateObject] IReadOnlyCollection<IComparisonCondition> Conditions { get; }
}
