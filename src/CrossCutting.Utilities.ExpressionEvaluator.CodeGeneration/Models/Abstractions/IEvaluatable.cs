namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Abstractions;

internal interface IEvaluatable
{
}

#pragma warning disable S2326 // Unused type parameters should be removed
internal interface IEvaluatable<T> : IEvaluatable
{
#pragma warning restore S2326 // Unused type parameters should be removed
}
