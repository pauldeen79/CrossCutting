namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IBinaryConditionGroup
{
    [Required][ValidateObject] IReadOnlyCollection<IBinaryCondition> Conditions { get; }
}
