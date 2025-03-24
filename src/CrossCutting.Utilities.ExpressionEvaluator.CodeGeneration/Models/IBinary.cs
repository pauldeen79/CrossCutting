namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IBinary
{
    [Required][ValidateObject] IReadOnlyCollection<IBinaryCondition> Conditions { get; }
}
