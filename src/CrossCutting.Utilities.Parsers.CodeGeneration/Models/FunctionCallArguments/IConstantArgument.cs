namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models.FunctionCallArguments;

internal interface IConstantArgument : IFunctionCallArgumentBase
{
    object? Value { get; }
}

internal interface IConstantArgument<T> : IFunctionCallArgumentBase, Abstractions.IFunctionCallArgument<T>
{
    T Value { get; }
}
