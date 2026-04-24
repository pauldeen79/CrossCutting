namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IStringEndsWithOperatorEvaluatable : IEvaluatableBase, IBinaryStringOperator, IEvaluatable<bool>
{
}