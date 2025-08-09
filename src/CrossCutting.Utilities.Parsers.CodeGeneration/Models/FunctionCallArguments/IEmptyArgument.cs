namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models.FunctionCallArguments;

internal interface IEmptyArgument : IFunctionCallArgumentBase
{
}

internal interface IEmptyArgument<T> : IFunctionCallArgumentBase, Abstractions.IFunctionCallArgument<T>
{
}
