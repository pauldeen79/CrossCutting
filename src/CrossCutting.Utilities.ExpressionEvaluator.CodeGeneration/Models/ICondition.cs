namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface ICondition
{
    Combination? Combination { get; }
    [Required] string LeftExpression { get; }
    [Required] Abstractions.IOperator Operator { get; }
    [Required] string RightExpression { get; }
    bool StartGroup { get; }
    bool EndGroup { get; }
}
