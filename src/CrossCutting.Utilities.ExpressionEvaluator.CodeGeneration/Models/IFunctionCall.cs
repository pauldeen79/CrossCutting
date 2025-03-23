namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IFunctionCall
{
    [Required] string Name { get; }
    [Required][ValidateObject] IReadOnlyCollection<string> Arguments { get; }
    [Required] IReadOnlyCollection<string> TypeArguments { get; }
}
