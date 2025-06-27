namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models;

internal interface ICondition
{
    Combination? Combination { get; set; }
    bool StartGroup { get; set; }
    bool EndGroup { get; set; }
}
