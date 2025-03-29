namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IBinaryCondition
{
    Combination? Combination { get; }
    [Required] string Expression { get; }
    bool StartGroup { get; }
    bool EndGroup { get; }
}
