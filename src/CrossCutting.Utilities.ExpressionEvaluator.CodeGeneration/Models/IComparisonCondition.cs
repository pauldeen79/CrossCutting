namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IComparisonCondition
{
    Combination? Combination { get; }
    [Required] string LeftExpression { get; }
    [Required] IOperator Operator { get; }
    [Required] string RightExpression { get; }
    bool StartGroup { get; }
    bool EndGroup { get; }
}
