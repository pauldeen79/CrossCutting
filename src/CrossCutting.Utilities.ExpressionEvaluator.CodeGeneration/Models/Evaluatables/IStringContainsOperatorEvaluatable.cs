namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IStringContainsOperatorEvaluatable : IEvaluatableBase, IBinaryStringOperator, IEvaluatable<bool>
{
}