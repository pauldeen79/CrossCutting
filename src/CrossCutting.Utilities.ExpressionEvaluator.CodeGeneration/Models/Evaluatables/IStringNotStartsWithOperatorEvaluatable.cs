namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IStringNotStartsWithOperatorEvaluatable : IEvaluatableBase, IBinaryStringOperator, IEvaluatable<bool>
{
}