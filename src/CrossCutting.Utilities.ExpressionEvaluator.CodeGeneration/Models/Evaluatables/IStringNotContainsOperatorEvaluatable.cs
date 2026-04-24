namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IStringNotContainsOperatorEvaluatable : IEvaluatableBase, IBinaryStringOperator, IEvaluatable<bool>
{
}