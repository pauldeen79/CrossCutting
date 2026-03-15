namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IEmptyEvaluatable : IEvaluatableBase
{
}

#pragma warning disable S2326 // Unused type parameters should be removed
internal interface IEmptyEvaluatable<T> : IEvaluatableBase, IEvaluatable<T>
{
#pragma warning restore S2326 // Unused type parameters should be removed
}