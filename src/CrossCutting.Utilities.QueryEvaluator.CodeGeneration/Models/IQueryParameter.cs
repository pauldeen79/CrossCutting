namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models;

internal interface IQueryParameter
{
    [Required] string Name { get; }
    object? Value { get; }
}
