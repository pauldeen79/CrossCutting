namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Abstractions;

internal interface IFunctionCallArgument
{
}

#pragma warning disable S2326 // Unused type parameters should be removed
internal interface IFunctionCallArgument<T> : IFunctionCallArgument
#pragma warning restore S2326 // Unused type parameters should be removed
{
}
