namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IOperatorContext
{
    [Required] Dictionary<string, Result> Results { get; }
    StringComparison StringComparison { get; }
}
