namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface ICondition
{
    Combination? Combination { get; }
    [Required] string LeftExpression{ get; }
    [Required]
    //[CsharpTypeName("System.Func<System.Collections.Generic.Dictionary<System.String, CrossCutting.Common.Results.Result>, StringComparison, CrossCutting.Common.Results.Result<bool>>")] string
    Func<Dictionary<string, Result>, StringComparison, Result<bool>> Operator { get; }
    [Required] string RightExpression { get; }
    bool StartGroup { get; }
    bool EndGroup { get; }
}
