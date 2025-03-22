namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface ICondition
{
    Combination? Combination { get; }
    [Required] string LeftExpression{ get; }
    [Required] string Operator { get; }
    [Required] string RightExpression { get; }
    bool StartGroup { get; }
    bool EndGroup { get; }
}
