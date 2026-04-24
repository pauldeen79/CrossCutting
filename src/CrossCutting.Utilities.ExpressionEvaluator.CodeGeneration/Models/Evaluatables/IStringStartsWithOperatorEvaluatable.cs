namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IStringStartsWithOperatorEvaluatable : IEvaluatableBase, IBinaryStringOperator, IEvaluatable<bool>
{
}